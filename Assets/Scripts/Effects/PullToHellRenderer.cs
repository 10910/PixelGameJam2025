using UnityEngine;

public class PullToHellRenderer : MonoBehaviour
{
    private PullToHellEffect pullToHellEffect;

    private void Awake()
    {
        pullToHellEffect = GetComponentInParent<PullToHellEffect>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PullGhostDown()
    {
        pullToHellEffect.PullGhostDown();
    }

}
