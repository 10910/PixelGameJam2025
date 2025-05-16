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
        animator.Play("Demon_raiseHand_Hell");
    }
}
