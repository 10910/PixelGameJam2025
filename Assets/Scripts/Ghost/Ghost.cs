using UnityEngine;
using PixelCrushers.DialogueSystem;
public class Ghost : MonoBehaviour
{

    [SerializeField]
    public GhostType ghostType;
    public GhostDataSO ghostDataSO;

    [SerializeField] public SpriteRenderer ghostSpriteRenderer;

    void Awake()
    {
        ghostSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    void OnMouseDown() {
        DialogueManager.StartConversation("Judgment_LittleGirl");
    }
}
