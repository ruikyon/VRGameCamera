using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerTest : MonoBehaviour
{
    [SerializeField] SteamVR_LaserPointer pointer;
    private Slider current = null;

    private void Awake()
    {
        pointer.PointerClick += OnClick;
        pointer.PointerIn += OnEnter;
        pointer.PointerOut += OnExit;
    }

    private void OnClick(object sender, PointerEventArgs e)
    {
        BaseEventData data = new BaseEventData(EventSystem.current);


        if (e.target.GetComponent<Toggle>() != null)
        {
            e.target.GetComponent<Toggle>().OnSubmit(data);
        }
    }

    private void OnEnter(object sender, PointerEventArgs e)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        Debug.Log(e.target.GetComponent<Slider>());

        if (e.target.GetComponent<Toggle>() != null)
        {
            e.target.GetComponent<Toggle>().OnPointerEnter(data);
        }

        if (e.target.GetComponent<Slider>() != null)
        {
            current = e.target.GetComponent<Slider>();
        }
    }
    private void OnExit(object sender, PointerEventArgs e)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);


        if (e.target.GetComponent<Toggle>() != null)
        {
            e.target.GetComponent<Toggle>().OnPointerExit(data);
        }

        if (e.target.GetComponent<Slider>() != null)
        {
            current = null;
        }
    }

    public void ChangeValue ()
    {
        Debug.Log(current == null);
        if (current == null)
        {
            return;
        }

        current.value += 0.01f;
    }
}