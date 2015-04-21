using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class BookPage
{
	public BookPage ()
	{
	}

	public BookPage (int number, string text) : this()
	{
		this.Number = number;
		this.Text = text;
	}

	[XmlAttribute("Number")]
	public int Number { get; set; }
	
	[XmlElement("Text")]   
	public string Text { get; set; }

	[XmlArray("Pictures"), XmlArrayItem("Picture")]
	public List<Picture> Pictures = new List<Picture>();
	
	public override string ToString ()
	{
		return string.Format ("Page number {0}. {1}. Contains {2} pictures", Number, Text, Pictures.Count);
	}

	public void OrganizePicturesIntoDirectory(string directory)
	{
		if (Pictures == null || Pictures.Count == 0) {
			return;
		}

		for (int pictureNumber = 1; pictureNumber <= Pictures.Count; pictureNumber++) {
			var picture = Pictures[pictureNumber - 1];
			var filename = string.Format("page-{0}-picture-{1}.png", Number, pictureNumber);
			var destinationPath = Path.Combine(directory, filename);
			Debug.Log("Picture destination: " + destinationPath);

			picture.moveIntoPath(destinationPath);
		}
	}
}
