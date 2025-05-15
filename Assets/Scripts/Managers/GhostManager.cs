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
        //注册事件 之后要改
        GameManager.onStartNewRound += GenerateNewGhost;
        // JudgeManager.onStartNewJudge += StartDialogue;
    }

    private void OnDestroy()
    {
        //注销事件
        uiManager.onLightStateChanged -= OnLightStateChanged;
        GameManager.onStartNewRound -= GenerateNewGhost;
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
    // }
    public void GenerateNewGhost()
    {
        //从so获取随机数据

        // currentGhost = Instantiate(ghostPrefab, transform);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
