using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Загрузка сцен
/// </summary>
public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("VolumeScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("ObjectManagement", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("CustomizationScene", LoadSceneMode.Additive);
    }
}