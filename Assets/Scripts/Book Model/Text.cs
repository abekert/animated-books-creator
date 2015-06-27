using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Helpers;

namespace BookModel
{
	public class Text : IPositionable
	{
		private string content;

		[XmlElement("Content")]   
		public string Content {
			set {
				content = value;
				if (textMesh != null) {
					textMesh.text = content;
					var size = GameObject.GetComponent<MeshRenderer> ().bounds.size;
					GameObject.GetComponent <BoxCollider> ().size = size;
				}
			}
			get {
				return content;
			}
		}
		
		private ABPosition position = new ABPosition (0.5f, 0.5f, 40);

		[XmlElement("Position")]
		public ABPosition Position {
			get {
				return position;
			}
			set {
				position = value;
				updateGameObject ();
			}
		}
		
		private Vector3 rotation = new Vector3 ();

		[XmlElement("Rotation")]
		public Vector3 Rotation {
			get {
				return rotation;
			}
			set {
				rotation = value;
				updateGameObject ();
			}
		}
		
		private Vector2 scale = new Vector2 (1, 1);
		[XmlElement("Scale")]
		public Vector2 Scale {
			get {
				return scale;
			}
			set {
				scale = value;
				updateGameObject ();
			}
		}

		private ABColor color = new ABColor(1, 1, 1);
		[XmlElement("Color")]
		public ABColor Color {
			get {
				return color;
			}
			set {
				color = value;
				if (textMesh != null) {
					textMesh.color = color.ToColor();
				}
			}
		}

		private TextAlignment textAlignment = TextAlignment.Left;
		[XmlIgnore]
		public TextAlignment TextAlignment {
			get {
				return textAlignment;
			}
			set {
				textAlignment = value;
				if (textMesh != null) {
					textMesh.alignment = textAlignment;
				}
			}
		}
		[XmlAttribute("Alignment")]
		public string TextAlignmentString {
			get {
				return ABTextAlignment.TextAlignmentToString(textAlignment);
			}
			set {
				textAlignment = ABTextAlignment.TextAlignmentFromString(value);
			}
		}

		[XmlIgnore]
		public GameObject GameObject;
		private TextMesh textMesh;
		
		private void updateGameObject ()
		{
			if (GameObject == null) {
				return;
			}
			GameObject.transform.position = position.OnScreenPosition ();
			GameObject.transform.eulerAngles = rotation;
			GameObject.transform.localScale = scale;

			textMesh.color = color.ToColor ();
			textMesh.alignment = textAlignment;
		}
		
		public void UpdatePositionByObject ()
		{
			position.convertFromOnScreenPosition (GameObject.transform.position);
		}
		
		public Text ()
		{
		}
		
		public Text (string content) : base ()
		{
			this.Content = content;
		}
		
		public void AddToTheScene (bool isDraggable = true, string fontname = "Arial")
		{
			GameObject = new GameObject ("Text Object");
			textMesh = GameObject.AddComponent<TextMesh> ();

			GameObject.GetComponent<MeshRenderer> ().material = Resources.Load<Material> (fontname);
			var myFont = Resources.Load<Font> (fontname);
			textMesh.font = myFont;
			textMesh.text = Content;
			textMesh.fontSize = 72;
			textMesh.alignment = UnityEngine.TextAlignment.Center;
			textMesh.anchor = TextAnchor.MiddleCenter;

			GameObject.transform.SetParent (BookComponent.BookObject.transform);

			if (isDraggable) {
				GameObject.AddComponent<BoxCollider> ();
				var mouseDrag = GameObject.AddComponent<MouseDrag> ();
				mouseDrag.PositionableObject = this;
			}

			// Set object's variables
			updateGameObject ();
		}
		
		public void RemoveFromTheScene ()
		{
			GameObject.Destroy (GameObject);
			GameObject = null;
		}

		public void RepositionBegan ()
		{

		}
	}
}