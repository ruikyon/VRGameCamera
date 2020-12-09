using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGameCamera : MonoBehaviour
{
    public enum CameraPosition 
    {
        Back,
        Front,
        Hand,
    }

    [Serializable]
    public class Range
    {
        public float min, max;

        public float getValue(float rate)
        {
            if (rate < 0 || rate > 1)
            {
                Debug.LogError("invalid rate: " + rate);
            }

            return min + (max - min) * rate;
        }
    }

    public static VRGameCamera Instance { get; private set; }

    [SerializeField] private Vector3 backPosition;
    [SerializeField] private Vector3 frontPosition;
    private Vector3 cameraBasePos;

    [SerializeField] private Transform target;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Range heightRange, degRange, distRange;
    public float heightOffset, degree, distance;
    private Camera cameraEntity;
    private CameraPosition state;
    private bool isShow;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("instance already exists");
            Destroy(gameObject);
        }
        Instance = this;

        foreach(Transform child in transform)
        {
            if(child.GetComponent<Camera>() != null) 
            {
                cameraEntity = child.GetComponent<Camera>();
                break;
            }
        }

        isShow = false;
        transform.position = target.position + Vector3.up;
        SetCameraPosition(CameraPosition.Back);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isShow = !isShow;
            cameraUI.SetActive(isShow);
            cameraEntity.gameObject.SetActive(isShow);
        }

        // state: 3
        if (state == CameraPosition.Hand)
        {
            transform.position = Vector3.Lerp(transform.position, hand.position, Time.deltaTime);

            // rot
            var handDeg = hand.eulerAngles;
            //handDeg.z = 0;
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, handDeg, Time.deltaTime);
            transform.eulerAngles = hand.eulerAngles;
            return;
        }

        // TODO
        //  高速いどうについては要検討
        //  あと高さは調整したさそうな気がするので、offset足すとかする
        var to = target.position;
        to.y += heightOffset;
        var from = transform.position;
        transform.position = Vector3.Lerp(from, to, Time.deltaTime);

        // rot
        var sub = target.eulerAngles.y - transform.eulerAngles.y;
        if (Mathf.Abs(sub) > 180)
        {
            sub =(Mathf.Abs(sub) - 360) * Mathf.Sign(sub);
        }
        var deg = transform.eulerAngles.y + sub * Time.deltaTime;
        transform.eulerAngles = Vector3.up * deg;
    }

    public void NextState() 
    {
        SetCameraPosition((CameraPosition)(((int)state + 1) % 3));
    }

    public void SetCameraPosition(CameraPosition position) 
    {
        switch(position)
        {
            case CameraPosition.Back:
                cameraEntity.transform.localPosition = backPosition;
                cameraEntity.transform.LookAt(transform);
                SetTargetPosition();
                break;
            case CameraPosition.Front:
                cameraEntity.transform.localPosition = frontPosition;
                cameraEntity.transform.LookAt(transform);
                SetTargetPosition();
                break;
            case CameraPosition.Hand:
                cameraEntity.transform.localPosition = Vector3.zero;
                cameraEntity.transform.localEulerAngles = Vector3.zero;
                SetHandPosition();
                break;        
        }

        state = position;
    }

    private void SetHandPosition ()
    {
        transform.position = hand.position;
        var temp = hand.eulerAngles;
        temp.z = 0;
        transform.eulerAngles = temp;
    }

    private void SetTargetPosition ()
    {
        var tempPos = target.position;
        tempPos.y += heightOffset;
        transform.position = tempPos;

        transform.eulerAngles = Vector3.up * target.eulerAngles.y;
    }

    public void ResetPosition()
    {
        backPosition = Quaternion.Euler(degree, 0, 0) * (Vector3.back * distance);
        frontPosition = Quaternion.Euler(degree, 0, 0) * (Vector3.forward * distance);

        if (state == CameraPosition.Hand)
        {
            return;
        }

        switch (state)
        {
            case CameraPosition.Back:
                cameraEntity.transform.localPosition = backPosition;
                cameraEntity.transform.LookAt(transform);
                break;
            case CameraPosition.Front:
                cameraEntity.transform.localPosition = frontPosition;
                cameraEntity.transform.LookAt(transform);
                break;
        }

    }

    public void SetHeight(float rate)
    {
        heightOffset = heightRange.getValue(rate);
    }

    public void SetDegree(float rate)
    {
        degree = degRange.getValue(rate);
        ResetPosition();
    }

    public void SetDistance(float rate)
    {
        distance = distRange.getValue(rate);
        ResetPosition();
    }
}