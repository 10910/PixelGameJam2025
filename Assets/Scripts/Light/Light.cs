using UnityEngine;
using Sirenix.OdinInspector;
public class Light : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Floor floor;

    private Animator floorAnimator;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        floor = FindAnyObjectByType<Floor>(FindObjectsInactive.Include);
        floorAnimator = floor.GetComponentInChildren<Animator>();
    }

    public void OnEnable()
    {
        // animator.enabled = true;
        // OpenLight();
    }

    [Button("Open Light")]
    public void OpenLight()
    {
        animator.SetTrigger("Open");
        floorAnimator.SetTrigger("Open");
    }

    [Button("Close Light")]
    public void CloseLight()
    {
        Debug.Log("Close Light");
        animator.SetTrigger("Close");
        floorAnimator.SetTrigger("Close");
    }
    [Button("Close Light Immediately")]
    public void CloseLightImmediately()
    {
        animator.SetTrigger("CloseImmediately");
        floorAnimator.SetTrigger("CloseImmediately");
    }



}
