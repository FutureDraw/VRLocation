using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public Vector3 customGravity = new Vector3(0, 20f, 0);

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        // Исключаем игрока по тегу
        if (rb != null && !other.CompareTag("Player"))
        {
            rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }
}
