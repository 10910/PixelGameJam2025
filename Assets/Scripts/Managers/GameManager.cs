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
    public void StartGame()
    {
        SetGameState(GameState.Game);
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
