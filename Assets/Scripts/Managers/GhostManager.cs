using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;
    [SerializeField] public Ghost currentGhost;
    [SerializeField] private Transform ghostSpawnPoint;

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
        ghostStartPoint = ghostSpawnPoint;
    }

    private void OnDestroy()
    {
    }

    public void GenerateNewGhost()
    {
        GetNewGhostDialogue();
    }

    public void GetNewGhostDialogue()
    {
        currentGhost.ghostDialogue.GetNewDialogue();
    }

    //应该在幽灵出现后等待一秒调用
    public void StartGhostDialogue()
    {
        currentGhost.StartDialogue();
    }

    public void StopGhostDialogue()
    {
        currentGhost.StopDialogue();
    }

    public void StartGhostDialogue(float delay)
    {
        Invoke("StartGhostDialogue", delay);
    }

}
