using UnityEngine;
using Sirenix.OdinInspector;
using System;
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
