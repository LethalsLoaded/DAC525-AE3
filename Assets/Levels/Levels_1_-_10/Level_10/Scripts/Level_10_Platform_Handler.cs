using UnityEngine;

public class Level_10_Platform_Handler : MonoBehaviour
{

    public AnimationClip animClip;

    void Start()
    {
        ;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var _anim = gameObject.AddComponent<Animation>();
        _anim.clip = animClip;
        _anim.Play();
        Invoke("AnimationEnded", animClip.length);
    }

    void AnimationEnded()
    {
        gameObject.SetActive(false);
    }
}
