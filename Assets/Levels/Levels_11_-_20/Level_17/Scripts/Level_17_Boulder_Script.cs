using UnityEngine;

public class Level_17_Boulder_Script : MonoBehaviour
{

    bool _isActive = false;

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "TILE_SOLID" && _isActive)
        {
            Camera.main.GetComponent<Level_17_Level_Manager>().Shake();
            Destroy(gameObject);
            return;
        }

        if(collider.isTrigger || collider.tag != "PLAYER") return;
        GetComponent<Rigidbody2D>().gravityScale = 5;
        GetComponent<BoxCollider2D>().offset = Vector2.zero;
        _isActive = true;
    }
}
