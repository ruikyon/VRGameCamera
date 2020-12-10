using System;
using UnityEngine;

namespace VRGC
{
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

            public float GetValue(float rate)
            {
                if (rate < 0 || rate > 1)
                {
                    Debug.LogError("invalid rate: " + rate);
                }

                return min + (max - min) * rate;
            }
        }

        public static VRGameCamera Instance { get; private set; }

        [SerializeField] private Transform target = default;
        [SerializeField] private Transform hand = default;

        [Space(10)]
        [SerializeField] private float maxSpeed = default;
        [SerializeField] private Range heightRange = default, degRange = default, distRange = default;

        private float heightOffset, degree, distance;
        private Camera cameraEntity;
        private CameraPosition state;
        private Vector3 backPosition;
        private Vector3 frontPosition;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("instance already exists");
                Destroy(gameObject);
            }
            Instance = this;

            foreach (Transform child in transform)
            {
                if (child.GetComponent<Camera>() != null)
                {
                    cameraEntity = child.GetComponent<Camera>();
                    break;
                }
            }

            transform.position = target.position + Vector3.up;
            SetCameraPosition(CameraPosition.Back);
        }

        private void Update()
        {
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

            // TODO：高速いどうについては要検討
            var to = target.position;
            to.y += heightOffset;
            var from = transform.position;
            transform.position = Vector3.Lerp(from, to, Time.deltaTime);

            // rot
            var sub = target.eulerAngles.y - transform.eulerAngles.y;
            if (Mathf.Abs(sub) > 180)
            {
                sub = (Mathf.Abs(sub) - 360) * Mathf.Sign(sub);
            }
            var deg = transform.eulerAngles.y + sub * Time.deltaTime;
            transform.eulerAngles = Vector3.up * deg;
        }

        public void SetCameraPosition(CameraPosition position)
        {
            switch (position)
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

        private void SetHandPosition()
        {
            transform.position = hand.position;
            var temp = hand.eulerAngles;
            temp.z = 0;
            transform.eulerAngles = temp;
        }

        private void SetTargetPosition()
        {
            var tempPos = target.position;
            tempPos.y += heightOffset;
            transform.position = tempPos;

            transform.eulerAngles = Vector3.up * target.eulerAngles.y;
        }

        public void ResetPosition()
        {
            backPosition = Quaternion.Euler(degree, 0, 0) * (Vector3.back * distance);
            frontPosition = Quaternion.Euler(-degree, 0, 0) * (Vector3.forward * distance);

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
            heightOffset = heightRange.GetValue(rate);
        }

        public void SetDegree(float rate)
        {
            degree = degRange.GetValue(rate);
            ResetPosition();
        }

        public void SetDistance(float rate)
        {
            distance = distRange.GetValue(rate);
            ResetPosition();
        }
    }
}