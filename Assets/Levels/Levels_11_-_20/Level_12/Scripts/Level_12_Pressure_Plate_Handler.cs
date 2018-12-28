using UnityEngine;
using UnityEngine.Events;

public class Level_12_Pressure_Plate_Handler : MonoBehaviour
{

    public UnityEvent onActivate;

	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.isTrigger || (collider.tag != "PLAYER")) return;
        onActivate.Invoke();
    }
}
