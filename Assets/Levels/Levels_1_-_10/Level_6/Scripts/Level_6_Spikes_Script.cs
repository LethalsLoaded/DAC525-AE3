using UnityEngine;

public class Level_6_Spikes_Script : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "PLAYER")
        {
            collider.GetComponent<Entity>().Hit(100, null);
        }
    }
}
