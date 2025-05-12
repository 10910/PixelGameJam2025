using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ghost Data", menuName = "Scriptable Objects/New Ghost Data", order = 0)]
public class GhostDataSO : ScriptableObject
{
    [field: SerializeField]
    public string ghostName { get; private set; }

    [SerializeField]
    public string ghostDescription;
    [field: SerializeField]
    public Sprite sprite { get; private set; }
    [SerializeField]
    public GhostType ghostType;//Blue or Red

    [SerializeField]
    public List<GhostDocument> ghostDocuments;//list 可能不太合适？ 因为文件有很多细分的种类

}
