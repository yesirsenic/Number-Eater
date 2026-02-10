using UnityEngine;

public class SumNumberChange : MonoBehaviour
{
    [SerializeField] private GameObject[] digitPrefabs; // 0~9
    [SerializeField] private Transform numberRoot;
    [SerializeField] private float digitSpacing = -0.4f;

    public void RefreshNumberView()
    {
        // 기존 자식 제거
        for (int i = numberRoot.childCount - 1; i >= 0; i--)
            Destroy(numberRoot.GetChild(i).gameObject);

        string numStr = GameManager.Instance.getNumberSum.ToString();
        int length = numStr.Length;

        float startX = -(length - 1) * digitSpacing * 0.5f;

        for (int i = 0; i < length; i++)
        {
            int digit = numStr[i] - '0';

            Vector3 pos = new Vector3(startX + i * digitSpacing, 0, 0);
            GameObject digitObj = Instantiate(
                digitPrefabs[digit],
                numberRoot
            );
            digitObj.transform.localPosition = pos;
        }
    }
}
