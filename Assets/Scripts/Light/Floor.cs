using UnityEngine;

public class Floor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Animator floorAnimator;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        floorAnimator = GetComponentInChildren<Animator>();
    }



}
