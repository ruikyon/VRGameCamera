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

    [SerializeField] private Vector3 backPosition;
    [SerializeField] private Vector3 frontPosition;

    [SerializeField] private Transform target;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private float maxSpeed;
    private Camera cameraEntity;
    private CameraPosition state;
    private bool isShow;

    private void Awake()
    {
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
            handDeg.z = 0;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, handDeg, Time.deltaTime);

            return;
        }

        // TODO
        //  以下は仮で、高速に移動した場合のことも検討した方がよさそう
        //  あと高さは調整したさそうな気がするので、offset足すとかする
        //transform.position = new Vector3(target.position.x, 1, target.position.z);
        var to = target.position;
        to.y = 1;
        var from = transform.position;
        from.y = 1;
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
        tempPos.y = 1;
        transform.position = tempPos;

        transform.eulerAngles = Vector3.up * target.eulerAngles.y;
    }

    public void ChangeDistance (float value) 
    {
        switch (state)
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
                break;
        }
    }

    public void ChangeDegree (float value)
    {
        switch (state)
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
                break;
        }
    }
}
