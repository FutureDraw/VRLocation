using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление параметрами появляющегося объека через UI
/// </summary>
public class ObjectManagement : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject objectPrefab;             // Префаб для создания
    public Transform spawnPoint;                // Место, где появится объект
    private GameObject targetObject;            // Активный объект
    private Rigidbody rb;

    [Header("UI Elements")]
    public Slider sizeSlider;
    public Dropdown colorDropdown;
    public Toggle gravityToggle;
    public Button createButton;

    void Start()
    {
        sizeSlider.onValueChanged.AddListener(UpdateSize);
        colorDropdown.onValueChanged.AddListener(UpdateColor);
        gravityToggle.onValueChanged.AddListener(UpdateGravity);
        createButton.onClick.AddListener(CreateObject);
    }

    void CreateObject()
    {
        if (targetObject != null)
            Destroy(targetObject);

        targetObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
        rb = targetObject.GetComponent<Rigidbody>();

        if (rb != null)
            rb.useGravity = false;

        UpdateSize(sizeSlider.value);
        UpdateColor(colorDropdown.value);
        UpdateGravity(gravityToggle.isOn);
    }

    void UpdateSize(float value)
    {
        if (targetObject != null)
            targetObject.transform.localScale = Vector3.one * value;
    }

    void UpdateColor(int index)
    {
        if (targetObject != null)
        {
            Color[] colors = { Color.red, Color.green, Color.blue, Color.white };
            if (index < colors.Length)
                targetObject.GetComponent<Renderer>().material.color = colors[index];
        }
    }

    void UpdateGravity(bool useGravity)
    {
        if (rb != null)
        {
            rb.useGravity = useGravity;

            if (!useGravity)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
