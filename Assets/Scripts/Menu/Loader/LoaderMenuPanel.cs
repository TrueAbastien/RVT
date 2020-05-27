using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Menu Panel, allow user to load any Saved Scene File for construction.
/// </summary>
[RequireComponent(typeof(SceneTranscriver))]
public class LoaderMenuPanel : AbstractMenuPanel
{
    /// <summary>
    /// Content Viewport object to create buttons.
    /// </summary>
    public GameObject Content;
    /// <summary>
    /// Load-Scene button prefab.
    /// </summary>
    public GameObject ButtonPrefab;
    /// <summary>
    /// Prefab instantiating the 'View Scene' content.
    /// </summary>
    public GameObject InitPrefab;

    /// <summary>
    /// Remove all previous button & add them all (with any new one).
    /// </summary>
    public override void handle()
    {
        // Destroy All Children
        foreach (Button child in Content.transform.GetComponentsInChildren<Button>())
            Destroy(child.gameObject);

        // Add All Children
        int counter = 0;
        foreach (string name in SceneTranscriver.GetFiles())
        {
            GameObject instance = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0), Content.transform);
            instance.GetComponent<LoadButtonIndex>().value = counter;

            instance.GetComponentInChildren<TextMeshProUGUI>().SetText(name);
            instance.GetComponent<LoadButtonIndex>().loader = this;

            float height = instance.GetComponent<RectTransform>().rect.height;
            instance.GetComponent<RectTransform>().localPosition = new Vector3(instance.GetComponent<RectTransform>().rect.right + 10,
                -((height + 10) * counter++ + (height / 2 + 10)), 0);
        }
    }

    /// <summary>
    /// Load a save file from a specific Button Index.
    /// </summary>
    /// <param name="index">Indexed reference storing an index value</param>
    public void LoadFile(LoadButtonIndex index)
    {
        Button[] children = Content.GetComponentsInChildren<Button>();
        GetComponent<SceneTranscriver>().LoadFile(children[index.value].GetComponentInChildren<TextMeshProUGUI>().text);

        Instantiate(InitPrefab).GetComponent<SceneInit>().Save(GetComponent<SceneTranscriver>().data);
        SceneManager.LoadScene("Viewer");
    }
}
