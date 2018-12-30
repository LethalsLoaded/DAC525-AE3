using System.Collections;
using UnityEngine;

public class Mentally_Insane_Cloud_Elemental_Script : Entity
{

    public GameObject originPoint;
    public float orbitDistance = 5.0f;
    public Vector2 pointOne, pointTwo;
    
    private float _abilityUse = 2.0f;

    protected override void OnDeath(Entity entityKiller = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHit(Entity entityDamager)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start ()
	{
        foreach (var ability in entityAbilities)
            ability.abilityOwner = gameObject;

		orbitDistance = Vector2.Distance(transform.position, originPoint.transform.position);
        _abilityUse = GetAbility("Drop Aero Bomb").abilityValue;
        StartCoroutine(ReuseAirAbility());
	}

    IEnumerator ReuseAirAbility()
    {
        if(isDead) StopCoroutine(ReuseAirAbility());
        yield return new WaitForSeconds(_abilityUse);
        GetAbility("Drop Aero Bomb").Use();
        if(isDead) StopCoroutine(ReuseAirAbility());
        else StartCoroutine(ReuseAirAbility());
    }

	// -1.5 7
    // -14.3
    // 14.3
    bool _movingToPointTwo;
	protected override void Update ()
	{
        if(isDead) return;
        // *** MOVE THE ORIGIN *** //
        var speed = (entitySpeed / 50) * Time.deltaTime;
        if(_movingToPointTwo)
            originPoint.transform.position = Vector2.MoveTowards(originPoint.transform.position, pointTwo, speed);
        else
            originPoint.transform.position = Vector2.MoveTowards(originPoint.transform.position, pointOne, speed);

        if(((Vector2)originPoint.transform.position == pointTwo))
            _movingToPointTwo = false;
        else if (((Vector2)originPoint.transform.position == pointOne))
            _movingToPointTwo = true;

        // *** MOVE THE CLOUD ***
        // transform.RotateAround(originPoint.transform.position, new Vector3(0,0,1), entitySpeed * Time.deltaTime);
        // transform.rotation = Quaternion.identity;

        transform.position = originPoint.transform.position + (transform.position - originPoint.transform.position).normalized * orbitDistance;
        transform.RotateAround(originPoint.transform.position, Vector3.forward, entitySpeed * Time.deltaTime);
        transform.rotation = Quaternion.identity;

        foreach(var item in entityAbilities) item.Update();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PLAYER" && HasAbility("Hostile Touch"))
            UseAbility(GetAbility("Hostile Touch"));
    }
}
