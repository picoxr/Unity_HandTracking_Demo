using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            if (_instance != null)
                Debug.LogWarning("Second attempt to get EventManager");
            _instance = value;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    //events for actions
    public delegate void Target1Hit();
    public event Target1Hit OnTarget1Hit;
    public delegate void Target2Hit();
    public event Target2Hit OnTarget2Hit;
    public delegate void Target3Hit();
    public event Target3Hit OnTarget3Hit;

    //Triggers that happen if any other methods are subscribed to the event
    public void TriggerOnTarget1Hit()           { OnTarget1Hit?.Invoke(); }
    public void TriggerOnTarget2Hit()           { OnTarget2Hit?.Invoke(); }
    public void TriggerOnTarget3Hit()           { OnTarget3Hit?.Invoke(); }

}
