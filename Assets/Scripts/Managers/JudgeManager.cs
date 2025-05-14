using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    public static JudgeManager Instance;
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

    }

    // Update is called once per frame
    void Update()
    {

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
