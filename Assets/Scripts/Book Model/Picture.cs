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
				UpdateGameObject();
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
				UpdateGameObject();
			}
		}

		private Vector2 scale = new Vector2(1, 1);
		[XmlElement("Scale")]
		public Vector2 Scale {
			get {
				return scale;
			}
			set {
				scale = value;
				UpdateGameObject();
			}
		}

		private float alpha = 1;
		[XmlElement("Alpha")]
		public float Alpha {
			get {
				return alpha;
			}
			set {
				alpha = Mathf.Max (Mathf.Min (1, value), 0);
				UpdateGameObject();
			}
		}

		
		[XmlElement("Animation")]
		public ABAnimation Animation;

		[XmlIgnore]
		string filepath;
		[XmlIgnore]
		public GameObject GameObject;

		public void UpdateGameObject ()
		{
			if (GameObject == null) {
				return;
			}
			GameObject.transform.position = position.OnScreenPosition();
			GameObject.transform.eulerAngles = rotation;
			GameObject.transform.localScale = scale;

			var renderer = GameObject.GetComponent<SpriteRenderer> ();
			var color = renderer.color;
			color.a = alpha;
			renderer.color = color;
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

		public bool MoveIntoPath(string destinationPath)
		{
			Filename = Path.GetFileName(destinationPath);

			if (filepath == null) {
				filepath = Path.Combine(BookComponent.CurrentBook.WorkingDirectory, Filename);
			}

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

		public void AddToScene(bool isDraggable = true)
		{
			var texture = loadTexture ();
			var sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));

			// Add to the scene
			GameObject = new GameObject("Picture Object");
			var renderer = GameObject.AddComponent<SpriteRenderer>();
			renderer.sprite = sprite;
			GameObject.transform.SetParent (BookComponent.BookObject.transform);

			if (isDraggable) {
				var mouseDrag = GameObject.AddComponent<MouseDrag>();
				mouseDrag.PositionableObject = this;
				GameObject.AddComponent<BoxCollider> ();
			}

			// Set object's variables
			UpdateGameObject ();
		}

		public void RemoveFromScene() {
			GameObject.Destroy (GameObject);
			GameObject = null;
		}

		public void RemoveFromDiscIfInBookDirectory ()
		{
			var bookDirectory = BookComponent.CurrentBook.WorkingDirectory;
			if (filepath == null) {
				filepath = Path.Combine(bookDirectory, Filename);
				if (!File.Exists (filepath)) {
					File.Delete (filepath);
				}
				return;
			}

			var fileDirectory = Path.GetDirectoryName (filepath);
			if (fileDirectory == bookDirectory) {
				File.Delete (filepath);
			}
		}

		public void RepositionBegan () {
#if UNITY_EDITOR
			UI.PicturesWindow.CurrentPicture = this;
#endif
		}
	}
}