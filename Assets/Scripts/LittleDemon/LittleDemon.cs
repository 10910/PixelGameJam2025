using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class LittleDemon : MonoBehaviour
{
    [SerializeField]
    public Transform startPosition;
    [SerializeField]
    public Transform targetPosition;
    [SerializeField]
    public float speed;
    private Animator animator;
    // private RectTransform rectTransform;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        //订阅
        GhostDialogue.onDialogueEnd += OnDialogueEndCallback;
        // JudgeManager.onJudgeEnd += WalkBackToGetNewFile;
    }

    private void OnDestroy()
    {
        //取消订阅
        GhostDialogue.onDialogueEnd -= OnDialogueEndCallback;
        // JudgeManager.onJudgeEnd -= WalkBackToGetNewFile;
    }



    void OnEnable()
    {
        transform.position = startPosition.position;
        // DeliverFile();
    }

    [Button("DeliverFile")]
    public void DeliverFile()
    {
        Debug.Log("DeliverFile");
        transform.position = startPosition.position;
        // rectTransform.anchoredPosition = startPosition;
        animator.SetBool("isWalk", true);
        float duration = Vector3.Distance(startPosition.position, targetPosition.position) / speed;
        transform.DOMove(targetPosition.position, duration)
                 .SetEase(Ease.Linear)
                 //   .OnComplete(() => animator.SetBool("isWalk", false)); // 在移动结束时设置 isWalk 为 false回到idle状态
                 .OnComplete(() =>
                 {
                     SwitchToAnimation("LittleDemon_idle");
                     animator.SetBool("isWalk", false);
                 }); // 在移动结束时设置 isWalk 为 false回到idle状态

    }

    [Button("WalkBackToGetNewFile")]
    //可能需要判断一下现在的位置是否在targetposition 如果不是则不需要往回走
    public void WalkBackToGetNewFile()
    {
        Debug.Log("WalkBackToGetNewFile");
        if (transform.position == targetPosition.position)
        {
            transform.position = targetPosition.position;
            // rectTransform.anchoredPosition = startPosition;
            animator.SetBool("isWalkback", true);
            float duration = Vector3.Distance(targetPosition.position, startPosition.position) / speed;
            transform.DOMove(startPosition.position, duration)
                     .SetEase(Ease.Linear)
                      .OnComplete(() => animator.SetBool("isWalkback", false)); // 在移动结束时设置 isWalkback 为 false回到idle状态
                                                                                //  .OnComplete(() => SwitchToAnimation("LittleDemon_idle")); // 在移动结束时设置 isWalk 为 false回到idle状态

        }
    }
    //LittleDemon_idle

    // 立即切换到新动画（不等待当前动画结束）
    public void SwitchToAnimation(string newAnimationName)
    {
        // 直接播放新动画，并设置 normalizedTime=0（从头开始）
        animator.Play(newAnimationName, 0, 0f);
    }

    public void OnDialogueEndCallback()
    {
        Debug.Log("LittleDemon OnDialogueEndCallback");
        DeliverFile();
    }

    public void LittleDemonFileButtonCallback()
    {
        UIManager.Instance.OpenFilesPanel();
        //AudioManager.Instance.PlaySFX("documentClick");
    }

    public void LittleDemonIDButtonCallback()
    {
        UIManager.Instance.OpenIDPanel();
        //AudioManager.Instance.PlaySFX("documentClick");
    }
}

