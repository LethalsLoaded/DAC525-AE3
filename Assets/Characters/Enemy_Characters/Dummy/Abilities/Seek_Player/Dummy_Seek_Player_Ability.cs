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

[CreateAssetMenu(fileName = "Dummy_Seek_Player_Ability", menuName = "Character_Abilities/Dummy/Seek_Player", order = 2)]
public class Dummy_Seek_Player_Ability : Ability
{

    public override void Start()
    {
        // Do stuff here when an ability is SPAWNED into the world
		_timeLerpStarted = 0.0f;
		FlipLerp();
    }

	public Vector3 vectorPoint1;
	public Vector3 vectorPoint2;
	private Vector3 _vectorDirection;
	public float length;

	private LineRenderer _laserLineRenderer;
	private Transform _laserHitPosition;
	private float _timeLerpStarted;

	private bool _lerpOneToTwo;

	public float speed = 2.0f;

    public override void Update()
    {
		float timeSinceStarted = Time.time - _timeLerpStarted;
		float percentComplete = timeSinceStarted / speed;

		if(percentComplete < 0) 
		{
			FlipLerp();
			return;
		}

		if(_lerpOneToTwo)
			_vectorDirection = Vector3.Lerp(vectorPoint2, vectorPoint1, percentComplete);
		else
			_vectorDirection = Vector3.Lerp(vectorPoint1, vectorPoint2, percentComplete);

		//Debug.Log(percentComplete);

		if(percentComplete >= 1.0f) FlipLerp();

		if(_laserLineRenderer == null) _laserLineRenderer = abilityOwner.GetComponent<LineRenderer>();
		if(_laserHitPosition == null) _laserHitPosition = GameObject.Find("hit").transform;

		var hit = Physics2D.Raycast(abilityOwner.transform.position, _vectorDirection, length);
		_laserHitPosition.transform.position = hit.point;
		_laserLineRenderer.SetPosition(0, abilityOwner.transform.position);
		_laserLineRenderer.SetPosition(1, _laserHitPosition.transform.position);

		if(hit && hit.collider.tag == "PLAYER")
		{
			var player = hit.collider.GetComponent<Entity>();
			player.Hit(player.entityHitPoints, abilityOwner.GetComponent<Entity>());
		}
    }

	public void FixedUpdate()
	{
		
	}

	public void FlipLerp()
	{
		_timeLerpStarted = Time.time;
		_lerpOneToTwo = !_lerpOneToTwo;
	}

    public override void Use()
    {
        // Do stuff when an ability is used (set trajectory etc)
		isActive = true;
    }
	
}
