using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartEvents());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartEvents()
    {
        
        yield return new WaitForSeconds(3f);

        TutorialManager.instance.StartStep1();
        Debug.Log("Start Ch 1");
        yield return new WaitForSeconds(3f);

        TutorialManager.instance.HitTarget1Success();

        Debug.Log("Hit 1 success");
        yield return new WaitForSeconds(5f);

        //TutorialManager.instance.StartStep2();

        Debug.Log("Start Ch 2");
        yield return new WaitForSeconds(5f);

        Debug.Log("Hit 2 success");

        TutorialManager.instance.HitTarget2Success();
        yield return new WaitForSeconds(5f);

        //TutorialManager.instance.StartStep2();

        Debug.Log("Start Ch 2");
        yield return new WaitForSeconds(5f);

        Debug.Log("Hit 2 success");

        TutorialManager.instance.HitTarget2Success();

        yield return new WaitForSeconds(5f);

        //TutorialManager.instance.StartStep2();

        Debug.Log("Start step 4");
        TutorialManager.instance.StartStep4();

        yield return new WaitForSeconds(5f);

        Debug.Log("Start 5");

        TutorialManager.instance.StartStep5();

        yield return new WaitForSeconds(5f);

        //TutorialManager.instance.StartStep2();

        Debug.Log("Hit target 3 success");
        TutorialManager.instance.HitTarget3Success();

        yield return new WaitForSeconds(5f);

        Debug.Log("Start 6");

        TutorialManager.instance.StartStep6();

        yield return new WaitForSeconds(5f);

        Debug.Log("Start 7");

        TutorialManager.instance.StartStep7();



    }
}
