using UnityEngine;
using System.Collections;

public class ParallaxEnabler : MonoBehaviour {

	private CameraController cameraContorller;

	// Use this for initialization
	void Start () {
		cameraContorller = GetComponent<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if (UI.TopPanelDelegate.ParallaxEnabled) {
			cameraContorller.enabled = true;
//			Debug.Log("Parallax enabled");

		} else {
			cameraContorller.enabled = false;
			transform.position = cameraContorller.InitialPosition;
//			Debug.Log("Parallax disabled");
		}
#endif
	}
}
