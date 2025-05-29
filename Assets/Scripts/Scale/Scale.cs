using UnityEngine;
using UnityEngine.UI;
public class scale : MonoBehaviour
{
    private Animator scaleAnimator;
    public Button featherBtn;
    public Button heartBtn;
    void Awake()
    {
        scaleAnimator = GetComponent<Animator>();
    }

    public void OnScaleClicked(bool shouldReborn)
    {

        SetBtnsInteractable(false);
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
        SetBtnsInteractable(true);
        CloseJudgePanel();
        Debug.Log("Scale PullToHell");
        AnimationManager.Instance.PlayPullToHell();
        AudioManager.Instance.PlaySFX("hell");
    }

    // 由Animation Event调用
    public void Rebirth()
    {
        SetBtnsInteractable(true);
        CloseJudgePanel();
        //JudgeManager.Instance.Rebirth();
        AnimationManager.Instance.PlayRebirth();
        AudioManager.Instance.PlaySFX("rebirth");
    }

    private void CloseJudgePanel()
    {
        UIManager.Instance.CloseJudgePanel();
    }

    void SetBtnsInteractable(bool isInteractable){
        heartBtn.interactable = isInteractable;
        featherBtn.interactable = isInteractable;
    }

}
