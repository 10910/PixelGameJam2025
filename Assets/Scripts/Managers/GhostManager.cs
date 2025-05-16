using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;
    [SerializeField] public Ghost currentGhost;
    [SerializeField] private UIManager uiManager;



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
        uiManager = FindAnyObjectByType<UIManager>(FindObjectsInactive.Include);
        uiManager.onLightStateChanged += OnLightStateChanged;
        // JudgeManager.onStartNewJudge += StartDialogue;
    }

    private void OnDestroy()
    {
        //注销事件
        uiManager.onLightStateChanged -= OnLightStateChanged;
        // JudgeManager.onStartNewJudge -= StartDialogue;
    }

    private void OnLightStateChanged(bool isLightOn)
    {
        if (isLightOn)
        {
            currentGhost.gameObject.SetActive(true);
        }
        else
        {
            currentGhost.gameObject.SetActive(false);
        }
    }

    // public void StartDialogue()
    // {
    //     currentGhost.ghostDialogue.gameObject.SetActive(true);

}
