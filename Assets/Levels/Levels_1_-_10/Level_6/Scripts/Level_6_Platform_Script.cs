
using System.Collections;
using UnityEngine;

public class Level_6_Platform_Script : MonoBehaviour {

    [Tooltip("Time it takes for the block to change state")]
    public float stateTime;

	public bool changesState;
    public bool isOnFire;
    public bool isSpecialTile;

    public Color defaultColor = Color.blue;
    public Color onFireColor = Color.red;

    public BoxCollider2D platformPlayerTrigger;
    public Level_6_Platform_Script specialTileTarget;

    private bool _coroutineStarted;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = isOnFire ? onFireColor : defaultColor;
        if(isSpecialTile)
            platformPlayerTrigger.enabled = true;
    }

    public void Update()
    {
        if(changesState && !_coroutineStarted)
            StartCoroutine(ChangeState());
    }

    private IEnumerator ChangeState()
    {
        _coroutineStarted = true;
        yield return new WaitForSeconds(stateTime);
        if(isOnFire)
        {
            // * TURN FIRE OFF *
            GetComponent<SpriteRenderer>().color = defaultColor;
            isOnFire = false;
            platformPlayerTrigger.enabled = false;
        }
        else
        {
            // * TURN FIRE ON *
            GetComponent<SpriteRenderer>().color = onFireColor;
            isOnFire = true;
            platformPlayerTrigger.enabled = true;

        }
        _coroutineStarted = false;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(isOnFire && collider.tag == "PLAYER")
        {
            collider.GetComponent<Entity>().Hit(100, null);
        }

        if(isSpecialTile)
        {
            specialTileTarget.stateTime /= 2;
        }
    }
}
