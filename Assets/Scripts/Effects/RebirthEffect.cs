using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class RebirthEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Sprite babysprite;
    [SerializeField] private Sprite ratBabysprite;
    [SerializeField] private Vector3 babyStartPosition = new Vector3(0, 1.7f, 0);
    // 上浮的持续时间
    [SerializeField] private float floatDuration = 1.5f;
    // 上浮的距离

    [SerializeField] private float floatDistance = 5.0f;

    // 存储动画引用
    private Tween babyMoveTween;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Rebirth();
    }


    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    public void Rebirth()
    {
        AnimationManager.Instance.ghostSprite.SetActive(false);
        ChangeBabySprite();
        //等待一秒

        babyMoveTween = transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
                 .SetEase(Ease.InOutSine) // 使用缓动效果
                 .SetDelay(0.7f) // 延迟一秒开始
                 .OnComplete(() =>
                 {
                     ResetBaby();
                     JudgeManager.Instance.JudgeEnd();
                 });
    }

    private void ChangeBabySprite()
    {
        if (GhostManager.Instance.currentGhost.ghostType == GhostType.rat)
        {
            spriteRenderer.sprite = ratBabysprite;
        }
        else
        {
            spriteRenderer.sprite = babysprite;
        }
    }
    [Button]
    public void ResetBaby()
    {
        StopAnimation();
        transform.position = babyStartPosition;
        gameObject.SetActive(false);
    }

    public void StopAnimation()
    {
        if (babyMoveTween != null && babyMoveTween.IsActive())
        {
            babyMoveTween.Kill();
            // 如果需要重置位置
            // Kill(true); // true表示完全重置
        }
    }


}
