using UnityEngine;

namespace Helpers
{
	public class ABColor
	{
		public float Red;
		public float Green;
		public float Blue;

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