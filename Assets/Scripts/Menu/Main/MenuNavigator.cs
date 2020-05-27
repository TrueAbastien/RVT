using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas script used to switch over panels.
/// </summary>
public class MenuNavigator : MonoBehaviour
{
    /// <summary>
    /// Index of the first panel to be loaded.
    /// Usually meant for the Main Panel (0).
    /// </summary>
    public uint PrimaryPanelIndex = 0;

    /// <summary>
    /// Current Panel object (not the UI.Image, the Game Object).
    /// </summary>
    private GameObject CurrentPanel = null;
    /// <summary>
    /// List of all Panel objects found on Start.
    /// </summary>
    private List<GameObject> Panels;


    /// <summary>
    /// Start Function:
    /// - Search through children for Panels
    /// - Hide the non-essential Panels
    /// - Load and trigger the first panel handling
    /// </summary>
    void Start()
    {
        // Generate Panels
        Panels = new List<GameObject>();
        int nChildren = transform.childCount;
        for (int ii = 0; ii < nChildren; ++ii)
            if (transform.GetChild(ii).GetComponent<AbstractMenuPanel>() != null)
                Panels.Add(transform.GetChild(ii).gameObject);

        // Hide Panels
        foreach (GameObject pane in Panels)
            pane.SetActive(false);

        // Load First Panel
        if (Panels.Count > 0 && PrimaryPanelIndex < Panels.Count)
            FocusPanel(Panels[(int)PrimaryPanelIndex].GetComponent<AbstractMenuPanel>());
        else FocusPanel(Panels[0].GetComponent<AbstractMenuPanel>());
    }

    /// <summary>
    /// Focus on the selected Panel.
    /// Verify that the selected Panel is currently available.
    /// </summary>
    /// <param name="panel">Selected Panel object</param>
    /// <returns>Result of the verification</returns>
    private bool FocusPanelObject(GameObject panel)
    {
        if (Panels.Contains(panel))
        {
            if (CurrentPanel != null)
                CurrentPanel.SetActive(false);
            CurrentPanel = panel;
            CurrentPanel.SetActive(true);
        }
        else return false;
        return true;
    }

    /// <summary>
    /// Focus on the selected Panel.
    /// Trigger its handling if available.
    /// </summary>
    /// <param name="panel">Selected Panel</param>
    public void FocusPanel(AbstractMenuPanel panel)
    {
        if (FocusPanelObject(panel.gameObject))
            panel.handle();
    }

    /// <summary>
    /// Quit the current Application.
    /// Meant for a Button 'onPressEvent'.
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
