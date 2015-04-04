using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Picture
{
	[XmlElement("Filename")]   
	public string Filename { set; get; }

	[XmlAttribute("Name")]
	public string Name { set; get; }

	[XmlIgnore]
	string filepath;

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
}