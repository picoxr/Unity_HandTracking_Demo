using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get => _instance;
        private set
        {
            if (_instance != null)
                Debug.LogWarning("Second attempt to get SoundManager");
            _instance = value;
        }
    }

    [Header("Audiclips for interactions")]
    public AudioClip audiConfirm;
    public AudioClip audiTick;
    public AudioClip audiNoti;
    public AudioClip audiSuccess;
    public AudioClip audiFail;
    public AudioClip audiCheer;
    public AudioClip audiSelect;
    public AudioClip audiCoin;
    public AudioClip audiDrop;
    public AudioClip audiButton;
    public AudioClip audiHighScore;
    public AudioClip audiTimeover;
    public AudioClip audiGo;
    public AudioClip audiReady;
    public AudioClip audiSpawn;
    public AudioClip audiTargetHit;
    public AudioClip audiShootBullet;
    public AudioClip audiReload;
    public AudioClip audi1;
    public AudioClip audi2;
    public AudioClip audi3;
    public AudioClip audiGameOver;

    private AudioSource audiS;


    private void Awake()
    {
        instance = this;
        audiS = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }


    public void PlayConfirm()
    {
        if(audiConfirm != null)
            audiS.PlayOneShot(audiConfirm);
    }

    public void PlayCheer()
    {
        if (audiCheer != null)
            audiS.PlayOneShot(audiCheer);
    }


    public void PlayTick()
    {
        if (audiTick != null)
            audiS.PlayOneShot(audiTick);
    }

    public void PlayNotification()
    {
        if (audiNoti != null)
            audiS.PlayOneShot(audiNoti);
    }
    public void PlaySuccess()
    {
        if (audiSuccess != null)
            audiS.PlayOneShot(audiSuccess);
    }
    public void PlayFail()
    {
        if (audiFail != null)
            audiS.PlayOneShot(audiFail);
    }

    public void PlaySelect()
    {
        if(audiSelect)
            audiS.PlayOneShot(audiSelect);
    }

    public void PlayCoin()
    {
        if(audiCoin)
            audiS.PlayOneShot(audiCoin);
    }

    public void PlayDrop()
    {
        if (audiDrop)
            audiS.PlayOneShot(audiDrop);
    }

    public void PlayButtonHit()
    {
        if (audiButton)
            audiS.PlayOneShot(audiButton);
    }

    public void PlayTargetHit()
    {
        if(audiTargetHit)
            audiS.PlayOneShot(audiTargetHit);
    }

    public void PlayShootBullet()
    {
        if(audiShootBullet)
            audiS.PlayOneShot(audiShootBullet);
    }

    public void PlayReload()
    {
        if(audiReload)
            audiS.PlayOneShot(audiReload);
    }

    public void PlayHighScore()
    {
        if (audiHighScore)
            audiS.PlayOneShot(audiHighScore);
    }

    public void PlayTimeover()
    {
        if (audiTimeover)
            audiS.PlayOneShot(audiTimeover);
    }

    public void PlayGo()
    {
        if (audiGo)
            audiS.PlayOneShot(audiGo);
    }

    public void PlayReady()
    {
        if (audiReady)
            audiS.PlayOneShot(audiReady);
    }

    public void PlaySpawn()
    {
        if (audiReady)
            audiS.PlayOneShot(audiSpawn);
    }

    public void Play1()
    {
        if (audi1)
            audiS.PlayOneShot(audi1);
    }

    public void Play2()
    {
        if (audi2)
            audiS.PlayOneShot(audi2);
    }

    public void Play3()
    {
        if (audi3)
            audiS.PlayOneShot(audi3);
    }

    public void PlayGameOver()
    {
        if (audiGameOver)
            audiS.PlayOneShot(audiGameOver);
    }
}
