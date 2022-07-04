using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using Unity.Mathematics;


[System.Serializable]
public class StorageSG
{
    public System.DateTime myDateTime;

    public StorageSG()
    {
        myDateTime = System.DateTime.UtcNow;
    }
}

public class GameDataHandler : MonoBehaviour
{    
    public List<GameObject> targetList;
    public List<GameObject> destroyedObjects;
    public List<GameObject> instantiatedObjects;
    
    public List<GameObject> materials;
    public List<GameObject> lines;

    public List<GameObject> changedObjectsDestroyed;
    public List<GameObject> changedObjects;
    public List<GameObject> changedLines;
    public List<string> dataStrings;

    public List<string> loadedDataStrings;
    public List<string> loadedAutoStrings;

    public UIScript uiScript;
    

    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private GameObject areYouSure;    
    public Dropdown loadedLevelDropdown, loadedLevelAutoDropdown;
    private string timeDate;
    public string changeScene = null;
    public string changeSceneAuto = null;
    public bool loadOnStart = false, save = false;
    private int instantiatedObjectsCount = 0, materialCount = 0, linesCount = 0;
    private bool areYouSureBool = true, saveDataBool = true;
    [HideInInspector]
    public bool autoSave = false;


    void Start()
    {
        SaveGame.Encode = false;
        StartCoroutine(LoadedDataStrings());
        StartCoroutine(LoadedAutoStrings());

    }

    public void LoadStrings()
    {
        StartCoroutine(LoadedDataStrings());
        StartCoroutine(LoadedAutoStrings());
    }

    public void ClearAllLists()
    {
        targetList.Clear();
        destroyedObjects.Clear();
        instantiatedObjects.Clear();
        materials.Clear();
        lines.Clear();
        changedObjects.Clear();   
        changedLines.Clear();
        changedObjectsDestroyed.Clear();
    }

    void OnApplicationQuit()
    {
        autoSave = true;        
        if (save && !uiScript.camera3D.activeSelf)
        {
            Debug.Log("Save");
            Save();
        }
    }

    public void Save()
    {
        timeDate = System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        if (!Directory.Exists(Application.persistentDataPath + "/Data_Autosaves/"))
        {            
            Directory.CreateDirectory(Application.persistentDataPath + "/Data_Autosaves/");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/Autosaves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Autosaves/");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/SavedAssets/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SavedAssets/");
        }
        if (saveDataBool)
        {
            StartCoroutine(SaveData());
        }
        if (areYouSureBool)
        {

            if (Directory.Exists(Application.persistentDataPath + "/SavedAssets/" + inputField.text + "/") && !autoSave)
            {
                Directory.Delete(Application.persistentDataPath + "/SavedAssets/" + inputField.text + "/", true);
                Directory.CreateDirectory(Application.persistentDataPath + "/SavedAssets/" + inputField.text + "/");
            }
            if(!Directory.Exists(Application.persistentDataPath + "/SavedAssets/" + inputField.text + "/") && !autoSave)
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/SavedAssets/" + inputField.text + "/");
            }
            if(autoSave)
            {                
                Directory.CreateDirectory(Application.persistentDataPath + "/Autosaves/" + "Autosave_" + timeDate + "/");
            }
            
