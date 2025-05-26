using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSceneTeleportTrigger : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    public string targetSceneName;

    [Header("Имя точки спауна в новой сцене")]
    public string targetSpawnPointName;

    private bool isTransitioning = false;

    public GameObject player;

    public void Update()
    {
        if (isTransitioning) player.SetActive(false);
        else player.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;

        // Проверим, содержит ли объект компонент XROrigin (это XR игрок)
        var origin = other.GetComponentInParent<XROrigin>();
        if (origin != null)
        {
            StartCoroutine(TransitionToScene(origin));
        }
    }

    private System.Collections.IEnumerator TransitionToScene(XROrigin xrOrigin)
    {
        isTransitioning = true;
        // Загружаем новую сцену
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        // Находим сцену и точку спауна
        Scene newScene = SceneManager.GetSceneByName(targetSceneName);
        GameObject[] rootObjects = newScene.GetRootGameObjects();

        Transform spawnTransform = null;
        foreach (var obj in rootObjects)
        {
            if (obj.name == targetSpawnPointName)
            {
                spawnTransform = obj.transform;
                break;
            }
        }

        if (spawnTransform != null)
        {
            // Перемещаем XR Origin (весь риг)
            xrOrigin.transform.position = spawnTransform.position;
            xrOrigin.transform.rotation = spawnTransform.rotation;
        }

        // Выгружаем старую сцену (где находится XR Origin)
        Scene currentScene = xrOrigin.gameObject.scene;
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        while (!unloadOp.isDone)
            yield return null;

        // Устанавливаем новую сцену активной
        SceneManager.SetActiveScene(newScene);
        isTransitioning = false;
    }

    private void ForceReactivateXR(XROrigin xrOrigin)
    {
        // Реактивация интеракторов
        var interactors = xrOrigin.GetComponentsInChildren<XRBaseInteractor>(true);
        foreach (var interactor in interactors)
        {
            interactor.enabled = false;
            interactor.enabled = true;
        }

        // Реактивация телепортации
        var teleportationProviders = xrOrigin.GetComponentsInChildren<TeleportationProvider>(true);
        foreach (var provider in teleportationProviders)
        {
            provider.enabled = false;
            provider.enabled = true;
        }

        var teleportAreas = GameObject.FindObjectsOfType<TeleportationArea>(true);
        foreach (var area in teleportAreas)
        {
            area.enabled = false;
            area.enabled = true;
        }

        var teleportAnchors = GameObject.FindObjectsOfType<TeleportationAnchor>(true);
        foreach (var anchor in teleportAnchors)
        {
            anchor.enabled = false;
            anchor.enabled = true;
        }
    }

}
