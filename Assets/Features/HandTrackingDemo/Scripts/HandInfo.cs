using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HandInfo : MonoBehaviour
{
    public HandType handType;
    public PXR_Hand mHand;
    public GameObject mPointPose;

    public TextMeshProUGUI rayValidtext;
    public TextMeshProUGUI rayTouchedtext;
    public TextMeshProUGUI rayTouchStrengthtext;
    public TextMeshProUGUI rayTouchStrengthtext2;


    private float time = 0f;
    private ActiveInputDevice mCurActiveInputDeviceType = ActiveInputDevice.HeadActive;
    private bool mHitResult;
    private RaycastHit mCurHitInfo;
    private RayClickState mCurClickState = RayClickState.None;
    private enum RayClickState
    {
        None,
        Start,
        Clicking,
        End,
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandTracking();

    }

    private void FixedUpdate()
    {
        time += Time.fixedTime;
    }



    private void OnRayClickDown()
    {
        this.mCurClickState = RayClickState.Clicking;

    }

    private void OnRayClickUp()
    {

        this.mCurClickState = RayClickState.None;

    }

    private void UpdateHandTracking()
    {
        mCurActiveInputDeviceType = PXR_HandTracking.GetActiveInputDevice();
        if (mCurActiveInputDeviceType != ActiveInputDevice.HandTrackingActive)
            return;


        if (this.mHand.RayValid)
        {
            if (this.mHand.RayTouched)
            {
                if (this.mCurClickState == RayClickState.None)
                {
                    this.mCurClickState = RayClickState.Start;
                    OnRayClickDown();
                }
            }
            else
            {

                if (this.mCurClickState == RayClickState.Clicking)
                {
                    this.mCurClickState = RayClickState.End;
                    OnRayClickUp();
                }
            }
            rayTouchStrengthtext2.text = $"{mHand.TouchStrengthRay.ToString()}";

        }
        else
        {

            if (this.mCurClickState == RayClickState.Clicking)
            {
                this.mCurClickState = RayClickState.End;
                OnRayClickUp();
            }

            rayTouchStrengthtext2.text = $"0";

        }

        rayValidtext.text = $"{mHand.RayValid.ToString()}";
        rayTouchedtext.text = $"{mHand.RayTouched.ToString()}";
        rayTouchStrengthtext.text = $"{mHand.TouchStrengthRay.ToString()}";


    }

}
