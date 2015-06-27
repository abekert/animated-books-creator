using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace BookModel
{
	public class BookPage
	{
		static public BookPage EmptyPage (int number)
		{
			var sampleText = "This is the page number " + number + System.Environment.NewLine;
			sampleText += "It's time to make it perfect!";

			var page = new BookPage (number, sampleText);

			return page;
		}

		public BookPage ()
		{
		}

		public BookPage (int number, string text) : this()
		{
			this.Number = number;
			this.Text = new Text (text);
		}

		[XmlAttribute("Number")]
		public int Number { get; set; }
		
		[XmlElement("Text")]   
		public Text Text { get; set; }

		[XmlArray("Pictures"), XmlArrayItem("Picture")]
		public List<Picture> Pictures = new List<Picture> ();

		[XmlElement("Color")]
		public ABColor Color = new ABColor (0.2f, 0.3f, 0.6f);

		public override string ToString ()
		{
			int textLengthLimit = 30;
			string limitedText = "";
			if (Text != null && Text.Content != string.Empty) {
				if (Text.Content.Length <= textLengthLimit) {
					limitedText = Text + ". ";
				} else {
					limitedText = Text.Content.Substring (0, textLengthLimit) + "... ";
				}
			}
			return string.Format ("{0}. {1}{2} pictures", Number, limitedText, Pictures.Count);
		}

		public void OrganizePicturesIntoDirectory (string directory)
		{
			if (Pictures == null || Pictures.Count == 0) {
				return;
			}

			for (int pictureNumber = 1; pictureNumber <= Pictures.Count; pictureNumber++) {
				var picture = Pictures [pictureNumber - 1];
				var filename = string.Format ("page-{0}-picture-{1}.png", Number, pictureNumber);
				var destinationPath = Path.Combine (directory, filename);
				Debug.Log ("Picture destination: " + destinationPath);

				picture.MoveIntoPath (destinationPath);
			}
		}

		public void ClearPicturesIfNeeded ()
		{
			foreach (var item in Pictures) {
				item.RemoveFromDiscIfInBookDirectory ();
			}
		}
	}
}
