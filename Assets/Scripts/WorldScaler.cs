using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleChanger : MonoBehaviour
{
    public enum ScaleType { Micro, Normal }

    [Header("Тип масштаба")]
    public ScaleType scaleType = ScaleType.Micro;

    [Header("Целевой объект для масштабирования (например, XR Origin)")]
    public Transform targetToScale;

    [Header("Камера игрока (XR Camera внутри XR Origin)")]
    public Transform playerCamera;

    [Header("Размеры")]
    public Vector3 microScale = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 normalScale = Vector3.one;

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
        if (targetToScale == null || playerCamera == null) return;

        // Сохраняем мировую позицию головы
        Vector3 cameraWorldPosBefore = playerCamera.position;

        // Меняем масштаб
        switch (scaleType)
        {
            case ScaleType.Micro:
                targetToScale.localScale = microScale;
                break;
            case ScaleType.Normal:
                targetToScale.localScale = normalScale;
                break;
        }

        // Вычисляем смещение после масштабирования
        Vector3 cameraWorldPosAfter = playerCamera.position;
        Vector3 offset = cameraWorldPosBefore - cameraWorldPosAfter;

        // Сдвигаем XR Origin на это смещение
        targetToScale.position += offset;

        Debug.Log($"[ScaleChanger] Масштаб изменён на {targetToScale.localScale}, позиция скорректирована.");

        Destroy(gameObject);
    }
}
