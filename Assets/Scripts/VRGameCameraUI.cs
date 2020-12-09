using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRGameCameraUI : MonoBehaviour
{
    private const string MODE_SETTING = "Setting";
    private const string MODE_ANGLE = "Angle";

    [SerializeField] private RadioButton mode, angle;
    [SerializeField] private Slider distance, height, degree;
    [SerializeField] private RectTransform additionalUI;
    [SerializeField] private GameObject settingUI, angleUI;
    private Queue<int> heightTargets;

    // 0, 36, 75

    private void Awake()
    {
        //ChangeState(State.None);
        mode.OnChange += ChangeState;
        heightTargets = new Queue<int>();

        // UIとの紐づけ
        angle.OnChange += ChangeAngle;

        height.onValueChanged.AddListener(value => { VRGameCamera.Instance.SetHeight(value); });
        degree.onValueChanged.AddListener(value => { VRGameCamera.Instance.SetDegree(value); });
        distance.onValueChanged.AddListener(value => { VRGameCamera.Instance.SetDistance(value); });
    }

    private void Start()
    {
        height.value = 0.5f;
        degree.value = 0.5f;
        distance.value = 0.5f;
    }

    private void Update()
    {
        // 対象を決めてそちらを向くように

        if (heightTargets.Count > 0)
        {
            var target = heightTargets.Peek();
            var tmp = additionalUI.sizeDelta;

            tmp.y -= (tmp.y - target) * 0.2f;
            if (Math.Abs(tmp.y - target) < 2)
            {
                tmp.y = target;
                heightTargets.Dequeue();

                if (target == 0)
                {
                    settingUI.SetActive(mode.Value == MODE_SETTING);
                    angleUI.SetActive(mode.Value == MODE_ANGLE);
                }
            }

            additionalUI.sizeDelta = tmp;
        }
    }

    public void ChangeState (string state)
    {
        switch (state)
        {
            case null:
                heightTargets.Enqueue(0);
                break;
            case MODE_ANGLE:
                heightTargets.Enqueue(0);
                heightTargets.Enqueue(36);
                break;
            case MODE_SETTING:
                heightTargets.Enqueue(0);
                heightTargets.Enqueue(75);
                break;
            default:
                break;
        }
    }

    public void ChangeAngle (string angle)
    {
        if (angle == null)
        {
            return;
        }

        try
        {
            var position = (VRGameCamera.CameraPosition)Enum.Parse(typeof(VRGameCamera.CameraPosition), angle);
            if (Enum.IsDefined(typeof(VRGameCamera.CameraPosition), angle))
            {
                VRGameCamera.Instance.SetCameraPosition(position);
            }
        }
        catch (ArgumentException)
        {
            Debug.LogError("invalid angle: " + angle);
        }
    }
}
