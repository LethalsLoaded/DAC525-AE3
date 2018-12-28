using UnityEngine;

public class Teleporter_Door_Handler : MonoBehaviour {

    public Transform teleportTargetLocation;
    public bool isActive = true;
    bool _canTeleport = false;

	void Start ()
	{
		GameManager.GetInstance().playerCharacter.GetComponent<Rogue_Script>().onInteract.AddListener(OnInteract);
	}
	
	void OnInteract()
    {
        if(!_canTeleport) return;
        GameManager.GetInstance().playerCharacter.transform.position = teleportTargetLocation.position;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(isActive)
            _canTeleport = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        _canTeleport = false;
    }
}
