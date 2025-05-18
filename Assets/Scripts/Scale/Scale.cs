using UnityEngine;
using UnityEngine.UI;
public class scale : MonoBehaviour
{
    private Animator scaleAnimator;

    void Awake()
    {
        scaleAnimator = GetComponent<Animator>();
    }

    public void OnScaleClicked(bool shouldReborn)
    {
        // 记录审判
        JudgeManager.Instance.SetJudgement(shouldReborn);

        if (shouldReborn)
        {
            //播放羽毛动画
            scaleAnimator.Play("balance_feather");
        }
        else
        {
            //播放心脏动画
            scaleAnimator.Play("balance_heart");
        }
    }

    // 由Animation Event调用
    public void PullToHell()
    {
        CloseJudgePanel();
        Debug.Log("Scale PullToHell");
        AnimationManager.Instance.PlayPullToHell();
    }

    // 由Animation Event调用
    public void Rebirth()
    {
        CloseJudgePanel();
        //JudgeManager.Instance.Rebirth();
        AnimationManager.Instance.PlayRebirth();
    }

    private void CloseJudgePanel()
    {
        UIManager.Instance.CloseJudgePanel();
    }

}
