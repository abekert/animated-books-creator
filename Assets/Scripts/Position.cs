using UnityEngine;
using UnityEditor;

namespace Helpers
{
	public class Position
	{
		private float x = 0.5f;
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

		public Position ()
		{
		}

		public Position (float x, float y, float z) : this()
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public override string ToString ()
		{
			return string.Format ("Position X = {0}, Y = {1}, Z = {2}", X, Y, Z);
		}

		public Vector3 OnScreenPosition()
		{
			var screen = Handles.GetMainGameViewSize ();
//			Debug.Log (string.Format ("Screen size Width = {0}, Height = {1}", screen.x, screen.y));
			float aspect = screen.x / screen.y;
			float screenX = (x - 0.5f) * 20 * aspect;
			float screenY = (y - 0.5f) * 30;
//			Debug.Log (string.Format ("OnScreen position X = {0}, Y = {1}", screenX, screenY));
			return new Vector3 (screenX, screenY, z);
		}

		public void importOnScreenPosition(Vector3 OnScreenPosition)
		{
			var screen = Handles.GetMainGameViewSize ();
//			Debug.Log (string.Format ("Screen size Width = {0}, Height = {1}", screen.x, screen.y));
			float aspect = screen.x / screen.y;
			x = (OnScreenPosition.x / 20f / aspect) + 0.5f;
			y = (OnScreenPosition.y / 30f) + 0.5f;
			z = OnScreenPosition.z;
		}
	}
}

