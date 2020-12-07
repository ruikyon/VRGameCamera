using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadioButton : MonoBehaviour
{
    public string Value { get; private set; }
    public string[] Labels { get; private set; }

    private Dictionary<string, Toggle> toggles;
    private bool nullable;
    [SerializeField] private Toggle defaultValue;

    private void Awake()
    {
        List<string> labelList = new List<string>();
        toggles = new Dictionary<string, Toggle>();
        nullable = defaultValue == null;

        foreach (Transform child in transform)
        {
            var tmp = child.GetComponent<Toggle>();
            Debug.Log(tmp);
            if (tmp != null)
            {
                labelList.Add(tmp.name);
                toggles.Add(tmp.name, tmp);
                tmp.onValueChanged.AddListener(ChangeValue(tmp.name));
            }
        }

        if (!nullable)
        {
            defaultValue.isOn = true;
        }
        else
        {
            Value = null;
        }

        Labels = labelList.ToArray();
    }

    public UnityAction<bool> ChangeValue(string label)
    {
        return value =>
        {
            if (!value)
            {
                return;
            }

            if (Value != null)
            {
                toggles[Value].isOn = false;
            }

            if (!nullable)
            {
                if (Value != null)
                {
                    toggles[Value].interactable = true;
                }
                toggles[label].interactable = false;
            }

            Value = Value == label ? null : label;
        };
    }
}
