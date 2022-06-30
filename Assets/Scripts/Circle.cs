using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour
{
    SpriteRenderer mSpriteRenderer;

    void Awake()
    {
        mSpriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        mSpriteRenderer.material.SetFloat("_StartTime", Time.time);

        float animationTime = mSpriteRenderer.material.GetFloat("_AnimationTime");
        float destroyTime = animationTime;
        destroyTime -= mSpriteRenderer.material.GetFloat("_StartWidth") * animationTime;
        destroyTime += mSpriteRenderer.material.GetFloat("_Width") * animationTime;
        Destroy(transform.gameObject, destroyTime);
    }
}