            if (destroyedObjects != null)
            {
                for (int i = 0; i < destroyedObjects.Count; i++)
                {
                    MyData data = new MyData();
                    data.position = destroyedObjects[i].transform.position;
                    data.inputFieldText = inputField.text;
                    if (!autoSave)
                    {
                        if (!destroyedObjects[i].name.Contains("tree_main"))
                        {
                            SaveGame.Save("SavedAssets/" + inputField.text + "/" + destroyedObjects[i].name + "Destroyed", data);
                        }
                        else
                        {
                            SaveGame.Save("SavedAssets/" + inputField.text + "/" + destroyedObjects[i].transform.parent.name + "Destroyed", data);
                        }
                    }
                    else
                    {
                        if (!destroyedObjects[i].name.Contains("tree_main"))
                        {
                            SaveGame.Save("Autosaves/" + inputField.text + "/" + destroyedObjects[i].name + "Destroyed", data);
                        }
                        else
                        {
                            SaveGame.Save("Autosaves/" + inputField.text + "/" + destroyedObjects[i].transform.parent.name + "Destroyed", data);
                        }
                        
                    }
                }
            }
            if (instantiatedObjects != null)
            {
                for (int i = 0; i < instantiatedObjects.Count; i++)
                {
                    GameObject a = instantiatedObjects[i];
                    MyData data = new MyData();
                    data.nameData = a.name;
                    data.position = a.transform.position;
                    data.rotation = a.transform.eulerAngles;
                    data.scale = a.transform.localScale;
                    data.inputFieldText = inputField.text;
                    if (!autoSave)
                    {
                        SaveGame.Save("SavedAssets/" + inputField.text + "/" + i.ToString() + "Instantiate", data);
                        
                    }
                    else
                    {
                        SaveGame.Save("Autosaves/" + inputField.text + "/" + i.ToString() + "Instantiate", data);
                    }
                }
            }
            if (materials != null)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    GameObject a = materials[i];
                    MyData data = new MyData();
                    data.nameData = a.name;

                    string newMaterialName = materials[i].GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
                    data.materialName = newMaterialName;
                    data.inputFieldText = inputField.text;

