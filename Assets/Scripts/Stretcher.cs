using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Stretcher : MonoBehaviour
{
    public XRBaseInteractor leftHand;
    public XRBaseInteractor rightHand;
    public TwoHandGrabbable targetObject;
    public float scaleMultiplier = 1.0f;

    private float initialDistance;
    private Vector3 initialScale;
    private bool isScaling = false;

    void Update()
    {
        bool leftHolding = targetObject.IsGrabbedBy(leftHand);
        bool rightHolding = targetObject.IsGrabbedBy(rightHand);

        if (leftHolding && rightHolding)
        {
            if (!isScaling)
            {
                initialDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
                initialScale = targetObject.transform.localScale;
                isScaling = true;
            }
            else
            {
                float currentDistance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
                float scaleFactor = currentDistance / initialDistance;
                targetObject.transform.localScale = initialScale * scaleFactor * scaleMultiplier;
            }
        }
        else
        {
            isScaling = false;
        }
    }
}
