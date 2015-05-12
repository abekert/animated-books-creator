using UnityEngine;
using System.Collections.Generic;
using System.IO;
using BookModel;

#if UNITY_EDITOR
using UnityEditor;

namespace UI
{
	public class BookWindow : EditorWindow
	{
		private Book book {
			get {
				return BookComponent.CurrentBook;
			}
			set {
				BookComponent.CurrentBook = value;
			}
		}

		void OnGUI () {
			GUILayout.Label ("The Book", EditorStyles.boldLabel);

			if (GUILayout.Button ("Create new")) {
				BookComponent.ClearScene ();
				bookPath = string.Empty;
				book = Book.TemplateBook();
				BookComponent.LoadScene ();
			}

			if (GUILayout.Button ("Open existing")) {
				OpenExistingBookButtonPressed ();
			}

			if (GUILayout.Button ("Save")) {
				SaveBookButtonPressed ();
			}

			GUILayout.Label ("Attributes", EditorStyles.boldLabel);

			book.Name = EditorGUILayout.TextField ("Title", book.Name);
			book.Author = EditorGUILayout.TextField ("Author", book.Author);
			EditorStyles.textField.wordWrap = true;
			book.Annotation = EditorGUILayout.TextField ("Annotation", book.Annotation, GUILayout.Height (100), GUILayout.ExpandHeight(true));
		}

		static private string bookPath;
		public void OpenExistingBookButtonPressed()
		{
			Debug.Log ("OpenExistingBookButtonPressed");
			var path = EditorUtility.OpenFilePanel(
				"Select a book to open",
				"",
				"xml");
			if (path.Length != 0 && File.Exists(path)) {
				book = Book.BookWithUrl(path);
				bookPath = path;
				Debug.Log (book);

				BookComponent.ReloadScene ();
				PicturesWindow.UpdatePicturesList();
			}
		}
		
		public void SaveBookButtonPressed()
		{
			Debug.Log ("SaveBookButtonPressed");
			var path = EditorUtility.SaveFilePanel(
				"Save book",
				"",
				bookFileName() + ".abook",
				"xml");

			if (path == null || path == string.Empty) {
				return;
			}

			book.Serialize(path);
			Debug.Log ("Saved " + book + " to path: " + path);
		}

		private string bookFileName()
		{
			if (bookPath != null && bookPath != string.Empty) {
				return Path.GetFileNameWithoutExtension (bookPath);
			}
			if (book.Author != null && book.Author != string.Empty && book.Name != null && book.Name != string.Empty) {
				return book.Author + " — " + book.Name;
			}
			if (book.Name != null && book.Name != string.Empty) {
				return book.Name;
			}
			if (book.Author != null && book.Author != string.Empty) {
				return book.Author;
			}

			return "The Greatest Book Ever";
		}

	}
}

#endif

