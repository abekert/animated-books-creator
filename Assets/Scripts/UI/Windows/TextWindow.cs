using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using BookModel;

namespace UI
{
	public class TextWindow : EditorWindow
	{
		Text text {
			get {
				var page = BookComponent.CurrentBook.CurrentPage;
				if (page == null) {
					return null;
				}
				return page.text;
			}
			set {
				var page = BookComponent.CurrentBook.CurrentPage;
				if (page == null) {
					return;
				}
				page.text = value;
			}
		}

		static private string[] textAlignments = { "Left", "Center", "Right" };

		void OnGUI () {
			GUILayout.Label ("Text", EditorStyles.boldLabel);

			showPositionControls ();
			showRotationControls ();
			showScaleControls ();
			showAlignmentControls ();
			showColorControls ();

			GUILayout.Label ("Content", EditorStyles.boldLabel);
			EditorStyles.textField.wordWrap = true;
			EditorStyles.textField.richText = true;

			GUILayoutOption[] options = { GUILayout.Height (200), GUILayout.ExpandHeight (true) };
			text.Content = EditorGUILayout.TextArea (text.Content, options);
		}

		private void showPositionControls() {
			var pos = text.Position.ToVector();
			pos = EditorGUILayout.Vector3Field ("Position", pos);
			text.Position = new Helpers.ABPosition(pos);
		}
		
		private void showRotationControls() {
			text.Rotation = EditorGUILayout.Vector3Field ("Rotation", text.Rotation);
		}
		
		private void showScaleControls() {
			text.Scale = EditorGUILayout.Vector2Field ("Scale", text.Scale);

			float scale = 1; 
			scale = EditorGUILayout.Slider (scale, 0.98f, 1.02f);
			text.Scale *= scale;
		}

		private void showAlignmentControls () {
			int index = (int)text.TextAlignment;
			index = EditorGUILayout.Popup ("Alignment", index, textAlignments);
			text.TextAlignment = (TextAlignment)index;
		}

		private void showColorControls() {
			GUILayout.Label ("Color", EditorStyles.boldLabel);

			var color = text.Color.ToColor ();
			color = EditorGUILayout.ColorField (color);
			text.Color = new Helpers.ABColor (color);
		}

	}
}
#endif
