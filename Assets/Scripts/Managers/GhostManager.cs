using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;
    [SerializeField] public Ghost currentGhost;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private Transform ghostSpawnPoint;

    private PullToHellEffect pullToHellEffect;

    private Transform ghostStartPoint;

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
        pullToHellEffect = FindAnyObjectByType<PullToHellEffect>(FindObjectsInactive.Include);

        uiManager.onLightStateChanged += OnLightStateChanged;
        //注册事件 之后要改
        GameManager.onStartNewRound += GenerateNewGhost;
        // JudgeManager.onStartNewJudge += StartDialogue;
        ghostStartPoint = ghostSpawnPoint;
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
            //重置ghost位置
            ResetGhostPosition();
            currentGhost.gameObject.SetActive(true);
        }
        else//关灯
        {
            currentGhost.gameObject.SetActive(false);
            //重置ghost位置
            ResetGhostPosition();
            //开始新的一轮裁决
            JudgeManager.Instance.StartNewJudge();
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

    //什么时候用？
    //这个方法有问题 不能获得现在的准确位置
    public void ResetGhostPosition()
    {
        // Debug.Log("ResetGhostPosition");
        // // currentGhost.transform.position = ghostSpawnPoint.position;

        // // 如果有父对象，先解除父对象的影响
        // Transform originalParent = currentGhost.transform.parent;
        // currentGhost.transform.SetParent(null, true);

        // // 设置世界坐标
        // currentGhost.transform.position = ghostStartPoint.position;

        // // 恢复父对象
        // currentGhost.transform.SetParent(originalParent, true);

        // currentGhost.ghostSpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);

        pullToHellEffect.ResetGhost();
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
