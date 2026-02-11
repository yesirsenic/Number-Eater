using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private float minSpawnRate;
    private float baseSpeed = 10f;
    private float maxSpeed = 30f;
    private bool is_End;
    private GameObject goalLine;
    

    public int nowNumberSum = 0;
    public int getNumberSum = 0;
    public float numberSpeed;
    public bool is_Clear;
    public GameState state;


    [SerializeField]
    Spawner spawner;

    [SerializeField]
    GameObject userNumber;

    [SerializeField]
    GameObject goal_LinePrefab;

    [SerializeField]
    GameObject number_Comps;

    [SerializeField]
    GameObject Clear_Popup;

    [SerializeField]
    GameObject GameOver_Popup;

    [SerializeField]
    Text level_Text;

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
        if(goalLine != null)
        {
            Destroy(goalLine);
        }
        state = GameState.MainGame;
        is_End = false;
        is_Clear = false;
        level = PlayerPrefs.GetInt("Level");
        level_Text.text = "Level " + level.ToString();
        nowNumberSum = 0;
        getNumberSum = 0;
        float rawGoal = 50 + level * 10f / (1f + level * 0.05f);
        goal = Mathf.RoundToInt(rawGoal / 10f) * 10;
        possible_NumberSum = goal + 25;
        spawnRate = Mathf.Max(minSpawnRate, 1f - Mathf.Log(level + 1f) * 0.15f); // юс╫ц
        numberSpeed = Mathf.Min(maxSpeed, baseSpeed + Mathf.Log(level + 1f) * 4f);
        userNumber.GetComponent<SumNumberChange>().RefreshNumberView();
        SetPlayerStartPos();
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
        goalLine = Instantiate(goal_LinePrefab, new Vector3(0, -3.99f, goal_Num + 50), Quaternion.identity);

        goalLine.GetComponent<GoalLine>().UpdateText(goal_Num.ToString());
    }

    void SetPlayerStartPos()
    {
        userNumber.transform.position = new Vector3(0, -4f, -5.5f);
    }

    public void NumberUserChange()
    {
        userNumber.GetComponent<SumNumberChange>().RefreshNumberView();
    }

    public void SetClear()
    {
        if(getNumberSum >= goal)
        {
            is_Clear = true;
            level++;
            PlayerPrefs.SetInt("Level", level);
        }
    }

    public void StartGameEndAniamtor()
    {
        userNumber.GetComponent<UserClear>().ClearAnimator();
    }

    public void GameEndPopupOn()
    {
        if(is_Clear)
        {
            Clear_Popup.SetActive(true);
        }

        else
        {
            GameOver_Popup.SetActive(true);
        }
    }

    public void GameRetry()
    {
        __Init__();
    }
}
