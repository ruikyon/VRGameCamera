﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace VRGC
{
    public class RadioButton : MonoBehaviour
    {
        private string _value;
        public string Value
        {
            get { return _value; }
            private set
            {
                if (value != null && Array.IndexOf(Labels, value) == -1)
                {
                    Debug.LogError("invalid value (RadioButton): " + value);
                    return;
                }

                _value = value;
                onChange?.Invoke(_value);
            }
        }
        public string[] Labels { get; private set; }
        public Action<string> onChange;

        private Dictionary<string, Toggle> toggles;
        private bool nullable;

        [SerializeField] private Toggle defaultValue = default;

        private void Awake()
        {
            var labelList = new List<string>();
            toggles = new Dictionary<string, Toggle>();
            nullable = defaultValue == null;

            foreach (Transform child in transform)
            {
                var tmp = child.GetComponent<Toggle>();
                if (tmp != null)
                {
                    labelList.Add(tmp.name);
                    toggles.Add(tmp.name, tmp);
                    tmp.onValueChanged.AddListener(ChangeValue(tmp.name));
                }
            }

            Labels = labelList.ToArray();

            if (!nullable)
            {
                defaultValue.isOn = true;
            }
            else
            {
                Value = null;
            }
        }

        private UnityAction<bool> ChangeValue(string label)
        {
            return value =>
            {
                if (!value && !nullable)
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
}