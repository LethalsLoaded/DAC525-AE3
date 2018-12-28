using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_11_Platform_Handler : MonoBehaviour
{
    public float stateTime;

    public List<GameObject> vanishingTiles = new List<GameObject>();
    private List<GameObject> _evenTiles = new List<GameObject>();
    private List<GameObject> _oddTiles = new List<GameObject>();

    private bool _EvensAreSpectral;

    void Start()
    {
        bool flipFlop = true;
        foreach(var item in vanishingTiles)
        {
            if(flipFlop)
                _evenTiles.Add(item);
            else
                _oddTiles.Add(item);

            flipFlop = !flipFlop;
        }

        foreach(var item in _evenTiles)
        SwitchObjectState(item);

        StartCoroutine(SwitchState());
    }

    IEnumerator SwitchState()
    {
        yield return new WaitForSeconds(stateTime);
        foreach(var item in _evenTiles)
            SwitchObjectState(item);
        foreach(var item in _oddTiles)
            SwitchObjectState(item);

            
        StartCoroutine(SwitchState());
    }

    void SwitchObjectState(GameObject targetObject)
    {
        var objectCollider = targetObject.GetComponent<Collider2D>();
        var objectSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        var objectColor = objectSpriteRenderer.color;
        if(objectColor.a == 1.0f)
        {
            objectColor.a = 0.15f;
            objectSpriteRenderer.color = objectColor;
            objectCollider.enabled = false;
        }
        else
        {
            objectColor.a = 1.0f;
            objectSpriteRenderer.color = objectColor;
            objectCollider.enabled = true;
        }

    }
}
