using UnityEngine;
using UnityEngine.EventSystems;

public class LittleDemonButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool isID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isID)
        {
            UIManager.Instance.OpenFilesPanel();
        }
        else
        {
            UIManager.Instance.OpenIDPanel();
        }
    }
}
