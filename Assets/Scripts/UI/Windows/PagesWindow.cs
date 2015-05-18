using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using BookModel;

namespace UI
{
	public class PagesWindow : EditorWindow
	{
		private Book book {
			get {
				return BookComponent.CurrentBook;
			}
		}

		private List<string> pagesDescriptions = new List<string>();
		void OnStart()
		{
		}

		void Update ()
		{
		}

		void OnGUI ()
		{
			GUILayout.Label ("Pages", EditorStyles.boldLabel);
			
			if (GUILayout.Button ("Add new page")) {
				var page = BookPage.EmptyPage(book.CurrentPageIndex + 2);
				book.Pages.Insert(book.CurrentPageIndex + 1, page);
				book.CurrentPageIndex++;
				BookComponent.ReloadScene ();
			}
			
			if (book.Pages.Count > 0) {
				GUILayout.Label ("Go to page", EditorStyles.boldLabel);
//				Debug.Log("BookComponent.CurrentBook.CurrentPageIndex = " + BookComponent.CurrentBook.CurrentPageIndex);
//				Debug.Log(pagesDescriptions.ToArray());
				pagesDescriptions = bookPagesDescriptionsArray (book);
				var index = book.CurrentPageIndex;
				book.CurrentPageIndex = EditorGUILayout.Popup (index, pagesDescriptions.ToArray());
				if (index != book.CurrentPageIndex) {
					BookComponent.ReloadScene ();
				}

				showMoveUpDownControls ();
			}

			showColorControls ();

		}

		private void showMoveUpDownControls ()
		{
			int oldIndex = book.CurrentPageIndex;

			GUILayout.BeginHorizontal ();

			var moveUpEnabled = oldIndex > 0;
			GUI.enabled = moveUpEnabled;
			if (GUILayout.Button ("Move page up")) {
				var page = book.Pages[oldIndex];
				book.Pages.RemoveAt(oldIndex);
				int newIndex = oldIndex - 1;
				book.Pages.Insert(newIndex, page);
				book.CurrentPageIndex = newIndex;
				page.Number = newIndex + 1;
				book.Pages[oldIndex].Number = oldIndex + 1;
			}

			var moveDownEnabled = oldIndex < book.Pages.Count - 1;
			GUI.enabled = moveDownEnabled;
			if (GUILayout.Button ("Move page down")) {
				var page = book.Pages[oldIndex];
				book.Pages.RemoveAt(oldIndex);
				int newIndex = oldIndex + 1;
				book.Pages.Insert(newIndex, page);
				book.CurrentPageIndex = newIndex;
				page.Number = newIndex + 1;
				book.Pages[oldIndex].Number = oldIndex + 1;
			}

			GUI.enabled = true;

			GUILayout.EndHorizontal ();
		}

		private void showColorControls() {
			GUILayout.Label ("Color", EditorStyles.boldLabel);
			
			var color = book.CurrentPage.Color.ToColor ();
			var newColor = EditorGUILayout.ColorField (color);
//			if (color != newColor) {
			book.CurrentPage.Color = new Helpers.ABColor (newColor);
				BookComponent.UpdatePageColor ();
//			}
		}

		private List<string> bookPagesDescriptionsArray(Book book)
		{
			int count = book.Pages.Count;
			List<string> pages = new List<string> (count);
			for (int i = 0; i < count; ++i) {
				pages.Add(book.Pages[i].ToString());
			}
			return pages;
		}

	}
}
#endif