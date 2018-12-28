using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level_12_Dispenser_Handler : MonoBehaviour
{

    public Direction facingDirection;
    public List<Level_12_Pressure_Plate_Handler> pressurePlatesConnected = new List<Level_12_Pressure_Plate_Handler>();
    public float fireDelay = 0.5f;
    public float fireStrength = 2.0f;
    public GameObject arrowObject;

    void Start()
    {
        foreach(var item in pressurePlatesConnected)
        {
            item.onActivate.AddListener(Fire);
        }
    }

	void Fire()
    {
        StartCoroutine(ActuallyFire());
    } //5 10 13

    IEnumerator ActuallyFire()
    {
        yield return new WaitForSeconds(fireDelay);
        float offset = transform.position.x - (transform.localScale.x / 2);
        var offsetVector = new Vector3(facingDirection == Direction.WEST ? offset : -offset, transform.position.y, 0);
        var arrow = Instantiate(arrowObject, transform.position, Quaternion.identity);
        float strength = facingDirection == Direction.WEST ? -fireStrength : fireStrength;
        var strengthVector = new Vector2(strength * 50, 0);
        if(facingDirection == Direction.WEST) arrow.GetComponent<SpriteRenderer>().flipX = true;
        arrow.GetComponent<Rigidbody2D>().AddForce(strengthVector);
    }
}