                    if (!autoSave)
                    {
                        SaveGame.Save("SavedAssets/" + inputField.text + "/" + i.ToString() + "Material", data);                        
                    }
                    else
                    {
                        SaveGame.Save("Autosaves/" + inputField.text + "/" + i.ToString() + "Material", data);
                    }
                }
            }
            if (lines != null)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    GameObject a = lines[i];
                    MyData data = new MyData();
                    data.position = lines[i].transform.position;
                    data.rotation = lines[i].transform.eulerAngles;
                    LineRenderer lineRenderer = a.GetComponent<LineRenderer>();
                    Vector3[] newPos = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(newPos);
                    data.positionLineRenderer = newPos;

                    data.materialName = lineRenderer.material.name.Replace(" (Instance)", "");
                    data.widthStart = lineRenderer.startWidth;
                    data.widthEnd = lineRenderer.endWidth;
                    data.inputFieldText = inputField.text;

                    if (!autoSave)
                    {
                        SaveGame.Save("SavedAssets/" + inputField.text + "/" + i.ToString() + "Lines", data);
                    }
                    else
                    {
                        SaveGame.Save("Autosaves/" + inputField.text + "/" + i.ToString() + "Lines", data);
                    }
                }
            }
            if (!loadedDataStrings.Contains(inputField.text) && inputField.text != null)
            {                
                StartCoroutine(LoadedDataStrings());
            }
            saveDataBool = true;
            areYouSure.SetActive(false);
        }
    }

    public void Ja()
    {
        saveDataBool = false;
        StartCoroutine(JaNumerator());
        Save();
    }
    public void Nee()
    {
        saveDataBool = true;
        areYouSureBool = true;
        areYouSure.SetActive(false );
    }

    IEnumerator JaNumerator()
    {
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        MyData dataText = new MyData();
        dataText.inputFieldText = inputField.text;        
        dataText.sceneString = sceneName;
        SaveGame.Save("Data/" + inputField.text + "_DataInputField", dataText);        
        areYouSureBool = true;
        yield return null;
    }
       

    IEnumerator LoadedDataStrings()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Data"))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/Data");
            for (int i = 0; i < files.Length; i++)
            {
                MyData data = SaveGame.Load<MyData>(files[i]);
                if (data.inputFieldText != null)
                {
                    loadedDataStrings.Add(data.inputFieldText);
                }
            }      
        }                    
        loadedLevelDropdown.ClearOptions();
        loadedLevelDropdown.AddOptions(loadedDataStrings);
        loadedDataStrings.Clear();
        yield return null;
    }
    IEnumerator LoadedAutoStrings()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Data_Autosaves"))
        {

            string[] files = Directory.GetFiles(Application.persistentDataPath + "/Data_Autosaves");            
            for (int i = 0; i < files.Length; i++)
            {
                MyData data = SaveGame.Load<MyData>(files[i]);
                if (data.inputFieldText != null)
                {                    
                    loadedAutoStrings.Add(data.inputFieldText);                       
                }
            }            
        }
        loadedLevelAutoDropdown.ClearOptions();
        loadedLevelAutoDropdown.AddOptions(loadedAutoStrings);    
        loadedAutoStrings.Clear();
        yield return null;
    }

    public void Load()
    {
        if (changeScene == null)
        {
            int index = loadedLevelDropdown.value;

            for (int i = 0; i < targetList.Count; i++)
            {
                if (!targetList[i].name.Contains("tree_main"))
                {
                    if (SaveGame.Exists("SavedAssets/" + loadedLevelDropdown.options[index].text + "/" + targetList[i].name + "Destroyed"))
                    {
                        changedObjectsDestroyed.Add(targetList[i]);
                        targetList[i].SetActive(false);
                    }
                }
                else
                {
                    if (SaveGame.Exists("SavedAssets/" + loadedLevelDropdown.options[index].text + "/" + targetList[i].transform.parent.name + "Destroyed"))
                    {
                        changedObjectsDestroyed.Add(targetList[i]);
                        targetList[i].SetActive(false);
                    }
                }
            }
            StartCoroutine(LoadInstantiated(loadedLevelDropdown.options[index].text));
            StartCoroutine(LoadMaterials(loadedLevelDropdown.options[index].text));
            StartCoroutine(LoadLines(loadedLevelDropdown.options[index].text));
        }
        else
        {      
            for (int i = 0; i < targetList.Count; i++)
            {
                if (!targetList[i].name.Contains("tree_main"))
                {
                    if (SaveGame.Exists("SavedAssets/" + changeScene + "/" + targetList[i].name + "Destroyed"))
                    {
                        changedObjectsDestroyed.Add(targetList[i]);
                        targetList[i].SetActive(false);
                    }
                }
                else
                {
                    if (SaveGame.Exists("SavedAssets/" + changeScene + "/" + targetList[i].transform.parent.name + "Destroyed"))
                    {
                        changedObjectsDestroyed.Add(targetList[i]);
                        targetList[i].SetActive(false);
                    }
                }
            }
            StartCoroutine(LoadInstantiated(changeScene));
            StartCoroutine(LoadMaterials(changeScene));
            StartCoroutine(LoadLines(changeScene));
            changeScene = null;
            changeSceneAuto = null;
        }
               
    } 
    public void LoadAutoSave()
    {
        if (changeSceneAuto == null)
        {
            int index = loadedLevelAutoDropdown.value;

            for (int i = 0; i < targetList.Count; i++)
            {
                if (SaveGame.Exists("Autosaves/" + loadedLevelAutoDropdown.options[index].text + "/" + targetList[i].name + "Destroyed"))
                {
                    changedObjectsDestroyed.Add(targetList[i]);
                    targetList[i].SetActive(false);
                }
            }
            StartCoroutine(LoadInstantiated(loadedLevelAutoDropdown.options[index].text));
            StartCoroutine(LoadMaterials(loadedLevelAutoDropdown.options[index].text));
            StartCoroutine(LoadLines(loadedLevelAutoDropdown.options[index].text));
            autoSave = false;
        }
        else
        {     
            for (int i = 0; i < targetList.Count; i++)
            {
                if (SaveGame.Exists("Autosaves/" + changeSceneAuto + "/" + targetList[i].name + "Destroyed"))
                {
                    changedObjectsDestroyed.Add(targetList[i]);
                    targetList[i].SetActive(false);
                }
            }
            StartCoroutine(LoadInstantiated(changeSceneAuto));
            StartCoroutine(LoadMaterials(changeSceneAuto));
            StartCoroutine(LoadLines(changeSceneAuto));
            autoSave = false;
            changeScene = null;
            changeSceneAuto = null;
        }
    }

    IEnumerator SaveData()
    {
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        MyData dataText = new MyData();
        
        if(!autoSave)
        {
            if (Directory.Exists(Application.persistentDataPath + "/Data"))
            {
                if (Directory.GetFiles(Application.persistentDataPath + "/Data") != null)
                {
                    string[] files = Directory.GetFiles(Application.persistentDataPath + "/Data");
                    for (int i = 0; i < files.Length; i++)
                    {
                        MyData data = SaveGame.Load<MyData>(files[i]);
                        if (data.inputFieldText != null)
                        {
                            dataStrings.Add(data.inputFieldText);
                        }
                    }
                }
            }
        }
        
        
        if (!dataStrings.Contains(inputField.text))
        {
            if (!autoSave)
            {
                dataText.inputFieldText = inputField.text;
                dataText.sceneString = sceneName;
                SaveGame.Save("Data/" + inputField.text + "_DataInputField", dataText);
            }
            else
            {
                inputField.text = "Autosave_" + timeDate;
                dataText.inputFieldText = inputField.text;
                dataText.sceneString = sceneName;                
                DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/" + "Data_Autosaves");
                FileInfo[] f = d.GetFiles("*", SearchOption.TopDirectoryOnly);
                
                if (f.Length < 5)
                {                    
                    SaveGame.Save("Data_Autosaves/" + inputField.text + "_DataInputField", dataText);
                }
                else
                {                    
                    string dirName = new DirectoryInfo(Application.persistentDataPath + "/Autosaves/").GetDirectories().OrderByDescending(x => x.LastWriteTime).Last().Name;
                    Directory.Delete(Application.persistentDataPath + "/Autosaves/" + dirName, true);                                                           
                    
                    foreach (var fi in new DirectoryInfo(Application.persistentDataPath + "/Data_Autosaves/").GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(4))
                        fi.Delete();                    
                    SaveGame.Save("Data_Autosaves/" + inputField.text + "_DataInputField", dataText);
                }
                areYouSureBool = true;
            }
        }
        else
        {
            areYouSure.SetActive(true);
            areYouSure.GetComponentInChildren<Text>().text = "Het bestand '" + inputField.text + "' bestaat al. Weet je zeker dat je het wilt vervangen?";            
            areYouSureBool = false;
        }
        dataStrings.Clear();        
        yield return null;
    }
   
   
    IEnumerator LoadInstantiated(string loadedInputFieldIns)
    {
        if (!autoSave)
        {
            
            while (SaveGame.Exists("SavedAssets/" + loadedInputFieldIns + "/" + instantiatedObjectsCount.ToString() + "Instantiate"))
            {
                GameObject go = null;
                MyData data = SaveGame.Load<MyData>("SavedAssets/" + loadedInputFieldIns + "/" + instantiatedObjectsCount.ToString() + "Instantiate");
                string newName = data.nameData.Replace("(Clone)", "");
                if (Resources.Load<GameObject>("ObjectPrefabs/" + newName) != null)
                {
                    go = Resources.Load<GameObject>("ObjectPrefabs/" + newName);
                }
                else
                {
                    go = Resources.Load<GameObject>("ObjectPrefabs/Vegetatie/" + newName);
                }
                GameObject current = Instantiate<GameObject>(go, data.position, Quaternion.Euler(data.rotation));
                current.transform.localScale = data.scale;
                changedObjects.Add(current);
                instantiatedObjectsCount++;
            }
        }
        else
        {            
            while (SaveGame.Exists("Autosaves/" + loadedInputFieldIns + "/" + instantiatedObjectsCount.ToString() + "Instantiate"))
            {
                GameObject go = null;
                MyData data = SaveGame.Load<MyData>("Autosaves/" + loadedInputFieldIns + "/" + instantiatedObjectsCount.ToString() + "Instantiate");
                string newName = data.nameData.Replace("(Clone)", "");
                if (Resources.Load<GameObject>("ObjectPrefabs/" + newName) != null)
                {
                    go = Resources.Load<GameObject>("ObjectPrefabs/" + newName);
                }
                else
                {
                    go = Resources.Load<GameObject>("ObjectPrefabs/Vegetatie/" + newName);
                }
                GameObject current = Instantiate<GameObject>(go, data.position, Quaternion.Euler(data.rotation));
                current.transform.localScale = data.scale;
                changedObjects.Add(current);
                instantiatedObjectsCount++;
            }
        }
        instantiatedObjectsCount = 0;
            
        yield return null;
    }
    IEnumerator LoadMaterials(string loadedInputFieldMat)
    {
        if (!autoSave)
        {
            while (SaveGame.Exists("SavedAssets/" + loadedInputFieldMat + "/" + materialCount.ToString() + "Material"))
            {
                MyData data = SaveGame.Load<MyData>("SavedAssets/" + loadedInputFieldMat + "/" + materialCount.ToString() + "Material");
                GameObject go = GameObject.Find(data.nameData);
                Material newMaterial = Resources.Load<Material>("Materials/" + data.materialName);
                go.GetComponent<Renderer>().sharedMaterial = newMaterial;                
                materialCount++;
            }
        }
        else
        {            
            while (SaveGame.Exists("Autosaves/" + loadedInputFieldMat + "/" + materialCount.ToString() + "Material"))
            {
                MyData data = SaveGame.Load<MyData>("Autosaves/" + loadedInputFieldMat + "/" + materialCount.ToString() + "Material");
                GameObject go = GameObject.Find(data.nameData);
                Material newMaterial = Resources.Load<Material>("Materials/" + data.materialName);
                go.GetComponent<Renderer>().sharedMaterial = newMaterial;                
                materialCount++;
            }
        }
        materialCount = 0;
        yield return null;
    }
    IEnumerator LoadLines(string loadedInputFieldLine)
    {
        if (!autoSave)
        {
            while (SaveGame.Exists("SavedAssets/" + loadedInputFieldLine + "/" + linesCount.ToString() + "Lines"))
            {
                GameObject go = new GameObject("Lines");
                MyData data = SaveGame.Load<MyData>("SavedAssets/" + loadedInputFieldLine + "/" + linesCount.ToString() + "Lines");
                LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
                lineRenderer.positionCount = data.positionLineRenderer.Length;
                lineRenderer.SetPositions(data.positionLineRenderer);
                lineRenderer.endWidth = data.widthEnd;
                lineRenderer.startWidth = data.widthStart;
                lineRenderer.material = Resources.Load<Material>("Materials/Painting/" + data.materialName);
                GameObject goFocus = new GameObject("focusLine");
                goFocus.transform.parent = go.transform;
                goFocus.transform.position = lineRenderer.GetPosition((int)Mathf.Round(lineRenderer.positionCount / 2));                
                changedLines.Add(goFocus);
                linesCount++;
            }
        }
        else
        {            
            while (SaveGame.Exists("Autosaves/" + loadedInputFieldLine + "/" + linesCount.ToString() + "Lines"))
            {
                GameObject go = new GameObject("Lines");
                MyData data = SaveGame.Load<MyData>("Autosaves/" + loadedInputFieldLine + "/" + linesCount.ToString() + "Lines");
                LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
                lineRenderer.positionCount = data.positionLineRenderer.Length;
                lineRenderer.SetPositions(data.positionLineRenderer);
                lineRenderer.endWidth = data.widthEnd;
                lineRenderer.startWidth = data.widthStart;
                lineRenderer.material = Resources.Load<Material>("Materials/Painting/" + data.materialName);
                changedLines.Add(go);
                linesCount++;
            }
        }
        linesCount = 0;
        yield return null;
    }
    

}



