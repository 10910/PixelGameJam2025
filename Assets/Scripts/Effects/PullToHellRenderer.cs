using UnityEngine;

public class PullToHellRenderer : MonoBehaviour
{
    // PullToHell�����ڼ���animation event����
    public void PullGhostDown()
    {
        AnimationManager.Instance.PullGhostDown();
    }

    // PullToHell������������animation event����
    public void PullAnimEnd()
    {
        //AnimationManager.Instance.ResetGhostSprite();
        //JudgeManager.Instance.JudgeEnd();
    }

}
