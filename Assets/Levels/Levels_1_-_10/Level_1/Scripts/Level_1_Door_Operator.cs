using System.Collections;
using System.Linq;
using UnityEngine;

public class Level_1_Door_Operator : MonoBehaviour
{
    public bool allTasksComplete = false;

    private void Update()
    {
        if (allTasksComplete) enabled = false;
        var aliveEnemies = GameObject.FindGameObjectsWithTag("ENTITY").Where(x=> !x.GetComponent<Entity>().isDead).Count();
        if(aliveEnemies != 0) return;
        allTasksComplete = true;
        GameObject.FindGameObjectWithTag("DOOR").GetComponent<DoorScript>()._isDoorOpen = true;
        
    }
}