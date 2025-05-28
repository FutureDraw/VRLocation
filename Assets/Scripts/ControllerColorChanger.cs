using UnityEngine;

public class ControllerColorChanger : MonoBehaviour
{
    [Header("Названия объектов контроллеров")]
    public string leftControllerName = "XR Controller Left(Clone)";
    public string rightControllerName = "XR Controller Right(Clone)";

    [Header("Материалы")]
    public Material[] materials;

    private int leftIndex = 0;
    private int rightIndex = 0;

    private Renderer[] leftRenderers;
    private Renderer[] rightRenderers;

    void Start()
    {
        GameObject left = GameObject.Find(leftControllerName);
        GameObject right = GameObject.Find(rightControllerName);

        if (left != null)
            leftRenderers = left.GetComponentsInChildren<Renderer>();
        else
            Debug.LogWarning("Левый контроллер не найден");

        if (right != null)
            rightRenderers = right.GetComponentsInChildren<Renderer>();
        else
            Debug.LogWarning("Правый контроллер не найден");
    }

    public void NextLeftColor()
    {
        if (materials.Length == 0 || leftRenderers == null) return;

        leftIndex = (leftIndex + 1) % materials.Length;
        foreach (var r in leftRenderers)
            r.material = materials[leftIndex];
    }

    public void NextRightColor()
    {
        if (materials.Length == 0 || rightRenderers == null) return;

        rightIndex = (rightIndex + 1) % materials.Length;
        foreach (var r in rightRenderers)
            r.material = materials[rightIndex];
    }

    public void NextBothColor()
    {
        NextLeftColor();
        NextRightColor();
    }
}
