using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSceneTeleportTrigger : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    public string targetSceneName;

    [Header("Имя точки спауна в новой сцене")]
    public string targetSpawnPointName;

    [Header("Кулдаун на перемещение (в секундах)")]
    public float cooldownTime = 1f;

    private bool isTransitioning = false;
    private float lastTeleportTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;
        if (Time.time < lastTeleportTime + cooldownTime) return;

        if (other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null)
        {
            StartCoroutine(TransitionToScene());
        }
    }

    private System.Collections.IEnumerator TransitionToScene()
    {
        isTransitioning = true;
        lastTeleportTime = Time.time;

        if (!IsSceneAlreadyLoaded(targetSceneName))
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone) yield return null;
        }

        var xrOrigin = XROriginSingleton.Instance;
        if (xrOrigin == null)
        {
            Debug.LogError("XROriginSingleton not found.");
            yield break;
        }

        Scene newScene = SceneManager.GetSceneByName(targetSceneName);
        GameObject[] rootObjects = newScene.GetRootGameObjects();

        Transform spawnPoint = null;
        foreach (var obj in rootObjects)
        {
            if (obj.name == targetSpawnPointName)
            {
                spawnPoint = obj.transform;
                break;
            }
        }

        if (spawnPoint != null)
        {
            xrOrigin.transform.position = spawnPoint.position;
            xrOrigin.transform.rotation = spawnPoint.rotation;
            SceneManager.MoveGameObjectToScene(xrOrigin.gameObject, newScene);
            SceneManager.SetActiveScene(newScene);
            UpdateLighting();
            RemoveDuplicateXROrigins();
            ForceReactivateXR(xrOrigin);
        }

        isTransitioning = false;
    }

    private bool IsSceneAlreadyLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
                return true;
        }
        return false;
    }

    private void RemoveDuplicateXROrigins()
    {
        var allOrigins = FindObjectsOfType<Unity.XR.CoreUtils.XROrigin>();
        foreach (var origin in allOrigins)
        {
            if (origin != XROriginSingleton.Instance)
                Destroy(origin.gameObject);
        }

        var controllers = GameObject.FindGameObjectsWithTag("XRController");
        foreach (var ctrl in controllers)
        {
            if (!ctrl.transform.IsChildOf(XROriginSingleton.Instance.transform))
                Destroy(ctrl);
        }
    }

    private void UpdateLighting()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        DynamicGI.UpdateEnvironment();
    }

    private void ForceReactivateXR(Unity.XR.CoreUtils.XROrigin origin)
    {
        var interactors = origin.GetComponentsInChildren<XRBaseInteractor>(true);
        foreach (var interactor in interactors)
        {
            interactor.enabled = false;
            interactor.enabled = true;
        }

        var providers = origin.GetComponentsInChildren<TeleportationProvider>(true);
        foreach (var provider in providers)
        {
            provider.enabled = false;
            provider.enabled = true;
        }

        var areas = GameObject.FindObjectsOfType<TeleportationArea>(true);
        foreach (var area in areas)
        {
            area.enabled = false;
            area.enabled = true;
        }

        var anchors = GameObject.FindObjectsOfType<TeleportationAnchor>(true);
        foreach (var anchor in anchors)
        {
            anchor.enabled = false;
            anchor.enabled = true;
        }
    }
}
