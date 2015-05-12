using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BookModel;

public class DeployerScript : MonoBehaviour {

	public string Name = "book.xml";
	static public Book book;

	// Use this for initialization
	void Start () {
		book = Book.BookFromResources (Name);
		BookComponent.CurrentBook = book;
		BookComponent.LoadScene ();
		Debug.Log (book.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			foreach (var touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					Debug.Log (touch.position.ToString ());

					if (touch.position.x < Screen.width * 0.5f) {
						GoToPreviousPage ();
					} else {
						GoToNextPage ();
					}
				}
			}
		}
	}

	private void GoToPreviousPage ()
	{
		if (book.CurrentPageIndex > 0) {
			book.CurrentPageIndex--;
			BookComponent.ReloadScene ();
		}
	}

	private void GoToNextPage ()
	{
		if (book.CurrentPageIndex < book.Pages.Count - 2) {
			book.CurrentPageIndex++;
			BookComponent.ReloadScene ();
		}
	}

	
	void OnGUI () {

	}

}
