using UnityEngine;

public class Level_12_Arrow_Handler : MonoBehaviour
{

    public int damage = 3;

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "TILE_OUTER" || collider.tag == "TILE_SOLID")
            Destroy(this.gameObject);
        
        if(collider.tag == "PLAYER" || collider.tag == "ENTITY")
        {
            if(collider.isTrigger) return;
            collider.GetComponent<Entity>().Hit(3, null);
            Destroy(this.gameObject);
        }
    }
}
