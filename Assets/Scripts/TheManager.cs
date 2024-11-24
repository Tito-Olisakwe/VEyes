using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class ImageTargetContent
{
    public string targetName;
    public string displayText;
}

public class TheManager : MonoBehaviour
{
    public Canvas dynamicCanvas;
    public TextMeshProUGUI infoTextTMP;
    public Button refreshButton;
    public List<ImageTargetContent> targetContentList;

    private Dictionary<string, ImageTargetContent> contentDictionary;
    private List<ImageTargetBehaviour> imageTargets;

    private void Start()
    {
        contentDictionary = new Dictionary<string, ImageTargetContent>();
        foreach (var content in targetContentList)
        {
            contentDictionary[content.targetName] = content;
        }

        imageTargets = new List<ImageTargetBehaviour>(FindObjectsOfType<ImageTargetBehaviour>());
        foreach (var target in imageTargets)
        {
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshScanning);
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour target, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED)
        {
            UpdateContentForTarget(target.TargetName);
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            if (dynamicCanvas != null)
            {
                dynamicCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateContentForTarget(string targetName)
    {
        if (contentDictionary.TryGetValue(targetName, out ImageTargetContent content))
        {
            if (dynamicCanvas != null)
            {
                dynamicCanvas.gameObject.SetActive(true);
            }

            if (infoTextTMP != null)
            {
                infoTextTMP.text = content.displayText;
            }
        }
    }

    private void RefreshScanning()
    {
        foreach (var target in imageTargets)
        {
            target.enabled = false;
        }

        Invoke(nameof(EnableTargets), 0.5f);

        if (dynamicCanvas != null)
        {
            dynamicCanvas.gameObject.SetActive(false);
        }

        if (infoTextTMP != null)
        {
            infoTextTMP.text = string.Empty;
        }
    }

    private void EnableTargets()
    {
        foreach (var target in imageTargets)
        {
            target.enabled = true;
        }
    }

    private void OnDestroy()
    {
        foreach (var target in imageTargets)
        {
            target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
