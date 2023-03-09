using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step0BestArea : MonoBehaviour
{

    private int thumbCount = 0;
    private bool canStartStep1 = false;
    private float time = 0;
    private float timeToStart = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        thumbCount = 0;
        canStartStep1 = false;
    }

    private void Update()
    {
        if(canStartStep1)
        {
            time += Time.deltaTime;
            if (time >= timeToStart)
                TutorialManager.instance.StartStep1();
        }
    }

    private void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, lerpPoint.position, Time.deltaTime * lerpSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lerpPoint.rotation, Time.deltaTime * lerpSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thumb"))
            thumbCount++;

        if (thumbCount >= 2 && canStartStep1 == false)
        {
            SoundManager.instance.PlayConfirm();
            canStartStep1 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Thumb"))
            thumbCount--;

        if (thumbCount < 2)
        {
            time = 0f;
            canStartStep1 = false;
        }
    }
}
