using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform head;
    private Transform rig;
    [SerializeField] private float heightOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rig = head.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHeightOffset();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SetHeightOffset();
        }

        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(x, 0, y);

        var pos = head.position;
        pos.y = 0;

        if (heightOffset < head.position.y)
        {
            pos.y = head.position.y - heightOffset;
        }
        transform.position = pos;
    }

    private void SetHeightOffset ()
    {
        var sub = heightOffset - head.position.y;
        var rigpos = rig.position;
        rigpos.y += sub;
        rig.position = rigpos;
    }
}
