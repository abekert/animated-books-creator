using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public static GameObject DragObject;
	public Picture Picture;

	void OnMouseDown()
	{
		Debug.Log("On Mouse Down Was Called");
	}
	void OnMouseDrag()
	{
		Debug.Log("On Mouse Drag Was Called");

		float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
		transform.position = new Vector3( pos_move.x, pos_move.y, transform.position.z );
		Picture.UpdatePositionByPictureObject ();
	}

}
