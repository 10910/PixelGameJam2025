using UnityEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
public class Ghost : MonoBehaviour
{

    [SerializeField]
    public GhostType ghostType;
    public GhostDataSO ghostDataSO;
    [SerializeField] public GhostDialogue ghostDialogue;

    [SerializeField] public SpriteRenderer ghostSpriteRenderer;

    void Awake()
    {
        ghostSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // ghostDialogue = GetComponentInChildren<GhostDialogue>();
    }



    private void OnEnable()
    {
        //等待一秒
        Invoke("StartDialogue", 1f);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ghostDialogue.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    // void OnMouseDown()
    // {
    //     DialogueManager.StartConversation("Judgment_LittleGirl");
    // }

    [Button("StartDialogue")]
    public void StartDialogue()
    {
        ghostDialogue.gameObject.SetActive(true);
    }
}
