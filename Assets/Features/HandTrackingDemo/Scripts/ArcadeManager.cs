using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using System;

public class ArcadeManager: MonoBehaviour
{
    private static ArcadeManager _instance;
    public static ArcadeManager instance
    {
        get => _instance;
        private set
        {
            if (_instance != null)
                Debug.LogWarning("Second attempt to get ArcadeManager");
            _instance = value;
        }
    }

    public enum GameState
    {
        None,
        Waiting,
        Active,
        End,
    }

    public GameState currGameState = GameState.None;
    [Header("Coin Components")]
    public Transform grabCoinTransform;
    public GameObject coinPrefab;
    public string CurrCoinValStr { get; set; }
    public int CurrCoinVal { get; set; }

    public bool CoinGrabbed { get; set; }
    private GameObject currCoin = null;


    [Header("Text Components")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;
    public TextMeshProUGUI timeText;


    [Header("Game Area Components")]
    public GameObject arcadePlayArea;
    public GameObject targetArea;
    public GameObject tutorialButton;
    public GameObject arcadeButton;
    public GameObject zoneParent;
    public Transform[] zoneList;


    [Header("Game Logic Components")]
    private List<GameObject> hatList = new List<GameObject>();
    public GameObject[] scoreHatPrefabs;
    public float maxTime = 60f;

    private GameObject prevHat;
    private bool last3Seconds = false;
    private float currTime = 0;
    private int currScore = 0;
    private int topScore = 0;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IntroSetup();
    }

    // Update is called once per frame
    void Update()
    {
        TimerUpdate();
    }

    public bool ArcadeReady()
    {
        return currGameState == GameState.Waiting;
    }

    //Arcade Logic methods area
    private void IntroSetup()
    {
        ResetScore();
        zoneList = zoneParent.GetComponentsInChildren<Transform>();
        currGameState = GameState.Waiting;
        arcadePlayArea.SetActive(false);
        CoinGrabbed = false;
        Random.InitState((int)System.DateTime.Now.Ticks); //To Randomize the random num gen
    }

    public void StartNewGame()
    {
        arcadeButton.SetActive(false);
        tutorialButton.SetActive(false);
        last3Seconds = false;
        currTime = maxTime;
        ResetScore();
        ResetHats();
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        SoundManager.instance.PlayReady();
        arcadePlayArea.SetActive(true);
        targetArea.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        SoundManager.instance.PlayGo();
        yield return new WaitForSeconds(.2f);

        currGameState = GameState.Active;
        AddScoreHat();
    }

    public void ResetHats()
    {
        foreach (GameObject box in hatList)
            Destroy(box);

        hatList.Clear();
    }

    public void ResetScore()
    {
        currScore = 0;
        scoreText.text = currScore.ToString();
    }

    public void AddScoreHat()
    {
        int prefabNum = Random.Range(0, scoreHatPrefabs.Length);
        int zoneNum = Random.Range(1, zoneList.Length);//zoneList[0] reserved for prev zone
        Transform zonePos = zoneList[zoneNum];
        ZoneSwap(zoneNum);

        Quaternion hatRotation = scoreHatPrefabs[prefabNum].transform.localRotation;
        GameObject newHat = Instantiate(scoreHatPrefabs[prefabNum], zonePos.position, hatRotation);
        hatList.Add(newHat);
        prevHat = newHat;

        SoundManager.instance.PlaySpawn();
    }

    private void ZoneSwap(int idx)
    {
        //swap zone so not to repeat zone
        Transform prevZone = zoneList[0];
        zoneList[0] = zoneList[idx];
        zoneList[idx] = prevZone;
    }

    public void UpdateScore()
    {
        currScore += 1;
        scoreText.text = currScore.ToString();
        SoundManager.instance.PlayCoin();

        if(prevHat != null)
            hatList.Remove(prevHat);
        prevHat = null;

        AddScoreHat();
    }

    private void TimerUpdate()
    {
        if (currGameState == GameState.Active)
        {
            currTime -= 1 * Time.deltaTime;
            timeText.text = currTime.ToString("0");

            if(currTime <= 3f && !last3Seconds)
            {
                last3Seconds = true;
                StartCoroutine(StartCountDown());
            }

            if (currTime <= 0)
                StartCoroutine(EndSequence());
        }
    }

    IEnumerator StartCountDown()
    {
        SoundManager.instance.Play3();
        yield return new WaitForSeconds(1f);

        SoundManager.instance.Play2();
        yield return new WaitForSeconds(1f);

        SoundManager.instance.Play1();
    }

    IEnumerator EndSequence()
    {
        currGameState = GameState.End;
        SoundManager.instance.PlayTimeover();
        ResetHats();
        DestroyCoin();
        targetArea.SetActive(false);
        yield return new WaitForSeconds(3f);

        CheckTopScore();
        yield return new WaitForSeconds(3f);

        SoundManager.instance.PlaySuccess();
        GameOver();
    }

    private void CheckTopScore()
    {
        if (currScore > topScore)
        {
            topScoreText.text = currScore.ToString();
            topScore = currScore;
            SoundManager.instance.PlayHighScore();
        }
        else
            SoundManager.instance.PlayGameOver();
    }

    public void GameOver()
    {
        currTime = 0;
        timeText.text = "0";
        currScore = 0;
        scoreText.text = "0";

        arcadePlayArea.SetActive(false);
        currGameState = GameState.Waiting;

        ResetHats();
        DestroyCoin();

        arcadeButton.SetActive(true);
        tutorialButton.SetActive(true);
    }

    //Coin methods area
    public void DestroyCoin()
    {
        if (currCoin)
            Destroy(currCoin);
        currCoin = null;
        CoinGrabbed = false;
    }

    public void MakeNewCoin()
    {
        DestroyCoin();

        currCoin = GameObject.Instantiate(coinPrefab, grabCoinTransform.position, grabCoinTransform.rotation);
        GrabCoinCtrl grabcc = currCoin.GetComponent<GrabCoinCtrl>();
        grabcc.SetUpCoin(CurrCoinVal, CurrCoinValStr);
    }

    public void MakeCoinOne()
    {
        CurrCoinVal = 1;
        CurrCoinValStr = "1";
        MakeNewCoin();
    }

    public void MakeCoinTwo()
    {
        CurrCoinVal = 2;
        CurrCoinValStr = "2";
        MakeNewCoin();
    }

    public void MakeCoinThree()
    {
        CurrCoinVal = 3;
        CurrCoinValStr = "3";
        MakeNewCoin();
    }

    

}
