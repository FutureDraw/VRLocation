using Unity.XR.CoreUtils;
using UnityEngine;

public class XROriginSingleton : MonoBehaviour
{
    public static XROrigin Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = GetComponent<XROrigin>();
        DontDestroyOnLoad(gameObject);
    }
}
