using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    private MouseCursor mouseCursor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseCursor = FindAnyObjectByType<MouseCursor>(FindObjectsInactive.Include);
    }

    private void OnEnable()
    {
        mouseCursor.DisableDefaultCursor();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
