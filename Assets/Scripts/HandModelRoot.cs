using UnityEngine;

public class HandSwitcher : MonoBehaviour
{
    public GameObject[] handPrefabs; // сюда добавим разные руки
    public Transform handHolder;     // это HandModelRoot

    private GameObject currentHand;

    public void SwitchToHand(int index)
    {
        if (currentHand != null)
        {
            Destroy(currentHand);
        }

        currentHand = Instantiate(handPrefabs[index], handHolder);
        currentHand.transform.localPosition = Vector3.zero;
        currentHand.transform.localRotation = Quaternion.identity;
    }
}
