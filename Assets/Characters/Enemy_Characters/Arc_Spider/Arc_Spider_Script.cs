using System.Collections;
using UnityEngine;

public class Arc_Spider_Script : Entity
{

    public GameObject platformUnlocked;
    public float minWaitTimeArcPoision, maxWaitTimeArcPoison;

    protected override void OnDeath(Entity entityKiller = null)
    {
        // TODO: Some death animation or effect?
        foreach (BoxCollider2D collider in transform.parent.GetComponents<BoxCollider2D>())
        {
            collider.enabled = true;
        }

        Destroy(gameObject);
    }

    protected override void OnHit(Entity entityDamager)
    {
        StartCoroutine(Blink());
    }

    protected override void OnInteraction(Entity entityInteracter)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start()
    {        foreach (var ability in entityAbilities)
            ability.abilityOwner = gameObject;
        StartCoroutine(ArcPoisionDart());
    }

    IEnumerator ArcPoisionDart()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTimeArcPoision, maxWaitTimeArcPoison));
        if(isDead) StopCoroutine(ArcPoisionDart());
        else
        {
            ((Arc_Poison_Dart_Ability_Script)GetAbility("Arc Poison Dart")).Use(transform);
        }

        if(!isDead) StartCoroutine(ArcPoisionDart());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.isTrigger || collider.tag != "PLAYER") return;
        if(HasAbility("Hostile Touch")) GetAbility("Hostile Touch").Use();
    }
}
