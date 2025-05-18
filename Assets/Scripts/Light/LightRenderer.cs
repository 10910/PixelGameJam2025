using UnityEngine;

public class LightRenderer : MonoBehaviour
{
    private Light lighting;
    private void Awake()
    {
        lighting = FindAnyObjectByType<Light>();
    }


    public void CloseLight()
    {
        // lighting.CloseLightObject();
    }
}
