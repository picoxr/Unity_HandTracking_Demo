using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatTrick : MonoBehaviour
{
    public enum GameMode { Tutorial, Arcade}
    public GameMode currGameMode;
    public enum HatValue { One, Two, Three }
    public HatValue hatVal;
    public int currHatVal = 0;
    public ParticleSystem hatTrickParticle;
    public Animator anim;
    public BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        SetValue();
        CheckComponents();
    }

    private void SetValue()
    {
        if (hatVal == HatValue.One)
            currHatVal = 1;
        else if (hatVal == HatValue.Two)
            currHatVal = 2;
        else
            currHatVal = 3;
    }

    private void CheckComponents()
    {
        if(!anim)
            anim = GetComponent<Animator>();
        if(!boxCollider)
            boxCollider = GetComponent<BoxCollider>();
    }

    public bool HatMatch(int coinVal)
    {
        return currHatVal == coinVal;
    }

    public void PlayTrick()
    {
        StartCoroutine(LastTrick());
    }

    IEnumerator LastTrick()
    {
        hatTrickParticle.Play();
        boxCollider.enabled= false;

        if(currGameMode == GameMode.Arcade)
        {
            //Arcade mode is active. Score points!
            ArcadeManager.instance.UpdateScore();
        }
        else
        {
            //Tutorial Mode Active.
            SoundManager.instance.PlayCoin();
        }

        yield return new WaitForSeconds(.5f);

        anim.SetTrigger("LastTrick");

        yield return new WaitForSeconds(1f);

        if (currGameMode == GameMode.Arcade)
        {
            //Arcade mode is active.
            Destroy(gameObject);

        }
        else
        {
            //Tutorial Mode Active.
            TutorialManager.instance.StartStep6();
            gameObject.SetActive(false);
        }
    }
}
