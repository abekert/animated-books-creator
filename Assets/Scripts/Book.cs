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

	public void Serialize(string path) {
		var xmlSerializer = new XmlSerializer(typeof(Book));
		var stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, this);
		File.WriteAllBytes(path, System.Text.Encoding.UTF8.GetBytes(stringWriter.ToString()));
	}

	[XmlElement("Name")]
	public string name  { get; set; }

	[XmlElement("Author")]   
	public string author { get; set; }

	[XmlIgnore]
	public string text = "";

	[XmlArray("Pages"), XmlArrayItem("page")]
	public List<BookPage> pages { get; set; }
	
	public override string ToString ()
	{
		return string.Format ("name={0}, author={1}, text={2}, pages={3}", name, author, text, pages);
	}
}
