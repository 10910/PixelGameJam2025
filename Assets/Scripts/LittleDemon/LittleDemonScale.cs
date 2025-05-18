using UnityEngine;
using UnityEngine.UI;
public class LittleDemonScale : MonoBehaviour
{

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        GhostDialogue.onDialogueStart += DisableInteraction;
        GhostDialogue.onDialogueEnd += EnableInteraction;
    }

    private void OnDestroy()
    {
        GhostDialogue.onDialogueStart -= DisableInteraction;
        GhostDialogue.onDialogueEnd -= EnableInteraction;
    }


    //禁止交互  
    public void DisableInteraction()
    {
        button.interactable = false;
    }

    //恢复交互
    public void EnableInteraction()
    {
        button.interactable = true;
    }
}
