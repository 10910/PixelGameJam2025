using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimationManager: MonoBehaviour{
    public static AnimationManager Instance;

    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;
    [SerializeField] private Image blackBackground;
    [SerializeField] public GameObject ghostSprite;

    [SerializeField] private LittleDemon littleDemon;
    [SerializeField] private PullToHellEffect pullToHellEffect;
    [SerializeField] private RebirthEffect rebirthEffect;
    [SerializeField] private Demon demon;

    public static Action onJudgeAnimEnd;
    private void Awake() {
        Instance = this;
        GameManager.onStartNewRound += OnStartNewRound;
    }

    
    private void Start() {
        
    }

    void OnStartNewRound(){
        Light.SetActive(false);
        ghostSprite.SetActive(false);
        Background_Light.SetActive(false);
        Background_Dark.SetActive(true);
        blackBackground.color = Color.black;
    }

    [Button("Open Light")]
    public Sequence OpenLight() {
        Sequence seq = DOTween.Sequence();
        // 1��󿪵ƣ� �ٹ�0.5���Ļ��ʧ
        seq.AppendInterval(1.0f)
           .AppendCallback(() =>
           {
               ghostSprite.SetActive(true);
               Light.SetActive(true);
               Background_Light.SetActive(true);
           })
           .AppendInterval(0.5f)
           .AppendCallback(() =>
           {
               // ���ú�Ļ͸����Ϊ0
               Color c = blackBackground.color;
               c.a = 0f;
               blackBackground.color = c;
           });
        return seq;
    }

    [Button("Close Light")]
    public Sequence CloseLight() {
        Sequence seq = DOTween.Sequence();
        // 1���صƣ� �ٹ�0.5��ȫ��
        seq.AppendInterval(1f)
           .AppendCallback(() =>
           {
               Light.SetActive(false);
               // �л����޹��ձ���
               Background_Light.SetActive(false);
               Background_Dark.SetActive(true);
           })
           .AppendInterval(0.5f)
           .AppendCallback(() =>
           {
               // ���ú�Ļ͸����Ϊ1
               Color c = blackBackground.color;
               c.a = 1f;
               blackBackground.color = c;
           });
        return seq;
    }

    [Button("Reset Ghost Sprite")]
    public void ResetGhostSprite(){
        ghostSprite.transform.localPosition = new Vector3(0, 0, 0);
    }

    [Button("Pull To Hell")]
    public void PlayPullToHell(){
        pullToHellEffect.gameObject.SetActive(true);
        demon.PullToHell();
    }

    public void PullGhostDown(){
        pullToHellEffect.PullGhostDown();
    }

    void PlayReborn(){

    }

}