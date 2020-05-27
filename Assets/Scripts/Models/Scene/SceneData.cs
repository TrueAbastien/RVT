using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene data for File Saving scene elements.
/// </summary>
[System.Serializable]
public class SceneData
{
    /// <summary>
    /// Equivalent of Vector3 as Serializable object.
    /// </summary>
    [System.Serializable]
    public class SeriazableV3
    {
        /// <summary>
        /// X position.
        /// </summary>
        public float x;
        /// <summary>
        /// Y position.
        /// </summary>
        public float y;
        /// <summary>
        /// Z position.
        /// </summary>
        public float z;
        /// <summary>
        /// Get current instance data as Vector3.
        /// </summary>
        /// <returns>Result of transformation</returns>
        public Vector3 get()
        {
            return new Vector3(x, y, z);
        }
        /// <summary>
        /// Default constructor.
        /// Store a Vector3 data per axis.
        /// </summary>
        /// <param name="v">Stored vec3 information</param>
        public SeriazableV3(Vector3 v)
        {
            x = v.x; y = v.y; z = v.z;
        }
    }

    /// <summary>
    /// Model data for a specific imported Model.
    /// </summary>
    [System.Serializable]
    public class ModelData
    {
        /// <summary>
        /// Google Poly key for importation.
        /// </summary>
        public string key;
        /// <summary>
        /// World position for the model.
        /// </summary>
        public SeriazableV3 position;
        /// <summary>
        /// World rotation, in euler angles, for the model.
        /// </summary>
        public SeriazableV3 eulerRotation;
        /// <summary>
        /// World scaling for the model.
        /// </summary>
        public SeriazableV3 scale;

        /// <summary>
        /// Default constructor.
        /// Take a key and the current object transform to construct.
        /// </summary>
        /// <param name="_key">Key for the imported object</param>
        /// <param name="_obj">Object current transform</param>
        public ModelData(string _key, Transform _obj)
        {
            key = _key;
            position = new SeriazableV3(_obj.position);
            eulerRotation = new SeriazableV3(_obj.rotation.eulerAngles);
            scale = new SeriazableV3(_obj.lossyScale);
        }
    }

    /// <summary>
    /// List of all imported Models.
    /// </summary>
    public List<ModelData> models;
    /// <summary>
    /// World position of the unique scene Player Object.
    /// </summary>
    public SeriazableV3 PlayerPosition;
    /// <summary>
    /// World rotation, in euler angles, of the unique scene Player Object.
    /// </summary>
    public SeriazableV3 PlayerEulerRotation;
    /// <summary>
    /// World scaling of the unique scene Player Object.
    /// </summary>
    public SeriazableV3 PlayerScale;

    /// <summary>
    /// Default constructor.
    /// Every content for a Scene Save File needs to be specified.
    /// </summary>
    /// <param name="_models">List of every models data</param>
    /// <param name="_playerPosition">Current position of the Player</param>
    /// <param name="_playerEulerRotation">Current rotation of the Player</param>
    /// <param name="_playerScale">Current scale of the Player</param>
    public SceneData(List<ModelData> _models, Vector3 _playerPosition, Vector3 _playerEulerRotation, Vector3 _playerScale)
    {
        models = _models;
        PlayerPosition = new SeriazableV3(_playerPosition);
        PlayerEulerRotation = new SeriazableV3(_playerEulerRotation);
        PlayerScale = new SeriazableV3(_playerScale);
    }
}
