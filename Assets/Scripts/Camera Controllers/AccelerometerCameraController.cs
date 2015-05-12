using UnityEngine;
using System.Collections;

public class AccelerometerCameraController : CameraController {

	// Update is called once per frame
	protected void Update () {
		Vector2 vector = Input.acceleration;
		ShiftX = -vector.x;
		ShiftY = -vector.y;
		base.Update ();
	}
}
