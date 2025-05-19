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
        AudioManager.Instance.PlaySFX("scaleJudge");

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
        AudioManager.Instance.PlaySFX("hell");
    }

    // 由Animation Event调用
    public void Rebirth()
    {
        CloseJudgePanel();
        //JudgeManager.Instance.Rebirth();
        AnimationManager.Instance.PlayRebirth();
        AudioManager.Instance.PlaySFX("rebirth");
    }

    private void CloseJudgePanel()
    {
        UIManager.Instance.CloseJudgePanel();
    }

}
