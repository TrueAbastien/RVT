using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Menu Panel, allow user to change 'View Scene' settings on Load.
/// </summary>
public class SettingsMenuPanel : AbstractMenuPanel
{
    /// <summary>
    /// Seriazable Data for the current Settings.
    /// </summary>
    [System.Serializable]
    public class SettingData
    {
        /// <summary>
        /// Main hand for the current user.
        /// </summary>
        public VRHand.Hand MainHand;
        /// <summary>
        /// Current user size (used to scale every scene object in 'View Scene').
        /// </summary>
        public float UserSize;

        /// <summary>
        /// Default constructor.
        /// Take the row data of any current settings.
        /// </summary>
        /// <param name="hand">Main hand for the current user</param>
        /// <param name="size">Size of the player in game</param>
        public SettingData(VRHand.Hand hand, float size)
        {
            MainHand = hand; UserSize = size;
        }
    }

    /// <summary>
    /// Currently selected data.
    /// </summary>
    [HideInInspector] public static SettingData data;

    /// <summary>
    /// Dropdown menu to pick the current main hand.
    /// </summary>
    public TMP_Dropdown handPicker;
    /// <summary>
    /// Scrollbar to change the current user size.
    /// </summary>
    public Scrollbar sizeScroll;

    /// <summary>
    /// Minimum user size possible.
    /// </summary>
    [Range(.1f, 1f)] public float minUserSize;
    /// <summary>
    /// Maximum user size possible.
    /// </summary>
    [Range(2f, 5f)] public float maxUserSize;

    /// <summary>
    /// Root path for 'settings.dat' file (containing the current Settings Data).
    /// </summary>
    private static string rootPath = null;

    /// <summary>
    /// Empty overwritten function.
    /// </summary>
    public override void handle() { }

    /// <summary>
    /// Awake Function, initialize settings data (through file system) and modify UI elements accordingly.
    /// </summary>
    private void Awake()
    {
        rootPath = Application.persistentDataPath;
        if (!readFile())
            data = new SettingData(VRHand.Hand.RIGHT, 2f);

        handPicker.onValueChanged.AddListener(delegate { 
            setMainHand(handPicker.options[handPicker.value].text.ToUpper()[0]); });
        sizeScroll.onValueChanged.AddListener(delegate {
            changeUserSize(sizeScroll.value); });

        handPicker.value = (data.MainHand == VRHand.Hand.RIGHT) ? 0 : 1;
        sizeScroll.value = (data.UserSize - minUserSize) / (maxUserSize - minUserSize);
    }

    /// <summary>
    /// Verify if a file is readable.
    /// If it is, update the current read the current Settings Data int 'settings.dat' file.
    /// </summary>
    /// <returns>Result of the verification</returns>
    private bool readFile()
    {
        string destination = rootPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        if (file.Length == 0)
        {
            Debug.Log("Empty file !");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SettingData loaded = (SettingData)bf.Deserialize(file);
        file.Close();

        data = new SettingData(loaded.MainHand, loaded.UserSize);
        return true;
    }

    /// <summary>
    /// Save the current Settings Data in 'settings.dat' file.
    /// </summary>
    public void saveFile()
    {
        string destination = rootPath + "/settings.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else if (data == null)
        {
            Debug.LogError("Data inexistant");
            return;
        }
        else
        {
            Debug.LogError("File not found");
            return;
        }

        SettingData saved = new SettingData(data.MainHand, data.UserSize);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, saved);

        file.Close();
        return;
    }

    /// <summary>
    /// Event for the Hand Picker dropdown element selection.
    /// </summary>
    /// <param name="hand">First character of the selected element test</param>
    public void setMainHand(char hand) { data.MainHand = (hand == 'R') ? VRHand.Hand.RIGHT : VRHand.Hand.LEFT; }

    /// <summary>
    /// Event for the User Size scrollbar change of value.
    /// </summary>
    /// <param name="val">Current value (between 0 and 1 with .1 step)</param>
    public void changeUserSize(float val) { data.UserSize = val * (maxUserSize - minUserSize) + minUserSize; }
}
