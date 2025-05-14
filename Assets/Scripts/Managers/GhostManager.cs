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
    }

    private void OnDestroy()
    {
        uiManager.onLightStateChanged -= OnLightStateChanged;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
