using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	bool lookingAtMenu = false;
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown (0)) {
			var third = Screen.width / 3;
			if (Input.mousePosition.x >= third &&
			    Input.mousePosition.x <= 2 * third) {
				switchMenu ();
			}
		}

		#else
		
//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
//			var primaryTouch = Input.GetTouch(0);
//			var Velocity_X = primaryTouch.deltaPosition.x / primaryTouch.deltaTime;
//			
////			Debug.Log ("Touch was moved with velocity " + Velocity_X);
//			
//			if (userInteractionEnabled) {
//				if (Velocity_X < -threshold) {
//					GoToNextPage ();
//				} else if (Velocity_X > threshold) {
//					GoToPreviousPage ();
//				}
//			}
//		}
//		
////		if (Input.touchCount > 0) {
////
////			foreach (var touch in Input.touches) {
////				if (touch.phase == TouchPhase.Began) {
////					if (userInteractionEnabled) {
////						if (touch.position.x < Screen.width * 0.5f) {
////							GoToPreviousPage ();
////						} else {
////							GoToNextPage ();
////						}
////					}
////				}
////			}
////		}
#endif

	}

	private void switchMenu ()
	{
		lookingAtMenu = !lookingAtMenu;

		if (lookingAtMenu) {
			Camera.main.transform.LookAt (new Vector3 (0, 100, 0));
		} else {
			Camera.main.transform.LookAt (new Vector3 (0, 0, 10));
		}
	}
}
