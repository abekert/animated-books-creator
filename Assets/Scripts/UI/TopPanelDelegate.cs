using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace UI {

	public class TopPanelDelegate : MonoBehaviour
	{
		public void OpenExistingBookButtonPressed()
		{
			#if UNITY_EDITOR
			Debug.Log ("OpenExistingBookButtonPressed");
			var path = EditorUtility.OpenFilePanel(
				"Select a book to open",
				"",
				"xml");
			if (path.Length != 0 && File.Exists(path)) {
				Book book = Book.BookWithUrl(path);
				BookComponent.CurrentBook = book;
				Debug.Log (book);

				PicturesWindow.UpdatePicturesList();

//				book.CurrentPageIndex = 1;
				var page = book.CurrentPage;
				if (page == null) {
					Debug.Log("There are no current page");
				}

				var pictures = page.Pictures;
				if (pictures == null) {
					Debug.Log("There are no pictures on current page");
				}

				foreach (var picture in pictures) {
					picture.AddToTheScene ();
				}

			}
			#elif
				Debug.Log ("You can't use this function outside the Unity Editor");
			#endif
		}

		public void SaveBookButtonPressed()
		{
			#if UNITY_EDITOR
			Debug.Log ("SaveBookButtonPressed");
			var path = EditorUtility.SaveFilePanel(
				"Save book",
				"",
				"book" + ".abook",
				"xml");

			Book book = BookComponent.CurrentBook;
			book.Serialize(path);
			Debug.Log ("Saved " + book + " to path: " + path);


//				Book book = new Book();
//				book.Name = "Алиса в стране чудес";
//				book.Author = "Льюис Кэрролл";
//				book.Text = "сказка, написанная английским математиком, поэтом и писателем Чарльзом Лютвиджом Доджсоном";
//
//				book.Pages = new List<BookPage>();
//				book.Pages.Add(new BookPage(1, "Первая страница"));
//				book.Pages.Add(new BookPage(2, "А это уже вторая страница"));
//
//				book.Serialize(path);
			#elif
				Debug.Log ("You can't use this function outside the Unity Editor");
			#endif
		}

		public void ShowPicturesWindow()
		{
			PicturesWindow window = ScriptableObject.CreateInstance<PicturesWindow>();
			window.ShowAuxWindow();
		}

	}
}