using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Helpers
{
	public class ABPosition
	{
		private float x = 0.5f;
		[XmlElement("x")]
		public float X {
			get {
				return x;
			}
			set {
				if (value < 0) {
					x = 0;
				} else if (value > 1) {
					x = 1;
				} else {
					x = value;
				}
			}
		}

		private float y = 0.5f;
		[XmlElement("y")]
		public float Y {
			get {
				return y;
			}
			set {
				if (value < 0) {
					y = 0;
				} else if (value > 1) {
					y = 1;
				} else {
					y = value;
				}
			}
		}

		private float z = 0.5f;
		[XmlElement("z")]
		public float Z {
			get {
				return z;
			}
			set {
				z = value;
//				if (value < 0) {
//					z = 0;
//				} else if (value > 1) {
//					z = 1;
//				} else {
//					z = value;
//				}
			}
		}

		public ABPosition ()
		{
		}

		public ABPosition (float x, float y, float z) : this()
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public ABPosition (Vector3 vector) : this()
		{
			this.X = vector.x;
			this.Y = vector.y;
			this.Z = vector.z;
		}

		public override string ToString ()
		{
			return string.Format ("Position X = {0}, Y = {1}, Z = {2}", X, Y, Z);
		}

		private Vector2 screenSize()
		{
#if UNITY_EDITOR
			var screen = Handles.GetMainGameViewSize ();
#else
			var resolution = Screen.currentResolution;
			var screen = new Vector2(resolution.width, resolution.height);
#endif
			return screen;
		}

		public Vector3 OnScreenPosition()
		{
			var screen = screenSize ();
//			Debug.Log (string.Format ("Screen size Width = {0}, Height = {1}", screen.x, screen.y));
			float aspect = screen.x / screen.y;
			float screenX = (x - 0.5f) * 100 * aspect;
			float screenY = (y - 0.5f) * 100f;
//			Debug.Log (string.Format ("OnScreen position X = {0}, Y = {1}", screenX, screenY));
			return new Vector3 (screenX, screenY, z);
		}

		public void convertFromOnScreenPosition(Vector3 OnScreenPosition)
		{
			var screen = screenSize ();
//			var screen = Handles.GetMainGameViewSize ();
//			Debug.Log (string.Format ("Screen size Width = {0}, Height = {1}", screen.x, screen.y));
			float aspect = screen.x / screen.y;
			x = (OnScreenPosition.x / 100f / aspect) + 0.5f;
			y = (OnScreenPosition.y / 100f) + 0.5f;
			z = OnScreenPosition.z;
		}

		public Vector3 ToVector() {
			return new Vector3 (x, y, z);
		}
	}
}

