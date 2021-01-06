using UnityEngine;

public class Pointer : MonoBehaviour
{
    public RaycastHit Target { get; private set; }
    public bool Hit { get; private set; }
    public bool Clicked { get; private set; }

    [SerializeField] private Transform targetPoint = default;
    [SerializeField] private Material normalMaterial = default, clickedMaterial = default;
    [SerializeField] private float length = 1;

    private LineRenderer pointer;

    private void Start()
    {
        pointer = GetComponent<LineRenderer>();
        if (!pointer)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        RaycastHit hit;
        var end = transform.position + transform.forward * length;
        Hit = Physics.Raycast(transform.position, transform.forward, out hit, length);
        if (Hit)
        {
            end = hit.point;
            targetPoint.position = Target.point;
        }

        Vector3[] positions = { transform.position, end };
        pointer.SetPositions(positions);

        Target = hit;
        targetPoint.gameObject.SetActive(Hit);

       //  Clicked = Input.GetButton("Fire1");

        pointer.material = Clicked ? clickedMaterial : normalMaterial;
        targetPoint.GetComponent<MeshRenderer>().material = Clicked ? clickedMaterial : normalMaterial;
    }

    public void OnButtonDown()
    {
        Clicked = true;
    }

    public void OnButtonUp()
    {
        Clicked = false;
    }
}
