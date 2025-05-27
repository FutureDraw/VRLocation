using UnityEngine;

public class NegativeMass : MonoBehaviour
{
    public float pseudoMass = -1f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb == null || gameObject.CompareTag("Player")) return;

        Vector3 gravity = Physics.gravity;
        rb.AddForce(-gravity * pseudoMass, ForceMode.Acceleration);
    }
}
