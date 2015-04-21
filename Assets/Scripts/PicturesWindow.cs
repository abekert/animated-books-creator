using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;


public class PicturesWindow : EditorWindow {

	// Add menu named "My Window" to the Window menu
//	[MenuItem ("Window/Pictures")]
//	static void Init () {
//		// Get existing open window or if none, make a new one:
//		PicturesWindow window = (PicturesWindow)EditorWindow.GetWindow (typeof (PicturesWindow));
//		window.ShowAuxWindow();
//	}

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
	}

	static private bool isRenamingCurrentPicture = false;


	void OnGUI () {
		GUILayout.Label ("Pictures", EditorStyles.boldLabel);

		if (GUILayout.Button ("Add a picture")) {
			addPictureButtonPressed ();
		}

		if (picturesNames.Count > 0) {
			GUILayout.Label ("Choose a picture", EditorStyles.boldLabel);
			showExistingPicturesList();
			showPositionControls();
		}
	}

	private void showExistingPicturesList() {
		EditorGUILayout.BeginHorizontal ();
		if (isRenamingCurrentPicture) {
//			GUI.SetNextControlName ("TextField");
			picturesNames[CurrentPictureIndex] = GUILayout.TextField(picturesNames[CurrentPictureIndex]);
//			GUI.FocusControl ("TextField");
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
		GUILayout.Label ("Position", EditorStyles.boldLabel);
//		EditorGUILayout.BeginHorizontal ();
//		GUILayout.Label ("Position");
		var pos = CurrentPicture.ImagePosition;
//		pos.X = EditorGUILayout.Slider ((int)(pos.X * 100), 0, 99) / 100f;
		pos.X = EditorGUILayout.FloatField ("X", pos.X);
		pos.Y = EditorGUILayout.FloatField ("Y", pos.Y);
		pos.Z = EditorGUILayout.FloatField ("Z", pos.Z);
		CurrentPicture.ImagePosition = pos;
//		EditorGUILayout.EndHorizontal ();
	}

	private void addPictureButtonPressed() {
		#if UNITY_EDITOR
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
		
		#elif
		Debug.Log ("You can't use this function outside the Unity Editor");
		#endif

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
