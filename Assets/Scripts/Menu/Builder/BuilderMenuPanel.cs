using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Menu Panel, allow user to move imported objects and save them as Scene Data.
/// </summary>
[RequireComponent(typeof(SceneTranscriver))]
public class BuilderMenuPanel : AbstractMenuPanel
{
    /// <summary>
    /// Save file input name.
    /// </summary>
    public TMP_InputField input;

    /// <summary>
    /// Empty overwritten function.
    /// </summary>
    public override void handle() { }

    /// <summary>
    /// Modify the content of an already created file.
    /// </summary>
    public void AlterCurrentFile()
    {
        if (input.text.Length > 0)
            if (GetComponent<SceneTranscriver>().data != null)
            {
                SceneTranscriver.CreateFile(input.text);
                GetComponent<SceneTranscriver>().SaveFile(input.text);
            }
    }

    /// <summary>
    /// Enumeration of every transformation possible on an object.
    /// </summary>
    public enum CurrentTool { TRANSLATE, ROTATE, SCALE };
    /// <summary>
    /// Currently selected tool for object transformation.
    /// </summary>
    [HideInInspector] public CurrentTool tool = CurrentTool.TRANSLATE;

    /// <summary>
    /// Dropdown of every tool selectable.
    /// </summary>
    public TMP_Dropdown toolSelector;
    /// <summary>
    /// Awake Function, delegate tool selection method on 'toolSelector.onValueChanged' event.
    /// </summary>
    private void Awake()
    {
        toolSelector.onValueChanged.AddListener(delegate { SelectTool(); });
    }
    /// <summary>
    /// Tool selection method.
    /// Read the first character of the newly selected dropdown tab to set the current Transformation Tool.
    /// </summary>
    private void SelectTool()
    {
        switch (toolSelector.options[toolSelector.value].text.ToUpper()[0])
        {
            case 'T': tool = CurrentTool.TRANSLATE; break;
            case 'R': tool = CurrentTool.ROTATE; break;
            case 'S': tool = CurrentTool.SCALE; break;
            default: break;
        }
    }
}
