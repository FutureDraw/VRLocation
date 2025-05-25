using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class colorChanger : MonoBehaviour
{
    [Header("Список источников света")]
    public Light[] roomLights;

    [Header("Режим цвета")]
    public bool useRandomColor = true;
    public Color chosenColor = Color.cyan;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (roomLights == null || roomLights.Length == 0) return;

        Color newColor = useRandomColor
            ? Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.6f, 1f)
            : chosenColor;

        foreach (Light light in roomLights)
        {
            if (light != null)
            {
                light.color = newColor;
            }
        }

        Debug.Log($"[LightColorChangerMulti] Цвет освещения изменён на: {newColor}");
    }
}
