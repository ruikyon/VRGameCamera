using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private LineRenderer pointer;
    [SerializeField] Transform targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        pointer = GetComponent<LineRenderer>();
        if (!pointer)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var end = transform.position + transform.forward * 1;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
        {
            end = hit.point;
            Debug.Log("hit: " + hit.collider.gameObject.name);
        }

        Vector3[] positions = { transform.position, end };
        pointer.SetPositions(positions);
        targetPoint.position = end;
    }
}
