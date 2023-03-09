using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HandClickCtrl : MonoBehaviour
{
    public HandType handType;
    public PXR_Hand pxrHand;
    public GameObject mPointPose;
    public GameObject bulletPrefab;
    [Range(.7f, 1f)]
    public float clickDownRayStrength;
    [Range(0f, .6f)]
    public float clickUpRayStrength;
    public float bulletForce = 5f;
    public float fireDelay = 1f;
    public LayerMask shootZoneLayer;
    public LineRenderer laserLine;
    public float shootRange = 30f;
    
    private bool mHitResult;
    private RaycastHit mCurHitInfo;
    private Material rayMat;

    private bool canFire = true;
    private bool reload = true;
    private bool fireZone = true;
    private float timeToShoot = 0f;

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

    private void Awake()
    {
        rayMat = mPointPose.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFireDelay();
        UpdateRaycast();
        UpdateHandTracking();
    }

    

    //We want to control where and when the user can shoot, add colors changes to the ray pointer for valid and invalid areas
    //add a laser to help players aim when in valid area
    private void UpdateRaycast()
    {
        mHitResult = Physics.Raycast(this.mPointPose.transform.position, this.mPointPose.transform.up, out mCurHitInfo, shootRange, shootZoneLayer);

        if (mHitResult)
        {
            if (mCurHitInfo.collider.CompareTag("ZoneShoot"))
            {
                //we hit something
                if (!fireZone)
                {
                    fireZone = true;

                    rayMat.SetColor("_FresnelColor", Color.cyan);
                    rayMat.SetFloat("_AlphaIntensity", .2f);
                    rayMat.SetFloat("_FresnelPower", .9f);
                }

                if (fireZone && !ArcadeManager.instance.CoinGrabbed)
                {
                    if (!laserLine.enabled)
                        laserLine.enabled = true;

                    laserLine.SetPosition(0, mPointPose.transform.position);
                    laserLine.SetPosition(1, mCurHitInfo.point);
                }
            }
        }
        else
        {
            if (fireZone)
            {
                fireZone = false;

                rayMat.SetColor("_FresnelColor", Color.red);
                rayMat.SetFloat("_AlphaIntensity", .1f);
                rayMat.SetFloat("_FresnelPower", .2f);
                laserLine.enabled = false;
            }
        }
    }

    
    private void UpdateHandTracking()
    {
        
        if (PXR_HandTracking.GetActiveInputDevice() != ActiveInputDevice.HandTrackingActive)
            return;

        //Ray pointer is valid when it appears on the users hand
        if (pxrHand.RayValid)
        {
            //Raytouched is true when TouchStrengthRay >= .7, otherwise it is false
            if (pxrHand.RayTouched && pxrHand.TouchStrengthRay >= clickDownRayStrength)
            {
                if (this.mCurClickState == RayClickState.None)
                {
                    this.mCurClickState = RayClickState.Start;
                    OnRayClickDown();
                }
            }
            else if(!pxrHand.RayTouched && pxrHand.TouchStrengthRay <= clickUpRayStrength)
            {
                if (this.mCurClickState == RayClickState.Clicking)
                {
                    this.mCurClickState = RayClickState.End;
                    OnRayClickUp();
                }
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

    }
    private void OnRayClickUp()
    {
        //Triggers when TouchStrengthRay <= clickUpRayStrength;
        this.mCurClickState = RayClickState.None;
        if (reload == false)
        {
            reload = true;
            SoundManager.instance.PlayReload();
        }
    }

    //User must have reloaded, be pointing at the firezone area and have waited past his delay time
    private void OnRayClickDown()
    {
        //Triggers when TouchStrengthRay >= clickDownRayStrength;
        this.mCurClickState = RayClickState.Clicking;
        if (!ArcadeManager.instance.CoinGrabbed)
        {
            //when all conditions are met, let the user fire
            if (canFire && reload && fireZone)
            {
                ClickFire();
                canFire = false;
                reload = false;
                laserLine.startColor = Color.red;

            }
        }
    }
    
    //Fire from the raypointer
 
    private void ClickFire()
    {
        if (bulletPrefab == null)
            return;
        GameObject newBullet = Instantiate(bulletPrefab, mPointPose.transform.position, Quaternion.identity);
        newBullet.transform.forward = mPointPose.transform.up;
        
        Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(mPointPose.transform.up * bulletForce, ForceMode.Impulse);
        SoundManager.instance.PlayShootBullet();
    }

    //Delay the user from firing too fast
    private void UpdateFireDelay()
    {
        if (canFire == false)
        {
            timeToShoot += Time.deltaTime;
            if (timeToShoot >= fireDelay)
            {
                timeToShoot = 0;
                canFire = true;
                laserLine.startColor = Color.cyan;
            }
        }
    }
}
