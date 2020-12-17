using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VR;

public class VRInputModule : BaseInputModule
{
    public GameObject controllerObject;

    private GameObject pointerResource;
    private GameObject pointer;
    private LineRenderer lineRenderer;
    private Material material;

    public override void Process()
    {
        SendInputEvent(GetVRPointerEventData());
    }

    protected virtual PointerEventData GetVRPointerEventData()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);

        pointerData.Reset();
        var oldPos = pointerData.position;
        pointerData.position = GetVRPointerPosition();
        pointerData.delta = pointerData.position - pointerData.position;
        pointerData.scrollDelta = Vector3.zero;
        eventSystem.RaycastAll(pointerData, m_RaycastResultCache);
        var raycast = FindFirstRaycast(m_RaycastResultCache);
        pointerData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();
        return pointerData;
    }

    private Vector2 GetVRPointerPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            pointer.transform.position = hit.point;
            return Camera.main.WorldToScreenPoint(hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            if (pointer != null)
            {
                Destroy(pointer);
            }
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Send event to seleced object
    /// </summary>
    /// <returns></returns>
    private bool SendInputEvent(PointerEventData _pointerData)
    {
        if (_pointerData.pointerCurrentRaycast.gameObject == null)
        {
            return false;
        }
        BaseEventData data = GetBaseEventData();
        var inputable = inputAdapter.GetInputable();
        inputable.SetController(controllerObject);
        var hitObj = _pointerData.pointerCurrentRaycast.gameObject;
        if (inputable.IsSubmit(hitObj))
        {
            ExecuteEvents.Execute(hitObj, data, ExecuteEvents.submitHandler);
        }
        if (inputable.IsHold(_pointerData.selectedObject))
        {
            ExecuteEvents.Execute(hitObj, data, ExecuteEvents.dragHandler);
        }
        return data.used;
    }
}