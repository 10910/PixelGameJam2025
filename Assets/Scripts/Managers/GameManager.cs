using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private GameState gameState;

    [Header("Actions")]
    public Action onGamePause;
    public Action onGameResume;
    public static Action onStartNewJudge;
    public static Action onStartNewRound;
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 设置帧率
        Application.targetFrameRate = 60;
        SetGameState(GameState.Menu);
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        IEnumerable<IGameStateListener> gameStateListeners =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IGameStateListener>();

        foreach (IGameStateListener listener in gameStateListeners)
        {
            listener.GameStateChangedCallback(gameState);
        }

    }
    public void StartGame()//menu的play里调用
    {
        SetGameState(GameState.Game);
        StartNewRound();
    }

    //游戏开始时调用 还有结算页面的按钮调用
    public void StartNewRound()
    {
        //应该直接调用生成新ghost 做成事件
        onStartNewRound?.Invoke();
        //这里直接生成一组ghost数据
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

    public void SettingsButtonCallback()
    {
        Time.timeScale = 0;
        onGamePause?.Invoke();
    }

    public void ResumeButtonCallback()
    {
        Time.timeScale = 1;
        onGameResume?.Invoke();
    }

}

public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}
