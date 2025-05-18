using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;

    public GameObject ghostSprite;
    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;
    [SerializeField] private Image blackBackground;
    [SerializeField] private LittleDemon littleDemon;
    [SerializeField] private PullToHellEffect pullToHellEffect;
    [SerializeField] private RebirthEffect rebirthEffect;
    [SerializeField] private Demon demon;

    private LittleDemonScale littleDemonScale;
    private Light lighting;

    public static Action onJudgeAnimEnd;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        lighting = FindAnyObjectByType<Light>();
        littleDemonScale = FindAnyObjectByType<LittleDemonScale>();
        GameManager.onStartNewRound += OnStartNewRound;
    }

    private void OnDestroy()
    {
        GameManager.onStartNewRound -= OnStartNewRound;
    }

    private void Start()
    {

    }
    void OnStartNewRound()
    {
        lighting.CloseLightImmediately();
        ghostSprite.SetActive(false);
        Background_Light.SetActive(false);
        Background_Dark.SetActive(true);
        blackBackground.color = Color.black;
    }

    [Button("Open Light")]
    public Sequence OpenLight()
    {
        Sequence seq = DOTween.Sequence();
        // 1秒后开灯， 再过0.5秒黑幕消失
        seq.AppendInterval(1.0f)
           .AppendCallback(() =>
           {
               ghostSprite.SetActive(true);
               lighting.OpenLight();
               Background_Light.SetActive(true);
               // 延迟0.7秒开始幽灵对话
               GhostManager.Instance.StartGhostDialogue(0.7f);
           })
           .AppendInterval(0.5f)
           .AppendCallback(() =>
           {
               // 设置黑幕透明度为0
               Color c = blackBackground.color;
               c.a = 0f;
               blackBackground.color = c;
           });
        return seq;
    }

    [Button("Close Light")]
    public Sequence CloseLight()
    {
        Sequence seq = DOTween.Sequence();
        // 1秒后关灯， 再过0.5秒全黑
        seq.AppendInterval(1f)
           .AppendCallback(() =>
           {
               lighting.CloseLight();
               // 切换成无光照背景
               Background_Light.SetActive(false);
               Background_Dark.SetActive(true);
           })
           .AppendInterval(0.5f)
           .AppendCallback(() =>
           {
               // 设置黑幕透明度为1
               Color c = blackBackground.color;
               c.a = 1f;
               blackBackground.color = c;
           });
        return seq;
    }

    [Button("Reset Ghost Sprite")]
    public void ResetGhostSprite()
    {
        ghostSprite.transform.localPosition = new Vector3(0, 0, 0);
    }

    [Button("Pull To Hell")]
    public void PlayPullToHell()
    {
        pullToHellEffect.gameObject.SetActive(true);
        Debug.Log("AnimationManager PlayPullToHell");
        demon.PullToHell();
        GhostManager.Instance.StopGhostDialogue();
        littleDemon.WalkBackToGetNewFile();
        littleDemonScale.DisableInteraction();
    }

    public void PlayRebirth()
    {
        rebirthEffect.gameObject.SetActive(true);
        Debug.Log("AnimationManager PlayPullToHell");
        demon.Rebirth();
        GhostManager.Instance.StopGhostDialogue();
        littleDemon.WalkBackToGetNewFile();
        littleDemonScale.EnableInteraction();
    }

    public void PullGhostDown()
    {
        pullToHellEffect.PullGhostDown();
    }
}