using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : AbstractMenuPanel
{
    public Button loadingBtn;

    public override void handle()
    {
        loadingBtn.interactable = SceneTranscriver.GetFiles().Count > 0;
    }
}
