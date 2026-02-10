using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int goal;
    private int level = 1;
    private int possible_NumberSum;
    
    private float spawnRate;

    public int nowNumberSum = 0;
    public int getNumberSum = 0;
    public float numberSpeed;


    [SerializeField]
    Spawner spawner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        __Init__();
    }

    private void __Init__()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        level = PlayerPrefs.GetInt("Level");
        nowNumberSum = 0;
        getNumberSum = 0;
        goal = level * 50;
        possible_NumberSum = goal * 3 / 2;
        spawnRate = 2f; // 임시
        numberSpeed = 10f; // 임시

        StartSpawn();


    }

    void StartSpawn()
    {
        StartCoroutine(NumberSpawn());
    }

    IEnumerator NumberSpawn()
    {
        while(nowNumberSum < possible_NumberSum)
        {
            spawner.SpawnRandom();

            yield return new WaitForSeconds(spawnRate);

            
        }
    }
}
