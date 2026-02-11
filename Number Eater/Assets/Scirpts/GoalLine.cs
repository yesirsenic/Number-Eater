using TMPro;
using UnityEngine;

public class GoalLine : MonoBehaviour
{
    [SerializeField]
    TextMeshPro textMesh;

    public void UpdateText(string newText)
    {
        textMesh.text = newText;
    }
}
