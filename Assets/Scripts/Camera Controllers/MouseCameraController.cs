using UnityEngine;
using System.Collections;

public class MouseCameraController : CameraController {
	
	// Update is called once per frame
	protected void Update () {
		Vector2 mouse = Input.mousePosition;
		ShiftX = (mouse.x / (Screen.width / 2)) - 1;
		ShiftY = (mouse.y / (Screen.height / 2)) - 1;
		base.Update ();
	}
}
