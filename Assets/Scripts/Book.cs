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
		loadedBook.WorkingDirectory = Path.GetDirectoryName(path);

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

	public static Book TemplateBook() {
		Book book = new Book();
		book.Name = "New Book";
		book.Author = "The Grand Author";

		var page = new BookPage ();
		page.Number = 1;
		page.Pictures = new List<Picture> ();
		book.Pages = new List<BookPage> ();
		book.Pages.Add (page);
		book.CurrentPageIndex = 0;

		return book;
	}

	public Book()
	{
//		Pages.Add(new BookPage(1, "New Page"));
//		CurrentPageIndex = 0;
	}

	[XmlIgnore]
	public string WorkingDirectory = "";

	[XmlElement("Name")]
	public string Name  { get; set; }

	[XmlElement("Author")]   
	public string Author { get; set; }

	[XmlIgnore]
	public string Text = "";

	[XmlArray("Pages"), XmlArrayItem("Page")]
	public List<BookPage> Pages = new List<BookPage> ();

	[XmlIgnore]
	public int CurrentPageIndex = 0;

	[XmlIgnore]
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
		var pagesString = "";
		foreach (var page in Pages) {
			pagesString = pagesString + page.ToString() + System.Environment.NewLine;
		}
		return string.Format ("name={0}, author={1}, text={2}, pages count={3}. Pages content: {4}", Name, Author, Text, Pages.Count, pagesString);
	}

	private void organizePicturesOnEveryPage(string workingDirectory)
	{
		foreach (var page in Pages) {
			page.OrganizePicturesIntoDirectory(workingDirectory);
		}
	}
}
