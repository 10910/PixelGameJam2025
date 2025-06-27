using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Lang language;
    public bool spceialGhostTestMode = false;

    private GameState gameState;

    [Header("Actions")]
    public Action onGamePause;
    public Action onGameResume;
    public static Action onStartNewRound;
    public int RoundsPlayed = 0; // 每局开始时+1

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
        RoundsPlayed++;
        //应该直接调用生成新ghost 做成事件
        onStartNewRound?.Invoke();
        print("new round start");
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
