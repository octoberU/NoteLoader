using NoteLoader.UI;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    Dictionary<string, Panel> childPanels = new Dictionary<string, Panel>();
    void Awake()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Panel childPanel = transform.GetChild(i).GetComponent<Panel>();
            if (childPanel != null)
            {
                childPanels.Add(childPanel.name, childPanel);
            }
        }

    }

    /// <summary>
    /// Opens a child panel.
    /// </summary>
    /// /// <seealso cref="Panel"/>
    /// <param name="panelName">GameObject Name of the panel</param>
    public void GoToPanel(string panelName)
    {
        if (childPanels.ContainsKey(panelName))
        {
            childPanels[panelName].ShowPanel();
            foreach (var panel in childPanels)
            {
                if (panel.Key != panelName) panel.Value.HidePanel();
            }
        }

    }

    public void QuitNoteLoader()
    {
        Application.Quit();
    }
}
