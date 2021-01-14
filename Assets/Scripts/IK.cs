using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

//Name of class must be name of file as well

public class IK : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    public Transform bodyObj = null;
    public Transform leftFootObj = null;
    public Transform rightFootObj = null;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform lookAtObj = null;

    public float leftFootWeightPosition = 1;
    public float leftFootWeightRotation = 1;

    public float rightFootWeightPosition = 1;
    public float rightFootWeightRotation = 1;

    public float leftHandWeightPosition = 1;
    public float leftHandWeightRotation = 1;

    public float rightHandWeightPosition = 1;
    public float rightHandWeightRotation = 1;

    public float lookAtWeight = 1.0f;

    private Quaternion plus = new Quaternion(0, 0, 1, 90);
    private Quaternion minus = new Quaternion(0, 0, 1, -90);


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        //ikActive = false;
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Change")) ikActive = !ikActive;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (ikActive)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeightRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeightPosition);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeightRotation);

                animator.SetLookAtWeight(lookAtWeight, 0.3f, 0.6f, 1.0f, 0.5f);

                if (bodyObj != null)
                {
                    animator.bodyPosition = bodyObj.position;
                    animator.bodyRotation = bodyObj.rotation;
                }

                if (leftFootObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
                }

                if (rightFootObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
                }

                if (leftHandObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation * plus);
                }

                if (rightHandObj != null)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation * minus);
                }

                if (lookAtObj != null)
                {
                    animator.SetLookAtPosition(lookAtObj.position);
                }
            }
            /*
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0.0f);
                if (bodyObj != null)
                {
                    bodyObj.position = animator.bodyPosition;
                    bodyObj.rotation = animator.bodyRotation;
                }
                if (leftFootObj != null)
                {
                    leftFootObj.position = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                    leftFootObj.rotation = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
                }
                if (rightFootObj != null)
                {
                    rightFootObj.position = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                    rightFootObj.rotation = animator.GetIKRotation(AvatarIKGoal.RightFoot);
                }
                if (leftHandObj != null)
                {
                    leftHandObj.position = animator.GetIKPosition(AvatarIKGoal.LeftHand);
                    leftHandObj.rotation = animator.GetIKRotation(AvatarIKGoal.LeftHand);
                }
                if (rightHandObj != null)
                {
                    rightHandObj.position = animator.GetIKPosition(AvatarIKGoal.RightHand);
                    rightHandObj.rotation = animator.GetIKRotation(AvatarIKGoal.RightHand);
                }
                if (lookAtObj != null)
                {
                    lookAtObj.position = animator.bodyPosition + animator.bodyRotation * new Vector3(0, 0.5f, 1);
                }
            }
            */
        }
    }
}