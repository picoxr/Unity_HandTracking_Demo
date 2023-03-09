using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClickRayCtrl : MonoBehaviour
{
    [SerializeField]
    PXR_Hand pxrHand;
    [SerializeField]
    TextMeshProUGUI textMesh;
    [SerializeField]
    GameObject textObj;

    private void Start()
    {
        pxrHand = GetComponent<PXR_Hand>();
    }

    private void Update()
    {
        if (pxrHand.RayValid)
        {
            textObj.SetActive(true);
            textMesh.text = pxrHand.TouchStrengthRay.ToString();
        }
        else
            textObj.SetActive(false);
    }
}
