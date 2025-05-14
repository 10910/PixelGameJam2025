using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class PullToHellEffect : MonoBehaviour
{
    [SerializeField] private float pullToHellDistance = 10.0f;
    [SerializeField] private float pullToHellDuration = 4.0f;
    private Animator pullToHellAnimator;

    private void Awake()
    {
        pullToHellAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        pullToHellAnimator.Play("PullToHell");
    }

    [Button]
    public void PullGhostDown()
    {
        //让ghost跟着往下移动
        GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.DOMoveY(transform.position.y - pullToHellDistance, pullToHellDuration);
    }

    [Button]
    public void ResetGhost()
    {
        GhostManager.Instance.currentGhost.ghostSpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);

    }

}
