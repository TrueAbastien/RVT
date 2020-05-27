using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Generation of the User Inventory when Awake.
/// </summary>
public class InventoryGenerator : MonoBehaviour
{
    /// <summary>
    /// Main inventory instance.
    /// </summary>
    public static InventoryGenerator main;

    /// <summary>
    /// Awake Function, basic init & call 'onAwake' method.
    /// </summary>
    public void Awake()
    {
        main = this;

        invContent = new List<GameObject[]>();
        onAwake();
    }

    /// <summary>
    /// Structure of every Inventory Item theoricall content.
    /// </summary>
    [System.Serializable]
    public struct _InventoryItem { public Sprite pic; public string name; public GameObject prefab; }

    // ----------------------------------------------------------------------------------------- //

    /// <summary>
    /// Item prefab for initialization.
    /// </summary>
    public GameObject ItemPrefab;
    /// <summary>
    /// Holding image preset.
    /// </summary>
    public Sprite holdingImage;
    /// <summary>
    /// Empty image preset.
    /// </summary>
    public Sprite emptyImage;
    /// <summary>
    /// List of every Inventory Item created.
    /// </summary>
    public List<_InventoryItem> inventory;
    /// <summary>
    /// Default image (when no picture is attributed).
    /// </summary>
    public Sprite DefaultImage;

    /// <summary>
    /// Offset for the visual Item prefab in Inventory.
    /// </summary>
    public Vector2 offset = new Vector2(-1.25e-2f, -4.5e-2f);
    /// <summary>
    /// Width of a visual Item.
    /// </summary>
    public float holderWidth = 1e-1f;
    /// <summary>
    /// Max possible Items in Inventory.
    /// </summary>
    public int maxItems = 10;
    /// <summary>
    /// List of every Items: visual content & prefab.
    /// </summary>
    [HideInInspector] public List<GameObject[]> invContent;

    /// <summary>
    /// Temporary instantiated Item prefab.
    /// </summary>
    private GameObject tItem;


    /// <summary>
    /// Awake Function, fill Inventory content on first frame from the specified editor content.
    /// </summary>
    private void onAwake()
    {
        // Fill Content
        int invSize = inventory.Count;
        float padding = .5f * invSize * holderWidth;
        for (int ii = 0; ii < invSize && ii < maxItems; ++ii)
        {
            invContent.Add(new GameObject[] { tItem = Instantiate(ItemPrefab, transform), inventory[ii].prefab });
            tItem.transform.localPosition = new Vector3(offset.x - padding + holderWidth * ii, offset.y, 0f);

            // Children:
            // 0 -> Background
            // 1 -> Picture
            // 2 -> Label
            tItem.transform.GetChild(0).GetComponent<Image>().sprite = emptyImage;
            tItem.transform.GetChild(1).GetComponent<Image>().sprite = inventory[ii].pic == null ? DefaultImage : inventory[ii].pic;
            tItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inventory[ii].name;
        }
    }

    /// <summary>
    /// Hold on to a specific item index.
    /// Called on item-cycling to change color.
    /// </summary>
    /// <param name="index">Specific item index</param>
    /// <param name="col">Color of the currently hold visual Item</param>
    public void holdOn(int index, Color col)
    {
        invContent[index][0].transform.GetChild(0).GetComponent<Image>().sprite = holdingImage;
        invContent[index][0].transform.GetChild(0).GetComponent<Image>().color = col;
    }
}
