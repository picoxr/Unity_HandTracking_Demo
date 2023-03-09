using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;
    public static TutorialManager instance
    {
        get => _instance;
        private set
        {
            if (_instance != null)
                Debug.LogWarning("Second attempt to get TutorialManager");
            _instance = value;
        }
    }

    public enum TutorialState { None, Step0, Step1, Step2, Step3, Step4, Step5, Step6, Step7, Step8 };
    public TutorialState currTutorial;

    [Header("Game Area Components")]
    public GameObject tutorialPlayArea;
    public GameObject tutorialButton;
    public GameObject arcadeButton;
    //public GameObject coinPrefab;
    //public Transform coinPos;

    [Header("Text Instructions Components")]
    public TextMeshProUGUI targetInfoText;
    public GameObject targetInfoObj;
    public Animator targetInfoAnim;
    public TextMeshProUGUI coinInfoText;
    public GameObject coinInfoObj;
    public Animator coinInfoAnim;
    public GameObject bestArea;

    [Header("Target Components")]
    public BoxCollider target1;
    public BoxCollider target2;
    public BoxCollider target3;

    [Header("DropZone Components")]
    public GameObject dropZoneOne;
    public GameObject dropZoneTwo;
    public GameObject dropZoneThree;
    public GameObject dropZoneRed;

    private GameObject currentCoin = null;
    private bool hitTarget1 = false;
    private bool hitTarget2 = false;
    private bool hitTarget3 = false;




    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currTutorial = TutorialState.None;
        tutorialPlayArea.SetActive(false);
    }

    public bool TutorialReady()
    {
        return currTutorial == TutorialState.None;
    }

    //ideal hand positon for user
    public void StartStep0()
    {
        //Change to beginning
        currTutorial = TutorialState.Step0;
        //remove the starting area
        tutorialButton.SetActive(false);
        arcadeButton.SetActive(false);
        tutorialPlayArea.SetActive(true);
        //remove anything on table from last time
        ArcadeManager.instance.DestroyCoin();
        dropZoneOne.SetActive(false);
        dropZoneTwo.SetActive(false);
        dropZoneThree.SetActive(false);
        dropZoneRed.SetActive(false);
        //set up info
        targetInfoObj.SetActive(true);
        coinInfoObj.SetActive(true);
        StartCoroutine(ChangeTargetInfoText("This green box represents the best tracking Area"));
        StartCoroutine(ChangeCoinInfoText("Keep your hands in the area to start"));

        //set up targets
        target1.enabled = false;
        target2.enabled = false;
        target3.enabled = false;
        //hit checkpoints
        hitTarget1 = false;
        hitTarget2 = false;
        hitTarget3 = false;

        StartCoroutine(LoadBestArea());
    }

    IEnumerator LoadBestArea()
    {
        yield return new WaitForSeconds(3f);
        bestArea.SetActive(true);
    }
    
    //shoot the target 1
    public void StartStep1()
    {
        SoundManager.instance.PlayConfirm();
        //Change to beginning
        currTutorial = TutorialState.Step1;
        //set up info
        StartCoroutine(ChangeTargetInfoText("Shoot the 1 Target"));
        coinInfoText.text = "";
        //set up targets
        target1.enabled = true;
        target2.enabled = false;
        target3.enabled = false;
        bestArea.SetActive(false);

    }

    public void HitTarget1Success()
    {
        if(!hitTarget1 && currTutorial == TutorialState.Step1)
        {
            hitTarget1 = true;

            StartCoroutine(EndStep1());
        }
    }

    IEnumerator EndStep1()
    {
        coinInfoObj.SetActive(true);
        StartCoroutine(ChangeCoinInfoText("You now have a \n1 Coin!"));
        yield return new WaitForSeconds(2);
        StartStep2();
    }
    
    //shoot the second target
    public void StartStep2()
    {
        StartCoroutine(ChangeTargetInfoText("Shoot the 2 target"));
        coinInfoText.text = "";

        ArcadeManager.instance.DestroyCoin();
        SoundManager.instance.PlayConfirm();

        currTutorial = TutorialState.Step2;
        target1.enabled = false;
        target2.enabled = true;
        target3.enabled = false;
    }

    public void HitTarget2Success()
    {
        if (!hitTarget2 && currTutorial == TutorialState.Step2)
        {
            hitTarget2 = true;
            StartCoroutine(ChangeCoinInfoText("You now have a \n2 coin"));
            StartStep3();
        }
    }
    //grab the coin
    public void StartStep3() 
    {
        SoundManager.instance.PlayConfirm();

        currTutorial = TutorialState.Step3;
        EmptyTargetInfo();
        StartCoroutine(ChangeCoinInfoText("Grab the coin with Index and Thumb"));
    }

    public bool OnStep3()
    {
        return currTutorial == TutorialState.Step3;
    }

    //Grabbed the coin success
    public void StartStep4()
    {
        if(currTutorial == TutorialState.Step3)
        {
            SoundManager.instance.PlayConfirm();

            currTutorial = TutorialState.Step4;
            StartCoroutine(ChangeTargetInfoText("Move the Coin to the Hat"));
            StartCoroutine(ChangeCoinInfoText("Score points!"));
            dropZoneThree.SetActive(true);
        }
    }

    public bool OnStep4()
    {
        return currTutorial == TutorialState.Step4;
    }

    //Failed, wrong target to hat, now must shoot the target 3
    public void StartStep5()
    {
        if (currTutorial == TutorialState.Step4)
        {
            SoundManager.instance.PlayConfirm();

            currTutorial = TutorialState.Step5;
            StartCoroutine(ChangeTargetInfoText("Shoot the 3 target"));
            StartCoroutine(ChangeCoinInfoText("Coin must match the hat!"));

            target1.enabled = false;
            target2.enabled = false;
            target3.enabled = true;
        }
    }
    //match the coin to hat
    public void HitTarget3Success()
    {
        if (!hitTarget3 && currTutorial == TutorialState.Step5)
        {
            hitTarget3 = true;
            StartCoroutine(ChangeTargetInfoText("Drop the coin in the hat!"));
            StartCoroutine(ChangeCoinInfoText("You now have a \n3 coin"));
        }
    }


    //Dropped coin in hat success
    public void StartStep6()
    {
        if (currTutorial == TutorialState.Step5)
        {
            SoundManager.instance.PlayConfirm();

            currTutorial = TutorialState.Step6;
            StartCoroutine(ChangeTargetInfoText("Red Button"));
            StartCoroutine(ChangeCoinInfoText("Hit the Red Button!"));
            dropZoneRed.SetActive(true);
            target1.enabled = false;
            target2.enabled = false;
            target3.enabled = false;
            ArcadeManager.instance.DestroyCoin();
        }
    }
    //tutorial complete
    public void StartStep7()
    {
        if (currTutorial == TutorialState.Step6)
        {
            SoundManager.instance.PlayConfirm();
            currTutorial = TutorialState.Step7;
            StartCoroutine(ChangeTargetInfoText("Tutorial Complete!"));
            SoundManager.instance.PlayCheer();
            StartCoroutine(FinishTutorial());
        }
    }
    //start finish sequence
    IEnumerator FinishTutorial()
    {
        dropZoneRed.SetActive(false);
        StartCoroutine(ChangeCoinInfoText("Restart in 3"));
        SoundManager.instance.PlayTick();
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ChangeCoinInfoText("2"));
        SoundManager.instance.PlayTick();
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ChangeCoinInfoText("1"));
        SoundManager.instance.PlayTick();
        yield return new WaitForSeconds(1.5f);
        SoundManager.instance.PlaySuccess();
        EndTutorial();
    }
    //reset the area and go back to beginning
    public void EndTutorial()
    {
        currTutorial = TutorialState.None;
        EmptyCoinInfo();
        EmptyTargetInfo();
        tutorialPlayArea.SetActive(false);
        tutorialButton.SetActive(true);
        arcadeButton.SetActive(true);
    }

    //info text area
    IEnumerator ChangeCoinInfoText(string str)
    {
        Debug.Log("Changing Coin Info: " + str);
        coinInfoAnim.SetTrigger("TextClose");
        yield return new WaitForSeconds(1f);
        coinInfoText.text = str;
        coinInfoAnim.SetTrigger("TextOpen");

        Debug.Log("Coin Change done");

    }

    private void EmptyCoinInfo()
    {
        coinInfoText.text = "";
    }

    private void EmptyTargetInfo()
    {
        targetInfoText.text = "";
    }

    IEnumerator ChangeTargetInfoText(string str)
    {
        Debug.Log("Changing Target Info: " + str);

        targetInfoAnim.SetTrigger("TextClose");
        yield return new WaitForSeconds(1f);
        targetInfoText.text = str;
        targetInfoAnim.SetTrigger("TextOpen");

        Debug.Log("Target change done");
    }

    private void CloseTargetInfo()
    {
        targetInfoText.text = "";
        targetInfoAnim.SetTrigger("TextClose");
    }
}
