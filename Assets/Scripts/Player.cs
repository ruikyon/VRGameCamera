using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform head;
    private float heightOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHeightOffset();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(x, 0, y);
        
        if (heightOffset < head.position.y)
        {
            var tmp = transform.position;
            tmp.y = head.position.y - heightOffset;
            transform.position = tmp;
        } 
    }

    private void SetHeightOffset ()
    {
        heightOffset = head.position.y;
    }
}
