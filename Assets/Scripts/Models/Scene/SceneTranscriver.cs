using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;

/// <summary>
/// Allow for better use of the File system by writing, reading and accessing files in your 'Scenes/' local folder.
/// </summary>
public class SceneTranscriver : MonoBehaviour
{
    /// <summary>
    /// Permanent path of the current Save File folder.
    /// </summary>
    private static string rootPath = null;

    /// <summary>
    /// Data of the currently built local scene.
    /// </summary>
    [HideInInspector] public SceneData data = null;

    /// <summary>
    /// Awake Function, init the root Path.
    /// </summary>
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Initialize the static root Path for any scene Save File folder.
    /// </summary>
    private static void Init()
    {
        rootPath = Application.persistentDataPath + "/Scenes";
    }

    /// <summary>
    /// Load a specific scene from its basic, and specified, file name.
    /// </summary>
    /// <param name="fileName">Specific name file</param>
    /// <returns>Verification if the file is existant/accessible</returns>
    public bool LoadFile(string fileName)
    {
        string destination = rootPath + "/" + fileName + ".dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SceneData loaded = (SceneData)bf.Deserialize(file);
        file.Close();

        data = new SceneData(loaded.models, loaded.PlayerPosition.get(),
            loaded.PlayerEulerRotation.get(), loaded.PlayerScale.get());
        return true;
    }

    /// <summary>
    /// Save a specific scene from its basic, and specified, file name.
    /// </summary>
    /// <param name="fileName">Specific name file</param>
    /// <returns>Verification if the file is existant/accessible</returns>
    public bool SaveFile(string fileName)
    {
        string destination = rootPath + "/" + fileName + ".dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else if (data == null)
        {
            Debug.LogError("Data inexistant");
            return false;
        }
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        SceneData saved = new SceneData(data.models, data.PlayerPosition.get(),
            data.PlayerEulerRotation.get(), data.PlayerScale.get());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, saved);

        file.Close();
        return false;
    }

    /// <summary>
    /// Access all existing file names in rootPath.
    /// </summary>
    /// <returns>List of all file names</returns>
    public static List<string> GetFiles()
    {
        if (rootPath == null)
            Init();

        List<string> res = new List<string>();
        DirectoryInfo info = new DirectoryInfo(rootPath);

        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
            if (file.Name.Substring(file.Name.IndexOf(".")) == ".dat")
                res.Add(file.Name.Substring(0, file.Name.IndexOf(".")));

        return res;
    }

    /// <summary>
    /// Verify if a file of a specific name already exists in root path.
    /// If not, create the new file in root path.
    /// </summary>
    /// <param name="fileName">Specific file name</param>
    /// <returns>Result of the verification</returns>
    public static bool CreateFile(string fileName)
    {
        if (rootPath == null)
            Init();

        string destination = rootPath + "/" + fileName + ".dat";

        if (!File.Exists(destination))
            File.Create(destination).Close();
        else return false;

        return true;
    }
}
