using UnityEngine;
using DG.Tweening;
public class RebirthEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Sprite babysprite;
    [SerializeField] private Sprite ratBabysprite;
    // 上浮的持续时间
    [SerializeField] private float floatDuration = 5.0f;
    // 上浮的距离

    [SerializeField] private float floatDistance = 10.0f;

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

    public void Rebirth()
    {
        ChangeBabySprite();
        //等待一秒

        transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
                 .SetEase(Ease.InOutSine) // 使用缓动效果
                 .SetDelay(0.7f); // 延迟一秒开始

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
}
