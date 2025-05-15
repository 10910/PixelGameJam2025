using UnityEngine;
using System;
public class JudgeManager : MonoBehaviour
{
    public static JudgeManager Instance;
    public static Action onStartNewJudge;

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
        GameManager.onStartNewRound += OnStartNewRoundCallback;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //开始新的裁决
    public void StartNewJudge()
    {
        onStartNewJudge?.Invoke();
        //等待1秒
        //开灯
        //生成新的ghost
        //等待1秒
        //ghost 开始说话
        //对话完成后
        //小怪移动资料
        //
    }

    private void OnStartNewRoundCallback()
    {
        StartNewJudge();
    }

    public void PullToHell()
    {
        UIManager.Instance.OpenPullToHellEffect();
    }

    public void Rebirth()
    {
        GhostManager.Instance.currentGhost.gameObject.SetActive(false);
        UIManager.Instance.OpenRebirthEffect();
    }
}
