using UnityEngine;
using DG.Tweening;
public class LittleDemon_origin : MonoBehaviour
{
    [SerializeField]
    public Vector3 startPosition;
    [SerializeField]
    public Vector3 targetPosition;
    [SerializeField]
    public float speed;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    void OnEnable()
    {
        transform.position = startPosition;
    }

    public void DeliverFile()
    {
        animator.SetBool("isWalk", true);
        float duration = Vector3.Distance(startPosition, targetPosition) / speed;
        transform.DOMove(targetPosition, duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => animator.SetBool("isWalk", false)); // 在移动结束时设置 isWalk 为 false
    }
}
