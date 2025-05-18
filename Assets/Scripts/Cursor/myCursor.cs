using UnityEngine;

public class myCursor : MonoBehaviour
{

    public bool isCatPaw = false;
    public RectTransform rectTransform;
    private Camera uiCamera;

    private Vector2 sizeDelta;
    private CanvasGroup canvasGroup;
    public Texture2D originCursor; // A transparent or invisible cursor texture

    public Texture2D hiddenCursor; // A transparent or invisible cursor texture
                                   // Start is called once before the first execution of Update after the MonoBehaviour is created

    private UIManager uIManager;
    void Start()
    {
        uIManager = FindAnyObjectByType<UIManager>(FindObjectsInactive.Include);
        sizeDelta = rectTransform.sizeDelta;
        // 确保 RectTransform 的 pivot 在顶部中心
        rectTransform.pivot = new Vector2(0.5f, 1.0f);

        // 获取 UI 相机，通常是 Canvas 的相机
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = null; // ScreenSpaceOverlay 不需要相机
        }
        else
        {
            uiCamera = canvas.worldCamera;
        }

    }

    void OnEnable()
    {

        // 设置鼠标光标并锁定
        SetCustomCursor(hiddenCursor);

        // Make CatPaw non-interactable so it doesn't block raycasts
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame  
    void Update()
    {

        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标位置转换为 UI 坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, mousePosition, uiCamera, out localPoint);

        //为了防止猫爪伸的太外面
        // 计算新的 anchoredPosition 以顶部对齐
        // rectTransform.anchoredPosition = localPoint + new Vector2(0, rectTransform.sizeDelta.y / 2);
        //最大362
        rectTransform.anchoredPosition = localPoint + new Vector2(0, 50);
        if (isCatPaw)
        {
            if (rectTransform.anchoredPosition.y > 312)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 312);
            }
        }

    }

    void OnDisable()
    {
        // 显示鼠标光标并解除锁定
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
        // 将热点设置为光标中心
        Cursor.SetCursor(originCursor, Vector2.zero, CursorMode.Auto);
    }

    private void SetCustomCursor(Texture2D cursorTexture)
    {
        // 将热点设置为光标中心
        // Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

}
