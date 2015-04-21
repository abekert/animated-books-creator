using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Helpers;

public class Picture
{
	[XmlElement("Filename")]   
	public string Filename { set; get; }

	[XmlAttribute("Name")]
	public string Name { set; get; }

	private Position imagePosition = new Position();
	[XmlElement("Position")]
	public Position ImagePosition {
		get {
			return imagePosition;
		}
		set {
			imagePosition = value;
			updatePictureObject();
		}
	}

	[XmlIgnore]
	string filepath;
	[XmlIgnore]
	public GameObject pictureObject;

	private void updatePictureObject()
	{
		if (pictureObject == null) {
			return;
		}
		pictureObject.transform.position = imagePosition.OnScreenPosition();
	}

	public void UpdatePositionByPictureObject () {
		imagePosition.importOnScreenPosition (pictureObject.transform.position);
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

	public void AddToTheScene()
	{
		// Prepare filepath if the image is just loaded
		if (filepath == null) {
			filepath = Path.Combine(BookComponent.CurrentBook.WorkingDirectory, Filename);
		}

		if (!File.Exists (filepath)) {
			Debug.Log("Picture doesn't exist at path: " + filepath);
			return;
		}

		// Load sprite from a file
		byte[] fileData = File.ReadAllBytes(filepath);
		var texture = new Texture2D(2, 2);
		texture.LoadImage(fileData);
		var sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
		
		// Add to the scene
		pictureObject = new GameObject("Picture Object");
		pictureObject.transform.position = ImagePosition.OnScreenPosition ();
		var renderer = pictureObject.AddComponent<SpriteRenderer>();
		renderer.sprite = sprite;
		var mouseDrag = pictureObject.AddComponent<MouseDrag>();
		mouseDrag.Picture = this;
		pictureObject.AddComponent<BoxCollider2D> ();
	}

	public void RemoveFromTheScene() {
		GameObject.Destroy (pictureObject);
		pictureObject = null;
	}
}