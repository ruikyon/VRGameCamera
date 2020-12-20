using UnityEngine;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{
    [SerializeField] private Pointer pointer = default;

    private PointerEventData data;
    private bool preState = false;

    public override void ActivateModule()
    {
        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        if (!pointer || !pointer.Hit)
        {
            return;
        }

        data.Reset();
        data.position = Camera.main.WorldToScreenPoint(pointer.Target.point);
        eventSystem.RaycastAll(data, m_RaycastResultCache);

        data.button = PointerEventData.InputButton.Left;
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        var hitObject = data.pointerCurrentRaycast.gameObject;
        //var oldPos = pointerData.position;
        //pointerData.position = position;
        //pointerData.delta = pointerData.position - pointerData.position;
        //pointerData.scrollDelta = Vector3.zero;

        HandlePointerExitAndEnter(data, hitObject);

        HandleClickEvents(data, hitObject);

        HandleDragEvents(data, hitObject);

        preState = pointer.Clicked;
        m_RaycastResultCache.Clear();
    }

    private void HandleClickEvents(PointerEventData currentPointerData, GameObject target)
    {
        if (!preState && pointer.Clicked)
        {
            ExecuteEvents.ExecuteHierarchy(target, GetBaseEventData(), ExecuteEvents.submitHandler);
        }
    }

    private void HandleDragEvents(PointerEventData currentPointerData, GameObject target)
    {
        // drag start
        // drag

        if (preState && pointer.Clicked)
        {
            currentPointerData.pointerPressRaycast = currentPointerData.pointerCurrentRaycast;
            var res = ExecuteEvents.ExecuteHierarchy(target, data, ExecuteEvents.dragHandler);
        }
        // drag end
    }
}