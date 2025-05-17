using UnityEngine;

public class Demon : MonoBehaviour
{

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rebirth()
    {
        animator.Play("Demon_raiseHand_Rebirth");
    }

    public void PullToHell()
    {
        Debug.Log("Demon PullToHell");
        // animator.Play("Demon_raiseHand_Hell");
        animator.Play("Demon_raiseHand_Hell", -1, 0f); // 强制从头开始播放动画
    }
}
