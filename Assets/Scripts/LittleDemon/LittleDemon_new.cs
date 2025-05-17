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
        GhostDialogue.onDialogueEnd += DeliverFile;
        JudgeManager.onJudgeEnd += WalkBackToGetNewFile;
    }

    private void OnDestroy()
    {
        //取消订阅
        GhostDialogue.onDialogueEnd -= DeliverFile;
        JudgeManager.onJudgeEnd -= WalkBackToGetNewFile;
    }



    void OnEnable()
    {
        transform.position = startPosition.position;
        // DeliverFile();
    }

    [Button("DeliverFile")]
    public void DeliverFile()
    {
        transform.position = startPosition.position;
        // rectTransform.anchoredPosition = startPosition;
        animator.SetBool("isWalk", true);
        float duration = Vector3.Distance(startPosition.position, targetPosition.position) / speed;
        transform.DOMove(targetPosition.position, duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => animator.SetBool("isWalk", false)); // 在移动结束时设置 isWalk 为 false
    }

    [Button("WalkBackToGetNewFile")]
    public void WalkBackToGetNewFile()
    {
        transform.position = targetPosition.position;
        // rectTransform.anchoredPosition = startPosition;
        animator.SetBool("isWalkback", true);
        float duration = Vector3.Distance(targetPosition.position, startPosition.position) / speed;
        transform.DOMove(startPosition.position, duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => animator.SetBool("isWalkback", false)); // 在移动结束时设置 isWalk 为 false
    }

    public void LittleDemonFileButtonCallback()
    {
        UIManager.Instance.OpenFilesPanel();
    }

    public void LittleDemonIDButtonCallback()
    {
        UIManager.Instance.OpenIDPanel();
    }
}
