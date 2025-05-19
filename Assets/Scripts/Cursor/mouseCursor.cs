using UnityEngine;

public class mouseCursor : MonoBehaviour
{

    private SpriteRenderer rend;
    public Sprite handCursor;
    public Sprite normalCursor;
    public Texture2D hiddenCursor; // A transparent or invisible cursor texture

    public GameObject clickEffect;
    public float timeBtwSpawn = 0.1f;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        DisableDefaultCursor();
    }

    void OnEnable()
    {
        DisableDefaultCursor();
    }


    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0))
        {
            rend.sprite = handCursor;
            // Instantiate(clickEffect, transform.position, Quaternion.identity);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rend.sprite = normalCursor;
        }

        if (timeBtwSpawn <= 0)
        {
            // 此处的逻辑不完整，可能需要补充内容
        }
    }

    public void DisableDefaultCursor()
    {
        Cursor.visible = false;
        SetCustomCursor(hiddenCursor);
    }

    private void SetCustomCursor(Texture2D cursorTexture)
    {
        // 将热点设置为光标中心
        // Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}

