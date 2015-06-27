using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BookModel;

public class BookComponent : MonoBehaviour
{
	public static Book CurrentBook = Book.TemplateBook ();
	public static bool IsPlayerMode {
		get {
			return CurrentBook.IsFromResourcesFolder;
		}
	}

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
			picture.AddToScene (!IsPlayerMode);
		}
		
		if (page.Text == null) {
			Debug.Log("The text field is empty for current page");
			page.Text = new Text ("");
		}
		
		page.Text.AddToTheScene (!IsPlayerMode, CurrentBook.FontName);
		UpdatePageColor ();
	}

	public static void UpdatePageColor ()
	{
		var page = CurrentBook.CurrentPage;
		if (page == null) {
			Debug.Log("There are no current page");
		}

		Camera.main.backgroundColor = page.Color.ToColor ();
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
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static bool GoToNextPage (bool animated, Action completion = null)
	{
		if (CurrentBook.CurrentPageIndex == CurrentBook.Pages.Count - 1) {
			return false;
		}

		if (animated) {
			var nextPage = CurrentBook.Pages [CurrentBook.CurrentPageIndex + 1];
			Helpers.PageTransitions.FontName = CurrentBook.FontName;
			Helpers.PageTransitions.ShowNextPage (CurrentBook.CurrentPage, nextPage, IsPlayerMode, completion);
			CurrentBook.CurrentPageIndex++;
		} else {
			CurrentBook.CurrentPageIndex++;
			ReloadScene ();
			completion ();
		}

		return true;
	}
	
	public static bool GoToPreviousPage (bool animated, Action completion = null)
	{
		if (CurrentBook.CurrentPageIndex == 0) {
			return false;
		}
		
		if (animated) {
			var previousPage = CurrentBook.Pages [CurrentBook.CurrentPageIndex - 1];
			Helpers.PageTransitions.FontName = CurrentBook.FontName;
			Helpers.PageTransitions.ShowPreviousPage (CurrentBook.CurrentPage, previousPage, IsPlayerMode, completion);
			CurrentBook.CurrentPageIndex--;
		} else {
			CurrentBook.CurrentPageIndex--;
			ReloadScene ();
			completion ();
		}

		return true;
	}

}
