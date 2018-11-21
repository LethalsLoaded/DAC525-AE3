/*
 * This file has been created by Andrzej Odwald for DAC525-AE3
 * on Tue Nov 20 2018. Everything in this file is written
 * by Andrzej Odwald unless specified otherwise in a comment
 * or if the file has been edited by another developer which
 * is to be named below with the content added or modified mentioned
 *
 * NAME, DATE OF EDIT, CONTENT EDITED:
 */

using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
	#region PUBLIC_VARIABLES
	public float xMost, yMost;
	public GameObject objectToFollow;
	#endregion
	
	#region PRIVATE_VARIABLES
	private Vector2 startPosition;
	private Vector2 maxPosition;
	private float moveSpeed = 3.0f;
	#endregion

	#region PUBLIC_METHODS
	#endregion

	#region PRIVATE_METHODS
	private void Start()
	{
		startPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
		maxPosition = new Vector2(xMost, yMost);
	}

		// Update is called once per frame
	private void Update () 
	{
		float step = moveSpeed * Time.deltaTime;
		var objectToFollowPos= new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, transform.position.y);
		float speed = step * Vector2.Distance(transform.position, objectToFollowPos);
		transform.position = Vector3.MoveTowards(transform.position, objectToFollowPos, speed);
		
	}

	private void LateUpdate()
	{
		transform.position = new Vector3
		(
			Mathf.Clamp(transform.position.x, startPosition.x, maxPosition.x),
			Mathf.Clamp(transform.position.y, startPosition.y, maxPosition.y),
			-10
		);
	}
	#endregion
}
