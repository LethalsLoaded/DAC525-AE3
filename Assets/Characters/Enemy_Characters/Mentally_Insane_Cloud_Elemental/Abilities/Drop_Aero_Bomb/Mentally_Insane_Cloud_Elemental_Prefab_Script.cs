using UnityEngine;


public class Mentally_Insane_Cloud_Elemental_Prefab_Script : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.isTrigger && collider.tag == "PLAYER")
            GameManager.GetInstance().playerEntity.Hit(2, GameObject.FindGameObjectWithTag("ENTITY").GetComponent<Entity>());
        else if (collider.tag == "TILE_OUTER")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
