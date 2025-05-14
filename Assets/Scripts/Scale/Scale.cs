using UnityEngine;
using UnityEngine.UI;
public class scale : MonoBehaviour
{
    // private Button button;
    private Animator scaleAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // button = GetComponent<Button>();
        scaleAnimator = GetComponent<Animator>();
    }



    public void OnHeartClick()
    {
        //播放心脏动画
        scaleAnimator.Play("balance_heart");
    }

    public void OnFeatherClick()
    {
        //播放羽毛动画
        scaleAnimator.Play("balance_feather");
    }

    public void PullToHell()
    {
        CloseJudgePanel();
        JudgeManager.Instance.PullToHell();
    }

    public void Rebirth()
    {
        CloseJudgePanel();
        JudgeManager.Instance.Rebirth();
    }

    private void CloseJudgePanel()
    {
        UIManager.Instance.CloseJudgePanel();
    }

}
