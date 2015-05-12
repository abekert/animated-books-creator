using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

namespace Helpers
{

	public class ABTextAlignment
	{
		[XmlIgnore]
		public TextAlignment textAlignment;
		[XmlAttribute ("Alignment")]
		public string TextAlignmentString
		{
			get {
				return TextAlignmentToString(textAlignment);
			}

			set {
				textAlignment = TextAlignmentFromString(value);
			}
		}

		public ABTextAlignment ()
		{
		}

		static public string TextAlignmentToString(TextAlignment textAlignment)
		{
			switch (textAlignment) {
			case TextAlignment.Center:
				return "Center";
			case TextAlignment.Left:
				return "Left";
			default:
				return "Right";
			}
		}

		static public TextAlignment TextAlignmentFromString(string textAlignmentString)
		{
			switch (textAlignmentString) {
			case "Center":
				return TextAlignment.Center;
			case "Left":
				return TextAlignment.Left;
			default:
				return TextAlignment.Right;
			}
		}
	}
}