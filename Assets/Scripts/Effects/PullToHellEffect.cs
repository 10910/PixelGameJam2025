using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class PullToHellEffect : MonoBehaviour
{
    [SerializeField] private float pullToHellDistance = 5.0f;
    [SerializeField] private float pullToHellDuration = 6.0f;
    // [SerializeField] private float pullToHellSpeed = 1.66f;

    // [SerializeField]
    // public Transform targetPosition;
    private Animator pullToHellAnimator;
    // 存储动画引用
    private Tween ghostMoveTween;
    private void Awake()
    {
        pullToHellAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        pullToHellAnimator.Play("PullToHell");
    }

    [Button]
    public void PullGhostDown()
    {
        //让ghost跟着往下移动
        ghostMoveTween = GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.DOMoveY(transform.position.y - pullToHellDistance, pullToHellDuration);

        // GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.DOMoveY(transform.position.y - pullToHellDistance, pullToHellDistance * pullToHellSpeed);
    }

    [Button]
    public void ResetGhost()
    {
        StopAnimation();
        GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);
    }

    // 停止动画
    void StopAnimation()
    {
        if (ghostMoveTween != null && ghostMoveTween.IsActive())
        {
            ghostMoveTween.Kill();
            // 如果需要重置位置
            // ghostMoveTween.Kill(true); // true表示完全重置
        }
    }

}
