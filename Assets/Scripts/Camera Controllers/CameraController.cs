using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float FreedomX = 10;
	public float FreedomY = 10;

	/// <summary>
	/// Value in [-1 ... 1]
	/// </summary>
	[HideInInspector]
	public float ShiftX = 0;

	/// <summary>
	/// Value in [-1 ... 1]
	/// </summary>
	[HideInInspector]
	public float ShiftY = 0;

	[HideInInspector]
	public Vector3 InitialPosition;
	private Vector3 position {
		get { return transform.position; }
		set { transform.position = value; }
	}

	// Use this for initialization
	protected void Start () {
		InitialPosition = position;
	}
	
	// Update is called once per frame
	protected void Update ()
	{
		position = InitialPosition + new Vector3 (FreedomX * ShiftX, FreedomY * ShiftY, 0);
		transform.LookAt (lookPoint ());
	}

	private Vector3 lookPoint ()
	{
//		return new Vector3 (0, 0, 100);
		return new Vector3 (-position.x * 0.5f, -position.y * 0.5f, -position.z);
	}
}
