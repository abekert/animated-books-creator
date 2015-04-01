using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class BookPage {

	public BookPage() {

	}

	public BookPage(int number, string text) {
		this.number = number;
		this.text = text;
	}

	[XmlAttribute("Number")]
	public int number { get; set; }
	
	[XmlElement("Text")]   
	public string text { get; set; }

	public override string ToString ()
	{
		return string.Format ("Page number {0}. {1}", number, text);
	}
}
