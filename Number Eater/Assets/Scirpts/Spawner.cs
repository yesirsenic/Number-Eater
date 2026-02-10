using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] prefabs;

    [SerializeField]
    private GameObject SpawnComp;

    public void SpawnRandom()
    {
        if (spawnPoints.Length == 0 || prefabs.Length == 0)
        {
            Debug.LogWarning("SpawnPoints 또는 Prefabs가 비어있습니다.");
            return;
        }

        // 랜덤 좌표 선택
        Transform randomPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        int randomIndex = Random.Range(0, prefabs.Length);

        // 랜덤 프리팹 선택
        GameObject randomPrefab =
            prefabs[randomIndex];

        GameObject NumberObject = Instantiate(
            randomPrefab,
            randomPoint.position,
            randomPoint.rotation
        );

        NumberObject.transform.SetParent(SpawnComp.transform);

        GameManager.Instance.nowNumberSum += randomIndex + 1;
    }
}
