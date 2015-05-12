using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BookModel;

public class BookComponent : MonoBehaviour
{

	public static Book CurrentBook = Book.TemplateBook ();

	public static GameObject BookObject {
		get {
			return GameObject.FindGameObjectWithTag ("Book");
		}
	}
	
	public static void ClearScene ()
	{
		var parent = BookObject.transform;
		int childs = parent.childCount;
		for (int i = childs - 1; i >= 0; i--) {
			GameObject.Destroy (parent.GetChild (i).gameObject);
		}
	}

	public static void LoadScene ()
	{
		var page = CurrentBook.CurrentPage;
		if (page == null) {
			Debug.Log("There are no current page");
		}
		
		var pictures = page.Pictures;
		if (pictures == null) {
			Debug.Log("There are no pictures on current page");
			pictures = new List<Picture> ();
		}
		
		foreach (var picture in pictures) {
			picture.AddToTheScene ();
		}
		
		if (page.text == null) {
			Debug.Log("The text field is empty for current page");
			page.text = new Text ("");
		}
		
		page.text.AddToTheScene ();
	}

	public static void ReloadScene ()
	{
		ClearScene ();
		LoadScene ();
	}

	private static void DestroyGameObjectsWithTag (string tag)
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject target in gameObjects) {
			GameObject.Destroy (target);
		}
	}

	// Use this for initialization
	void Start ()
	{
		CurrentBook.CurrentPage.text.AddToTheScene ();
//		var text = new Text ("Hello - hello!");
//		text.Position = new Helpers.Position (0.5f, 0.5f, -9);
//		text.AddToTheScene ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
