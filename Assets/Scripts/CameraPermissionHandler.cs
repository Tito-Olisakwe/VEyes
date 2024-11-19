using UnityEngine;
using Vuforia;
using UnityEngine.Android;
using System.Collections.Generic;

public class CameraPermissionHandler : MonoBehaviour
{
    private List<GameObject> vuforiaObjects = new List<GameObject>();

    void Start()
    {
        FindVuforiaObjects();

        foreach (var obj in vuforiaObjects)
        {
            obj.SetActive(false);
        }

        CheckAndRequestCameraPermission();
    }

    void FindVuforiaObjects()
    {
        var allVuforiaBehaviours = FindObjectsOfType<VuforiaBehaviour>();
        foreach (var behaviour in allVuforiaBehaviours)
        {
            var gameObject = behaviour.gameObject;
            if (gameObject.GetComponent<ImageTargetBehaviour>() != null)
            {
                vuforiaObjects.Add(gameObject);
            }
        }

        if (vuforiaObjects.Count == 0)
        {
            Debug.LogError("No GameObjects with both VuforiaBehaviour and ImageTargetBehaviour found in the scene.");
        }
    }

    void CheckAndRequestCameraPermission()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                StartCoroutine(WaitForPermission());
            }
            else
            {
                EnableVuforiaObjects();
            }
        }
        else
        {
            EnableVuforiaObjects();
        }
    }

    System.Collections.IEnumerator WaitForPermission()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }
        EnableVuforiaObjects();
    }

    void EnableVuforiaObjects()
    {
        foreach (var obj in vuforiaObjects)
        {
            obj.SetActive(true);
        }
    }
}
