using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;
using UnityEngine.Events;

public class ButtonCtrl : MonoBehaviour
{
    public enum ButtonMode { TutorialActive, ArcadeActive, TutorialStart, ArcadeStart, End}
    public ButtonMode currButtonMode;

    public float buttonDelayTime = 1f;
    public GameObject button;
    public Animator buttonAnim;
    public Animator parentAnim;

    private enum ButtonState { None, Pressed, Ready, Delay };
    private ButtonState currState;
    private float currTime = 0f;
    private GameObject interactor;

    private void Awake()
    {
        if(buttonAnim == null)
            buttonAnim= GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        currTime = 0f;
        currState = ButtonState.Delay;
        if (parentAnim == null)
            parentAnim = GetComponentInParent<Animator>();

    }

    private void Update()
    {
        if(currState== ButtonState.Delay) 
        {
            currTime += Time.deltaTime;
            if(currTime > buttonDelayTime)
            {
                currTime = 0f;
                currState = ButtonState.Ready;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currState == ButtonState.Ready)
        {
            interactor = other.gameObject;
            ButtonPressAction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(currState == ButtonState.Pressed && other.gameObject == interactor)
        {
            StartCoroutine(ButtonReleaseAction());
        }
    }

    private void ButtonPressAction()
    {
        SoundManager.instance.PlayButtonHit();
        buttonAnim.SetTrigger("ButtonPress");
        currState = ButtonState.Pressed;
    }

    IEnumerator ButtonReleaseAction()
    {
        currState = ButtonState.Delay;
        buttonAnim.SetTrigger("ButtonRelease");
        yield return new WaitForSeconds(.2f);
        ButtonModeAction();
    }


    public void ButtonModeAction()
    {
        switch(currButtonMode)
        {
            case (ButtonMode.TutorialStart):
                if(TutorialManager.instance.TutorialReady())
                    TutorialManager.instance.StartStep0();
                break;
            case (ButtonMode.ArcadeStart):
                if(ArcadeManager.instance.ArcadeReady())
                    ArcadeManager.instance.StartNewGame();
                break;
            case (ButtonMode.ArcadeActive):
                StartCoroutine(RedButtonSequence());
                break;
            case (ButtonMode.TutorialActive):
                TutorialManager.instance.StartStep7();
                gameObject.SetActive(false);
                break;
            case (ButtonMode.End):
                SoundManager.instance.PlaySuccess();
                ArcadeManager.instance.GameOver();
                TutorialManager.instance.EndTutorial();
                break;
        }
    }

    IEnumerator RedButtonSequence()
    {
        ArcadeManager.instance.UpdateScore();
        if (parentAnim)
            parentAnim.SetTrigger("ButtonEnd");
        yield return new WaitForSeconds(.31f);
        Destroy(gameObject);
    }
}
