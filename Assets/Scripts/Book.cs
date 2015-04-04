using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Book {

	public static Book BookWithUrl(string path) {
		var loadedText = File.ReadAllText (path, System.Text.UTF8Encoding.UTF8);
		var xmlSerializer = new XmlSerializer(typeof (Book));
		var stringReader = new StringReader(loadedText);
		var loadedBook = (Book) xmlSerializer.Deserialize(stringReader);

		return loadedBook;
	}

	public void Serialize(string path)
	{
		string workingDirectory = Path.GetDirectoryName(path);
		Debug.Log("Directory path: " + workingDirectory);
		organizePicturesOnEveryPage(workingDirectory);

		var xmlSerializer = new XmlSerializer(typeof(Book));
		var stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, this);
		File.WriteAllBytes(path, System.Text.Encoding.UTF8.GetBytes(stringWriter.ToString()));
	}

	public Book()
	{
		Pages = new List<BookPage>();
		Pages.Add(new BookPage(1, "New Page"));
		CurrentPageIndex = 0;
	}

	[XmlElement("Name")]
	public string Name  { get; set; }

	[XmlElement("Author")]   
	public string Author { get; set; }

	[XmlIgnore]
	public string Text = "";

	[XmlArray("Pages"), XmlArrayItem("page")]
	public List<BookPage> Pages { get; set; }

	public int CurrentPageIndex { get; set; }
	public BookPage CurrentPage
	{
		get {
			if (Pages == null || CurrentPageIndex < 0 || CurrentPageIndex >= Pages.Count) {
				Debug.Log("Book was asked for current page, but it doesn't exist");
				return null;
			}
			return Pages[CurrentPageIndex];
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("name={0}, author={1}, text={2}, pages={3}", Name, Author, Text, Pages);
	}

	private void organizePicturesOnEveryPage(string workingDirectory)
	{
		foreach (var page in Pages) {
			page.OrganizePicturesIntoDirectory(workingDirectory);
		}
	}
}
