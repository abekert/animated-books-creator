using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class ButtonsDelegate : MonoBehaviour {

	public void OpenExistingBookButtonPressed() {
#if UNITY_EDITOR
		Debug.Log ("OpenExistingBookButtonPressed");
		var path = EditorUtility.OpenFilePanel(
			"Select a book to open",
			"",
			"xml");
		if (path.Length != 0) {
			Book book = Book.BookWithUrl(path);
			Debug.Log (book);
		}

#elif
		Debug.Log ("You can't use this function outside the Unity Editor");
#endif
	}

	public void SaveBookButtonPressed() {
#if UNITY_EDITOR
		Debug.Log ("SaveBookButtonPressed");
		var path = EditorUtility.SaveFilePanel(
			"Save book",
			"",
			"book" + ".abook",
			"xml");

		Book book = new Book();
		book.name = "Алиса в стране чудес";
		book.author = "Льюис Кэрролл";
		book.text = "сказка, написанная английским математиком, поэтом и писателем Чарльзом Лютвиджом Доджсоном";

		book.pages = new List<BookPage>();
		book.pages.Add(new BookPage(1, "Первая страница"));
        book.pages.Add(new BookPage(2, "А это уже вторая страница"));

		book.Serialize(path);
#elif
		Debug.Log ("You can't use this function outside the Unity Editor");
#endif
	}
	
}