using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BookModel;

using Helpers;

public class DeployerScript : MonoBehaviour {

	public string Name = "book.xml";
	static public Book book;

	private bool userInteractionEnabled = true;

	// Use this for initialization
	void Start () {
		book = Book.BookFromResources (Name);
		BookComponent.CurrentBook = book;
		loadCurrentPage ();
		BookComponent.LoadScene ();
		StartPageAnimation ();
	}

	private const int threshold = 1500;

	// Update is called once per frame
	void Update () {
#if UNITY_IOS

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			var primaryTouch = Input.GetTouch(0);
			var Velocity_X = primaryTouch.deltaPosition.x / primaryTouch.deltaTime;
			
//			Debug.Log ("Touch was moved with velocity " + Velocity_X);
			
			if (userInteractionEnabled) {
				if (Velocity_X < -threshold) {
					GoToNextPage ();
				} else if (Velocity_X > threshold) {
					GoToPreviousPage ();
				}
			}
		}

#else
		if (Input.GetMouseButtonDown (0)) {
			if (userInteractionEnabled) {
				var third = Screen.width / 3;
				if (Input.mousePosition.x < third) {
					GoToPreviousPage ();
				} else if (Input.mousePosition.x > 2 * third) {
					GoToNextPage ();
				}
			}
		}


//		if (Input.touchCount > 0) {
//
//			foreach (var touch in Input.touches) {
//				if (touch.phase == TouchPhase.Began) {
//					if (userInteractionEnabled) {
//						if (touch.position.x < Screen.width * 0.5f) {
//							GoToPreviousPage ();
//						} else {
//							GoToNextPage ();
//						}
//					}
//				}
//			}
//		}
#endif
	}

	private void GoToPreviousPage ()
	{
		bool success = BookComponent.GoToPreviousPage (true, () => {
			userInteractionEnabled = true;
			StartPageAnimation ();
		});

		userInteractionEnabled = !success;
	}
	
	private void GoToNextPage ()
	{
		bool success = BookComponent.GoToNextPage (true, () => {
			userInteractionEnabled = true;
			StartPageAnimation ();
		});

		userInteractionEnabled = !success;
	}

	private void StartPageAnimation ()
	{
		foreach (var picture in book.CurrentPage.Pictures) {
			if (picture.Animation != null) {
				ABAnimationSystem.RunAnimation (picture.Animation, picture.GameObject);
			}
		}
	}

	
	void OnGUI () {

	}

	private bool isPaused;
	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus != isPaused) {
			isPaused = pauseStatus;
			if (isPaused == true) {
				saveCurrentPage ();
			} else {
				loadCurrentPage ();
			}
		}
	}

	private void saveCurrentPage ()
	{
		PlayerPrefs.SetInt ("current page", BookComponent.CurrentBook.CurrentPageIndex);
	}

	private void loadCurrentPage ()
	{
		BookComponent.CurrentBook.CurrentPageIndex = PlayerPrefs.GetInt ("current page");
	}

}
