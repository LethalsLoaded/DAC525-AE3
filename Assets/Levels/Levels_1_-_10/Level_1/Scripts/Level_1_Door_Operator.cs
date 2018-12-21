using System.Collections;
using UnityEngine;

public class Level_1_Door_Operator : MonoBehaviour
{
    public bool allTasksComplete = false;

    private void Update()
    {
        if (allTasksComplete) enabled = false;
        foreach (Transform item in GameObject.Find("LEVEL_ENEMIES").transform.GetComponentInChildren<Transform>())
            // if(item.GetComponent<Entity>().isDead) break;
            // allTasksComplete = true;
            // Debug.Log("We done here.");
            // GameObject.FindGameObjectWithTag("DOOR").GetComponent<DoorScript>()._isDoorOpen = true;
            // return;

            if (item.GetComponent<Entity>().isDead)
                return;

        IEnumerable c = GameObject.Find("LEVEL_ENEMIES").transform.GetComponentInChildren<Transform>();
    }
}