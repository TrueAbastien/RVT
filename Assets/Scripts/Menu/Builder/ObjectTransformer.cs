using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allow object be moved my mouse controls on Builder Panel.
/// </summary>
public class ObjectTransformer : MonoBehaviour
{
    private float
        mouseScrollSpeed = 2.5f,
        mouseRotateSpeed = 15f,
        mouseScaleSpeed = .2f,
        minimalDistance = 2f,
        minimalScale = .2f;
    /// <summary>
    /// Builder Panel current instance.
    /// </summary>
    public BuilderMenuPanel panel;

    /// <summary>
    /// Distance between camera and object.
    /// </summary>
    private float translateDistance = 10f;
    /// <summary>
    /// Currently selected instance, shared between every instance.
    /// </summary>
    private static ObjectTransformer selectedInstance = null;
    /// <summary>
    /// Hit target for mouse Raycast.
    /// </summary>
    private RaycastHit hit;


    /// <summary>
    /// Start Function, call initialization.
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Initialization of the 'translateDistance'.
    /// </summary>
    public void Init()
    {
        // Determine Distance
        translateDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
    }

    /// <summary>
    /// Update Function, allow for item selection, unselection & drag transformation (depending on the Current Tool).
    /// </summary>
    private void Update()
    {
        // Select Object
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    selectedInstance = this;
                }
            }
        }
            

        // Drag Instance
        if (selectedInstance == this)
        {
            switch (panel.tool)
            {
                // Translation
                case BuilderMenuPanel.CurrentTool.TRANSLATE:
                    translateDistance = Mathf.Max(translateDistance + Input.mouseScrollDelta.y * mouseScrollSpeed, minimalDistance);
                    Ray look = Camera.main.ScreenPointToRay(Input.mousePosition);
                    transform.position = look.GetPoint(translateDistance / look.direction.z);
                    break;

                // Rotation
                case BuilderMenuPanel.CurrentTool.ROTATE:
                    transform.localRotation =
                        Quaternion.AngleAxis(-Input.GetAxis("Mouse X") * mouseRotateSpeed, Camera.main.transform.up) *
                        Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * mouseRotateSpeed, Camera.main.transform.right) *
                        transform.localRotation;
                    break;

                // Scaling
                case BuilderMenuPanel.CurrentTool.SCALE:
                    Vector3 previousScale = transform.localScale;
                    transform.localScale *= 1 + Input.GetAxis("Mouse X") * mouseScaleSpeed;
                    if (transform.lossyScale.x < minimalScale)
                        transform.localScale = previousScale;
                    break;

                default: break;
            }
        }

        // Unselect Object
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedInstance != null)
            {
                selectedInstance = null;
            }
        }
    }
}
