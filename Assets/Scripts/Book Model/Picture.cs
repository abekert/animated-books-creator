using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Helpers;

namespace BookModel
{
	public partial class Picture : IPositionable
	{
		[XmlElement("Filename")]   
		public string Filename { set; get; }

		[XmlAttribute("Name")]
		public string Name { set; get; }

		private ABPosition position = new ABPosition();
		[XmlElement("Position")]
		public ABPosition Position {
			get {
				return position;
			}
			set {
				position = value;
				updatePictureObject();
			}
		}

		private Vector3 rotation = new Vector3();
		[XmlElement("Rotation")]
		public Vector3 Rotation {
			get {
				return rotation;
			}
			set {
				rotation = value;
				updatePictureObject();
			}
		}

		private Vector2 imageScale = new Vector2(1, 1);
		[XmlElement("Scale")]
		public Vector2 ImageScale {
			get {
				return imageScale;
			}
			set {
				imageScale = value;
				updatePictureObject();
			}
		}

		[XmlElement("Animation")]
		public ABAnimation Animation;

		[XmlIgnore]
		string filepath;
		[XmlIgnore]
		public GameObject pictureObject;

		private void updatePictureObject()
		{
			if (pictureObject == null) {
				return;
			}
			pictureObject.transform.position = position.OnScreenPosition();
			pictureObject.transform.eulerAngles = rotation;
			pictureObject.transform.localScale = imageScale;
		}

		public Picture()
		{

		}
		
		public Picture(string originalFilepath)
		{
			filepath = originalFilepath;
		}

		public Picture(string originalFilepath, string name) : this(originalFilepath)
		{
			Name = name;
		}

		public bool moveIntoPath(string destinationPath)
		{
			Filename = Path.GetFileName(destinationPath);

			if (destinationPath == filepath) {
				return true;
			}

			try {
				File.Copy(filepath, destinationPath, true);
			} catch (IOException e) {
				Debug.Log(e.Message);
				return false;
			}
			return true;
		}

		private Texture2D loadTexture()
		{
			Book book = BookComponent.CurrentBook;

			if (book.IsFromResourcesFolder) {
				Texture2D rtexture = Resources.Load(Path.GetFileNameWithoutExtension(Filename)) as Texture2D;
				return rtexture;
			}

			// Prepare filepath if the image is just loaded
			if (filepath == null) {
				filepath = Path.Combine(BookComponent.CurrentBook.WorkingDirectory, Filename);
			}
			
			if (!File.Exists (filepath)) {
				Debug.Log("Picture doesn't exist at path: " + filepath);
				return null;
			}
			
			// Load sprite from a file
			byte[] fileData = File.ReadAllBytes(filepath);
			var texture = new Texture2D(2, 2);
			texture.LoadImage(fileData);

			return texture;
		}

		public void AddToTheScene()
		{
			var texture = loadTexture ();
			var sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));

			// Add to the scene
			pictureObject = new GameObject("Picture Object");
			var renderer = pictureObject.AddComponent<SpriteRenderer>();
			renderer.sprite = sprite;
			var mouseDrag = pictureObject.AddComponent<MouseDrag>();
			mouseDrag.PositionableObject = this;
			pictureObject.AddComponent<BoxCollider> ();
			pictureObject.transform.SetParent (BookComponent.BookObject.transform);

			// Set object's variables
			updatePictureObject ();
		}

		public void RemoveFromTheScene() {
			GameObject.Destroy (pictureObject);
			pictureObject = null;
		}

		public void RepositionBegan () {
#if UNITY_EDITOR
			UI.PicturesWindow.CurrentPicture = this;
#endif
		}
	}
}