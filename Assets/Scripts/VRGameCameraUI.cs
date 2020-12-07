using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRGameCameraUI : MonoBehaviour
{
    public enum State
    {
        None,
        Setting,
        Angle,
    }

    private State currentState;
    [SerializeField] private RadioButton mode, angle;
    [SerializeField] private Slider distance, height, degree;

    // 0, 36, 75

    private void Awake()
    {
        ChangeState(State.None);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 対象を決めてそちらを向くように
    }

    public void DebugLog()
    {
        Debug.Log("clicked");
    }

    // for button click
    public void SelectState (int state)
    {
        if (Enum.IsDefined(typeof(State), state)) 
        {
            ChangeState((State)state);
        }
    }

    public void ChangeState (State state)
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Angle:
                break;
            case State.Setting:
                break;
        }

        currentState = state;
    }
}
