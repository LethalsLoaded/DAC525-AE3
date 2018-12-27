using UnityEngine;

public class Level_9_Spring_Script : MonoBehaviour
{

    private bool _inTrigger;
    public float strength = 5.0f;

    void Start()
    {
        GameManager.GetInstance().playerCharacter.GetComponent<Rogue_Script>().onJump.AddListener(Bounce);
    }

    public void Bounce()
    {
        if(!_inTrigger) return;
        GameManager.GetInstance().playerCharacter.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, strength));
    }

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag != "PLAYER") return;
        _inTrigger = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag != "PLAYER") return;
        _inTrigger = false;
    }
}
