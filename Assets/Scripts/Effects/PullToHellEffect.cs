using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks.Triggers;
public class PullToHellEffect : MonoBehaviour
{
    [SerializeField] private float pullToHellDistance = 5.0f;
    [SerializeField] private float pullToHellDuration = 2.0f;
    [SerializeField] private float pullToHellSpeed = 1.66f;

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
    public void PullGhostDown() {
        //float distance = AnimationManager.Instance.ghostSprite.GetComponent<SpriteRenderer>().size.y;
        //float duration = distance / pullToHellSpeed;

        DOTween.Sequence().Append(AnimationManager.Instance.ghostSprite.transform.DOMoveY(transform.position.y - pullToHellDistance, pullToHellDuration).SetEase(Ease.OutSine))
            //.AppendInterval(pullToHellDuration - duration)
            .AppendCallback(() => {
                AnimationManager.Instance.ResetGhostSprite();
                AnimationManager.Instance.ghostSprite.SetActive(false);
                JudgeManager.Instance.JudgeEnd();
                gameObject.SetActive(false);
            });
            
        //});
        // GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.DOMoveY(transform.position.y - pullToHellDistance, pullToHellDistance * pullToHellSpeed);
    }

    //// 停止动画
    //void StopAnimation()
    //{
    //    if (ghostMoveTween != null && ghostMoveTween.IsActive())
    //    {
    //        ghostMoveTween.Kill();
    //        // 如果需要重置位置
    //        // ghostMoveTween.Kill(true); // true表示完全重置
    //    }
    //}

}
