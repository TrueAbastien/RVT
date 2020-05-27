using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common class, part of VR Input define.
/// Allow us to handle everything on a controller.
/// </summary>
public class VRHand
{
    /// <summary>
    /// Enumartion of each existing hand side.
    /// </summary>
    public enum Hand { RIGHT, LEFT, __COUNT }

    /// <summary>
    /// Enumeration of each remaining hand axis (beside buttons).
    /// </summary>
    public enum Axis { ThumbstickX, ThumbstickY, Middle, Index, __COUNT }
    /// <summary>
    /// Replacement table for each Input Axis Name depending on which hand is called.
    /// </summary>
    private static string[,] __axis = new string[(int)Axis.__COUNT, (int)Hand.__COUNT + 1]
    {
        { "Oculus_GearVR_*ThumbstickX", "R", "L" },
        { "Oculus_GearVR_*ThumbstickY", "R", "L" },
        { "*HandAxis", "R", "L" },
        { "*IndexButton", "R", "L" }
    };

    /// <summary>
    /// Access Input Axis Name from a specific controller axis.
    /// </summary>
    /// <param name="_hand">Specific hand (Right or Left)</param>
    /// <param name="_axis">Specific axis</param>
    /// <returns>Name of the corresponding Input Axis</returns>
    public static string inputFrom(Hand _hand, Axis _axis)
    {
        return __axis[(int)_axis, 0].Replace('*', __axis[(int)_axis, (int)_hand + 1][0]);
    }

    /// <summary>
    /// Access the opposite hand of the one specified.
    /// </summary>
    /// <param name="__hand">Specific hand (Right or Left)</param>
    /// <returns>Left (if Right) or Right (if Left)</returns>
    public static Hand oppositeOf(Hand __hand)
    {
        return (Hand)(((int)__hand + 1) % (int)Hand.__COUNT);
    }

    /// <summary>
    /// Struct of every existing theorical control on both Oculus Rift Controller.
    /// Allow for quick Item Handling access of the most valuable data.
    /// </summary>
    public struct Controls
    {
        /// <summary>
        /// Structure for every theorical axis on a Oculus Rift Controller.
        /// </summary>
        public struct HandInput
        {
            /// <summary>
            /// Content table: input axis names.
            /// </summary>
            public string[] content;
            /// <summary>
            /// Getter for the Horizontal Axis of the Top Joystick.
            /// </summary>
            public string ThumbX { get { return content[(int)Axis.ThumbstickX]; } }
            /// <summary>
            /// Getter for the Vertical Axis of the Top Joystick.
            /// </summary>
            public string ThumbY { get { return content[(int)Axis.ThumbstickY]; } }
            /// <summary>
            /// Getter for the Lower Middle-finger button.
            /// </summary>
            public string Middle { get { return content[(int)Axis.Middle]; } }
            /// <summary>
            /// Getter for the Upper Index button.
            /// </summary>
            public string Index { get { return content[(int)Axis.Index]; } }
            /// <summary>
            /// Copy hand input content from another instance.
            /// </summary>
            /// <param name="hi">Source hand input instance</param>
            public void Copy(HandInput hi) { hi.content.CopyTo(content, 0); }
        }
        /// <summary>
        /// Content for both hand input (Right & Left).
        /// </summary>
        public HandInput[] content;

        /// <summary>
        /// Getter for the primary hand input (depending on user preferences).
        /// </summary>
        public HandInput primary { get { return content[(int)main]; } }
        /// <summary>
        /// Getter for the secondary hand input (depending on user preferences).
        /// </summary>
        public HandInput secondary { get { return content[((int)main + 1) % 2]; } }
        /// <summary>
        /// Current primary hand side (Right or Left).
        /// </summary>
        public Hand main;
    }

    /// <summary>
    /// Controls constructor generating over any hand side (Right or Left).
    /// Taking the specified hand as the main one.
    /// </summary>
    /// <param name="mainHand">Main user hand to construct over</param>
    /// <returns>Constructed controls</returns>
    public static Controls constructOver(Hand mainHand)
    {
        var res = new Controls();
        res.content = new Controls.HandInput[2];
        res.content[0] = new Controls.HandInput();
        res.content[1] = new Controls.HandInput();
        res.main = mainHand;

        for (int ii = 0; ii < 2; ++ii)
        {
            res.content[ii].content = new string[4];
            for (int jj = 0; jj < 4; ++jj)
                res.content[ii].content[jj] = inputFrom((Hand)ii, (Axis)jj);
        }

        return res;
    }
    /// <summary>
    /// Change the current main hand.
    /// </summary>
    /// <param name="content">Reference to the current Controls instance</param>
    public static void reverseHand(ref Controls content)
    {
        content.main = oppositeOf(content.main);
    }
}
