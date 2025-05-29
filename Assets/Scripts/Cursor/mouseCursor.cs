using UnityEngine;

public class MouseCursor : MonoBehaviour
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

    void OnDisable()
    {
        EnableDefaultCursor();
    }


    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0))
        {
            DisableDefaultCursor();
            rend.sprite = handCursor;
            // Instantiate(clickEffect, transform.position, Quaternion.identity);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DisableDefaultCursor();
            rend.sprite = normalCursor;
        }

        if (timeBtwSpawn <= 0)
        {
            // 此处的逻辑不完整，可能需要补充内容
        }
    }

    public void DisableDefaultCursor()
    {
        // 隐藏Unity内部的鼠标
        Cursor.visible = false;
        // 锁定鼠标位置在屏幕中心（可选）
        // Cursor.lockState = CursorLockMode.Locked;
        SetCustomCursor(hiddenCursor);
    }

    public void EnableDefaultCursor()
    {
        Cursor.visible = true;
    }

    private void SetCustomCursor(Texture2D cursorTexture)
    {
        // 将热点设置为光标中心
        // Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}

