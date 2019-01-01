using UnityEngine;

public class Arc_Poison_Dart_Prefab_Script : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "TILE_OUTER" || collider.tag == "TILE_SOLID")
        {
            Destroy(gameObject);
            return;
        }

        if(collider.isTrigger || collider.tag != "PLAYER") return;
        GameManager.GetInstance().playerEntity.Hit(Random.Range(1, 3), null);
    }
}
