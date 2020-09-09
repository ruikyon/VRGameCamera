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

        // TODO
        //  以下は仮で、高速に移動した場合のことも検討した方がよさそう
        //  あと高さは調整したさそうな気がするので、offset足すとかする
        //transform.position = new Vector3(target.position.x, 1, target.position.z);
        var to = target.position;
        to.y = 1;
        var from = transform.position;
        from.y = 1;
        transform.position = Vector3.Lerp(from, to, Time.deltaTime);
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
                break;
            case CameraPosition.Front:
                cameraEntity.transform.localPosition = frontPosition;
                cameraEntity.transform.LookAt(transform);
                break;
            case CameraPosition.Hand:
                Debug.Log("hand");
                break;        
        }

        state = position;
    }
}
