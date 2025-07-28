using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrushers.DialogueSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening.Plugins.Core.PathCore;

[Serializable]
public class SaveData{
    public Dictionary<string, int[]> judgementHistory;
    public Dictionary<string, bool> endingHistory;
    public int roundsPlayed;
    public int goodness;
    public bool isEndingBad2;
    public bool isFirstJudgement;
    public bool hasAddictedAppeared;
}
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

    public SaveData saveData;
    string savePath;

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

        savePath = Application.persistentDataPath + "/savedata.data";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 设置帧率
        Application.targetFrameRate = 60;
        SetGameState(GameState.Menu);

        if (language == Lang.Chinese) {
            DialogueManager.SetLanguage("cn");
        }

        JudgeManager.onRoundEnd += SaveGame;
        LoadGame();
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

    public void QuitGame(){
        Application.Quit();
    }

    public void SaveGame(){
        saveData.goodness = JudgeManager.Instance.totalGoodness;
        saveData.endingHistory = JudgeManager.Instance.endingHistory;
        saveData.judgementHistory = JudgeManager.Instance.history;
        saveData.roundsPlayed = RoundsPlayed;
        saveData.isFirstJudgement = JudgeManager.Instance.isFirstJudgement;
        saveData.isEndingBad2 = JudgeManager.Instance.isEndingBad2;
        saveData.hasAddictedAppeared = JudgeManager.Instance.hasAddictedAppeared;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(savePath)) {
            bf.Serialize(file, saveData);
        }
        //string json = JsonUtility.ToJson(saveData);
        //File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        print("data saved");
    }

    public void LoadGame(){
        if (File.Exists(savePath)) {
            //string json = File.ReadAllText(savePath);
            //SaveData data = JsonUtility.FromJson<SaveData>(json);
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(savePath, FileMode.Open)) {
                saveData = (SaveData)bf.Deserialize(file);
                JudgeManager.Instance.totalGoodness = saveData.goodness;
                JudgeManager.Instance.history = saveData.judgementHistory;
                JudgeManager.Instance.endingHistory = saveData.endingHistory;
                RoundsPlayed = saveData.roundsPlayed;
                JudgeManager.Instance.isFirstJudgement = saveData.isFirstJudgement;
                JudgeManager.Instance.isEndingBad2 = saveData.isEndingBad2;
                JudgeManager.Instance.hasAddictedAppeared = saveData.hasAddictedAppeared;
            }

            Debug.Log("load save data, goodness: " + saveData.goodness);
        }
        else{
            print("no data");
            saveData = new SaveData();
        }
    }

}

public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}
