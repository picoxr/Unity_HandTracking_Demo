using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTipCtrl : MonoBehaviour
{
    public Transform objectSetPos;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        BlueHighlight();
    }


    public void RedHighlight()
    {
        if (rend)
        {
            rend.material.color = Color.red;
        }
    }

    public void YellowHighlight()
    {
        if (rend)
        {
            rend.material.color = Color.yellow;
        }
    }

    public void WhiteHighlight()
    {
        if (rend)
        {
            rend.material.color = Color.white;
        }
    }

    public void BlueHighlight()
    {
        if (rend)
        {
            rend.material.color = Color.blue;
        }
    }
}
