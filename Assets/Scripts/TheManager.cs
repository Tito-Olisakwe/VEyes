using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Define the ImageTargetContent class to hold information related to each target
[System.Serializable]
public class ImageTargetContent
{
    public string targetName;         // Name of the image target
    public string displayText;        // Text to display when the target is detected
}

public class TheManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Canvas dynamicCanvas;              // Assign your Canvas in the Inspector
    public TextMeshProUGUI infoTextTMP;       // TMP Text element for displaying text

    [Header("Content Management Table (CMT)")]
    public List<ImageTargetContent> targetContentList; // List of targets and their content

    private Dictionary<string, ImageTargetContent> contentDictionary; // Internal dictionary for quick lookup

    private void Start()
    {
        // Initialize the dictionary from the content table
        contentDictionary = new Dictionary<string, ImageTargetContent>();
        foreach (var content in targetContentList)
        {
            contentDictionary[content.targetName] = content;
        }

        // Subscribe to tracking events for all Image Targets
        var imageTargets = FindObjectsOfType<ImageTargetBehaviour>();
        foreach (var target in imageTargets)
        {
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour target, TargetStatus targetStatus)
    {
        // When a target is tracked
        if (targetStatus.Status == Status.TRACKED)
        {
            string targetName = target.TargetName;
            UpdateContentForTarget(targetName);
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            // Optionally hide the canvas when tracking is lost
            dynamicCanvas.gameObject.SetActive(false);
        }
    }

    private void UpdateContentForTarget(string targetName)
    {
        // Check if the targetName exists in the content dictionary
        if (contentDictionary.TryGetValue(targetName, out ImageTargetContent content))
        {
            // Display the content on the UI
            dynamicCanvas.gameObject.SetActive(true);

            // Update the TMP text
            infoTextTMP.text = content.displayText;

            // Update the display image (if assigned)

        }
        else
        {
            Debug.LogWarning($"No content found for target: {targetName}");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from tracking events to avoid memory leaks
        var imageTargets = FindObjectsOfType<ImageTargetBehaviour>();
        foreach (var target in imageTargets)
        {
            target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
