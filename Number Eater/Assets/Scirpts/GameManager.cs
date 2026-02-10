using System.Collections;
using UnityEngine;

public enum GameState
{
    MainGame, GameEnd
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int goal;
    private int level = 1;
    private int possible_NumberSum;
    private float spawnRate;
    private bool is_End;

    public int nowNumberSum = 0;
    public int getNumberSum = 0;
    public float numberSpeed;

    public GameState state;


    [SerializeField]
    Spawner spawner;

    [SerializeField]
    GameObject userNumber;

    [SerializeField]
    GameObject goal_LinePrefab;

    [SerializeField]
    GameObject number_Comps;

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
        state = GameState.MainGame;
        is_End = false;
        level = PlayerPrefs.GetInt("Level");
        nowNumberSum = 0;
        getNumberSum = 0;
        goal = level * 50;
        possible_NumberSum = goal * 3 / 2;
        spawnRate = 1f; // 임시
        numberSpeed = 30f; // 임시
        SetGoalLine(goal);
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

        is_End = true;
        StartCoroutine(CheckEnd());
    }

    IEnumerator CheckEnd()
    {
        while(is_End)
        {
            yield return new WaitForSeconds(0.5f);

            if (number_Comps.transform.childCount == 0)
            {
                is_End = false;
                state = GameState.GameEnd;
                userNumber.GetComponent<NumberRunnerController>().RunStart();
                break;

            }

            
        }
    }

    void SetGoalLine(int goal_Num)
    {
        Instantiate(goal_LinePrefab, new Vector3(0, -3.99f, goal_Num + 50), Quaternion.identity);
    }

    public void NumberUserChange()
    {
        userNumber.GetComponent<SumNumberChange>().RefreshNumberView();
    }
}
