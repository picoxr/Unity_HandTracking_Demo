using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public GameObject sparkParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("TargetNumber"))
        {
            //hit a target, spawn a coin
            SoundManager.instance.PlayTargetHit();
            GameObject newSpark = Instantiate(sparkParticle, transform.position, sparkParticle.transform.rotation);
            TargetCtrl tCtrl = collision.gameObject.GetComponent<TargetCtrl>();
            tCtrl.SetUpCoinWithManager();
        }

        Destroy(gameObject);
    }

}
