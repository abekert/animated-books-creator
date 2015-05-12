using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using BookModel;

namespace UI
{
	public class PicturesWindow : EditorWindow
	{
		static List<string> picturesNames = new List<string>();
		static int CurrentPictureIndex = 0;
		static public Picture CurrentPicture {
			get {
				var page = BookComponent.CurrentBook.CurrentPage;
				if (page == null || page.Pictures == null) {
					return null;
				}
				if (CurrentPictureIndex < 0 || CurrentPictureIndex >= page.Pictures.Count) {
					return null;
				}
				return page.Pictures [CurrentPictureIndex];
			}
			set {
				var page = BookComponent.CurrentBook.CurrentPage;
				if (page == null || page.Pictures == null) {
					return;
				}
				CurrentPictureIndex = page.Pictures.IndexOf(value);
			}
		}

		static private bool isRenamingCurrentPicture = false;


		void OnGUI () {
			GUILayout.Label ("Pictures", EditorStyles.boldLabel);

			if (GUILayout.Button ("Add a picture")) {
				addPictureButtonPressed ();
			}

			UpdatePicturesList ();
			if (picturesNames.Count > 0) {
				GUILayout.Label ("Choose a picture", EditorStyles.boldLabel);
				showExistingPicturesList();
				showPositionControls();
				showRotationControls();
				showScaleControls();
			}
		}

		private void showExistingPicturesList() {
			EditorGUILayout.BeginHorizontal ();
			if (isRenamingCurrentPicture) {
				picturesNames[CurrentPictureIndex] = GUILayout.TextField(picturesNames[CurrentPictureIndex]);
				if (GUILayout.Button ("OK", GUILayout.ExpandWidth(false))) {
					var page = BookComponent.CurrentBook.CurrentPage;
					page.Pictures[CurrentPictureIndex].Name = picturesNames[CurrentPictureIndex];
					isRenamingCurrentPicture = false;
				}
			} else {
				CurrentPictureIndex = EditorGUILayout.Popup (CurrentPictureIndex, picturesNames.ToArray ());
				if (GUILayout.Button ("Rename")) {
					isRenamingCurrentPicture = true;
				}
				if (GUILayout.Button ("Delete")) {
					deleteCurrentPictureButtonPressed ();
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void showPositionControls() {
			var pos = CurrentPicture.Position.ToVector();
			pos = EditorGUILayout.Vector3Field ("Position", pos);
			CurrentPicture.Position = new Helpers.ABPosition(pos);
		}

		private void showRotationControls() {
			CurrentPicture.Rotation = EditorGUILayout.Vector3Field ("Rotation", CurrentPicture.Rotation);
		}

		private void showScaleControls() {
			CurrentPicture.ImageScale = EditorGUILayout.Vector2Field ("Scale", CurrentPicture.ImageScale);

			float scale = 1; 
			scale = EditorGUILayout.Slider (scale, 0.98f, 1.02f);
			CurrentPicture.ImageScale *= scale;
		}


		private void addPictureButtonPressed() {
			Debug.Log ("OpenExistingBookButtonPressed");
			var path = EditorUtility.OpenFilePanel(
				"Select a picture",
				"",
				"png");
			if (path.Length != 0) {
				Debug.Log("Open picture at path: " + path);
				
				// Add picture to page of the book
				var name = Path.GetFileNameWithoutExtension(path);
				var picture = new Picture(path, name);
				var page = BookComponent.CurrentBook.CurrentPage;
				if (page.Pictures == null) {
					page.Pictures = new List<Picture>();
				}
				page.Pictures.Add(picture);
				picture.AddToTheScene ();

				// Combo box
				picturesNames.Add(picture.Name);
				isRenamingCurrentPicture = true;
				CurrentPictureIndex = picturesNames.Count - 1;
			}
		}
		
		private void deleteCurrentPictureButtonPressed() {
			var page = BookComponent.CurrentBook.CurrentPage;
			if (page.Pictures == null) {
				throw new IOException("The page doesn't contain any picture to delete");
			}
			page.Pictures [CurrentPictureIndex].RemoveFromTheScene ();
			page.Pictures.RemoveAt (CurrentPictureIndex);
			picturesNames.RemoveAt (CurrentPictureIndex);
			if (CurrentPictureIndex > 0) {
				CurrentPictureIndex--;
			}
		}

		static public void UpdatePicturesList()
		{
			picturesNames.Clear();
			var page = BookComponent.CurrentBook.CurrentPage;
			if (page == null || page.Pictures == null) {
				return;
			}
			foreach (var picture in page.Pictures) {
				picturesNames.Add(picture.Name);
			}
		}
		
	}
}

#endif
