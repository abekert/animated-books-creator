using UnityEngine;
using System.Xml.Serialization;

namespace Helpers
{
	public class ABColor
	{
		[XmlAttribute ("red")]
		public float Red = 0;

		[XmlAttribute ("green")]
		public float Green = 0;

		[XmlAttribute ("blue")]
		public float Blue = 0;

		public Color ToColor() {
			return new Color (Red, Green, Blue);
		}

		public ABColor ()
		{
		}

		public ABColor (float Red, float Green, float Blue)
		{
			this.Red = Red;
			this.Green = Green;
			this.Blue = Blue;
		}

		public ABColor (Color color)
		{
			this.Red = color.r;
			this.Green = color.g;
			this.Blue = color.b;
		}
	}
}