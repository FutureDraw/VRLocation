using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(XRGrabInteractable))]
public class RandomGrabber : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        ApplyRandomProperties();
    }

    void ApplyRandomProperties()
    {
        // Случайная масса
        rb.mass = Random.Range(0.1f, 50f);

        // Случайные параметры материала
        PhysicMaterial mat = new PhysicMaterial("RandomMat")
        {
            dynamicFriction = Random.Range(0f, 1f),
            staticFriction = Random.Range(0f, 1f),
            bounciness = Random.Range(0f, 1f),
            frictionCombine = PhysicMaterialCombine.Average,
            bounceCombine = PhysicMaterialCombine.Maximum
        };

        col.material = mat;

        // (Необязательно) отладочный вывод в консоль
        Debug.Log($"[RandomProperties] mass={rb.mass}, dynFric={mat.dynamicFriction}, bounce={mat.bounciness}");
    }
}
