using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform pointer;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Toggle setting, angle;
    [SerializeField] private Toggle back, front, hand;
    [SerializeField] private Slider height, distance, degree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        pointer.position += (new Vector3(x, y, 0)) * 0.01f;
        return;

        BaseEventData data = new BaseEventData(eventSystem);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            setting.OnSubmit(data);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            angle.OnSubmit(data);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            back.OnSubmit(data);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            front.OnSubmit(data);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            hand.OnSubmit(data);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                height.value -= 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                height.value += 0.1f;
            }
        }
        if (Input.GetKey(KeyCode.X))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                degree.value -= 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                degree.value += 0.1f;
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                distance.value -= 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                distance.value += 0.1f;
            }
        }
    }

    public void Logger()
    {
        Debug.Log("clicked");
    }
}
