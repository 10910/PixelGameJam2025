using UnityEngine;
using DG.Tweening;
public class LittleDemon : MonoBehaviour
{
    [SerializeField]
    public Vector3 startPosition;
    [SerializeField]
    public Vector3 targetPosition;
    [SerializeField]
    public float speed;
    [SerializeField]
    public GhostDocument ghostDocument;


    void OnEnable()
    {
        transform.position = startPosition;
        if (ghostDocument != null)
        {
            DeliverFile();
        }
    }

    public void DeliverFile()
    {
        float duration = Vector3.Distance(startPosition, targetPosition) / speed;
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear);
    }
}
