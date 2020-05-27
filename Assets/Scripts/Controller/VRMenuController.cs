using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controller for the Secondary Hand Menu allowing user to perform various basic application related task.
/// These currently includes:
/// - Teleporting the player body at his spawnpoint.
/// - Bringing the current drone in front of the current user.
/// - Going back to the 'Construction Scene'.
/// </summary>
public class VRMenuController : MonoBehaviour
{
    /// <summary>
    /// Hand for the current menu (secondary hand, currently).
    /// </summary>
    private VRHand.Hand MenuHand;
    /// <summary>
    /// Switch button:
    /// - short press for turning the menu on and off.
    /// - long press for changing to the next button after a specified 'Time for Change' value (repeating until released).
    /// </summary>
    private VRButton.Code SwitchButton;

    /// <summary>
    /// Verify if the current index has been changed or not.
    /// </summary>
    private bool hasBeenChanged = false;
    /// <summary>
    /// Store the time in seconds since the last Switch Button press.
    /// Used to determine if a press is long or short.
    /// </summary>
    private float timeSincePress = 0;
    /// <summary>
    /// Time for index Change, changing the current button after (the specified value) time duration in seconds.
    /// </summary>
    [Range(.5f, 2f)] public float TimeForChange = 1f;

    /// <summary>
    /// Color of the currenlty selected Button.
    /// </summary>
    public Color SelectionColor;
    /// <summary>
    /// Color of any other Button (not selected).
    /// </summary>
    public Color UnselectedColor;

    /// <summary>
    /// Table of each and every Button selectable.
    /// </summary>
    private Button[] Actions;
    /// <summary>
    /// Index of the currently selected Button.
    /// </summary>
    private int currentButtonIndex = 0;


    /// <summary>
    /// Start Function, initialize propreties and select first Button (having no purpose).
    /// </summary>
    void Start()
    {
        MenuHand = VRHand.oppositeOf(MGR_VRControls.mainHand);
        SwitchButton = (MenuHand == VRHand.Hand.RIGHT) ? VRButton.Code.B : VRButton.Code.Y;

        transform.SetParent(MenuHand == VRHand.Hand.RIGHT ?
            MGR_VRControls.get.RightHand.transform : MGR_VRControls.get.LeftHand.transform);
        transform.localPosition = new Vector3(0, .25f, 0);

        Actions = GetComponentsInChildren<Button>();
        foreach (Button btn in Actions)
            btn.image.color = UnselectedColor;
        LoadButton(0);

        SavedPlayerPosition = VRUserController.main.transform.position;

        setActivaton(false);
    }

    /// <summary>
    /// Update Function, process through Switch Button current Inputs.
    /// </summary>
    void Update()
    {
        // On Button Press
        if (Input.GetButtonDown(VRButton.toString(SwitchButton)))
        {
            setActivaton(true);
        }

        // If Button is Pressed
        if (Input.GetButton(VRButton.toString(SwitchButton)))
        {
            timeSincePress += Time.deltaTime;
            if (timeSincePress > TimeForChange)
            {
                timeSincePress -= TimeForChange;
                hasBeenChanged = true;

                NextButton();
            }
        }

        // On Button Release
        if (Input.GetButtonUp(VRButton.toString(SwitchButton)))
        {
            if (!hasBeenChanged)
                setActivaton(false);

            timeSincePress = 0;
            hasBeenChanged = false;
        }
    }

    /// <summary>
    /// Change the current Button Index value and change the Buttons color accordingly.
    /// </summary>
    /// <param name="idx">New index value</param>
    private void changeIndexTo(int idx)
    {
        Actions[currentButtonIndex].image.color = UnselectedColor;
        currentButtonIndex = idx;
        Actions[currentButtonIndex].image.color = SelectionColor;
    }

    /// <summary>
    /// Change the activation state of the Menu object (turning on and off).
    /// If closing, loads the current button function.
    /// </summary>
    /// <param name="state">New specific state of activation</param>
    private void setActivaton(bool state)
    {
        if (!state)
        {
            LoadCurrentButton();
            changeIndexTo(0);
        }
        transform.GetChild(0).gameObject.SetActive(state);
    }

    /// <summary>
    /// Change to the next Button in the list.
    /// </summary>
    private void NextButton()
    {
        changeIndexTo((currentButtonIndex + 1) % Actions.Length);
    }

    /// <summary>
    /// Load a button at a specific index.
    /// </summary>
    /// <param name="index">Specific index</param>
    private void LoadButton(int index)
    {
        changeIndexTo(index);
        LoadCurrentButton();
    }

    /// <summary>
    /// Load the currenlty selected Button by calling its 'onClick' function.
    /// </summary>
    private void LoadCurrentButton()
    {
        Actions[currentButtonIndex].onClick.Invoke();
    }

    // ----------------------------------------------------------------------------------------- //

    /// <summary>
    /// Body of the current Player object.
    /// </summary>
    public GameObject PlayerBody;
    /// <summary>
    /// Body of the current Drone object.
    /// </summary>
    public GameObject DroneBody;
    /// <summary>
    /// Saved Player position as Spawnpoint.
    /// Set on Scene Initialization.
    /// </summary>
    private Vector3 SavedPlayerPosition = Vector3.zero;

    /// <summary>
    /// Button Function, teleport the Player body to the Saved Player Position.
    /// </summary>
    public void RespawnPlayer()
    {
        PlayerBody.transform.position = SavedPlayerPosition;
    }

    /// <summary>
    /// Button Function, bring the drone in front of the Player body.
    /// </summary>
    public void BringDrone()
    {
        DroneBody.transform.position = PlayerBody.transform.position + Camera.current.transform.forward;
    }

    /// <summary>
    /// Button Function, load the 'Construction Scene' menu, exiting the current scene ('View Scene').
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
