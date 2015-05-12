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
			
			if (BookComponent.CurrentBook.Pages.Count > 0) {
				GUILayout.Label ("Go to page", EditorStyles.boldLabel);
//				Debug.Log("BookComponent.CurrentBook.CurrentPageIndex = " + BookComponent.CurrentBook.CurrentPageIndex);
//				Debug.Log(pagesDescriptions.ToArray());
				pagesDescriptions = bookPagesDescriptionsArray (book);
				var index = book.CurrentPageIndex;
				book.CurrentPageIndex = EditorGUILayout.Popup (index, pagesDescriptions.ToArray());
				if (index != book.CurrentPageIndex) {
					BookComponent.ReloadScene ();
				}
			}
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