using UnityEngine;

public class PullToHellRenderer : MonoBehaviour
{
    // PullToHell动画期间由animation event调用
    public void PullGhostDown()
    {
        AnimationManager.Instance.PullGhostDown();
    }

    // PullToHell动画结束后由animation event调用
    public void PullAnimEnd()
    {
        //AnimationManager.Instance.ResetGhostSprite();
        //JudgeManager.Instance.JudgeEnd();
    }

}
