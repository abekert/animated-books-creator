using UnityEngine;
using System.Collections;

using BookModel;

public class MouseDrag : MonoBehaviour
{	
	internal IPositionable PositionableObject;

	private Vector3 mouseDownPosition;
	private Vector3 originalPosition;
	private float distanceToScreen;

	void OnMouseDown()
	{
		distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		mouseDownPosition = worldMousePosition ();
		originalPosition = transform.position;

		PositionableObject.RepositionBegan ();
	}

	void OnMouseDrag()
	{
		Vector3 mousePosition = worldMousePosition ();
		var mouseDrag = mousePosition - mouseDownPosition;

		transform.position = new Vector3(mouseDrag.x + originalPosition.x, mouseDrag.y + originalPosition.y, transform.position.z);
		PositionableObject.Position.convertFromOnScreenPosition (transform.position);
	}

	private Vector3 worldMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
	}

}
