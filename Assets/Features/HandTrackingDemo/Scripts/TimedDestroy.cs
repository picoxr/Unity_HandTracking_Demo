using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyNow", time);
    }

    private void DestroyNow()
    {
        Destroy(gameObject);
    }
}
