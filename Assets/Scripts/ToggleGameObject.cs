using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private GameObject LinksPanel;

    public void ToggleInfoPanel()
    {
        if (InfoPanel != null)
        {
            InfoPanel.SetActive(!InfoPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Info Panel is not assigned.");
        }
    }


    public void ToggleLinksPanel()
    {
        if (LinksPanel != null)
        {
            LinksPanel.SetActive(!LinksPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("Links Panel is not assigned.");
        }
    }
}