using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class SaveAndLoad : MonoBehaviour
{    
    private UIScript uiScript;
    [SerializeField]
    private Dropdown dropdownAssets, dropdownAuto;
    private Dropdown dropdownAssetsOld, dropdownAutoOld;
    private GameDataHandler gameDataHandler;  
    

    
    public void AssignDropdowns()
    {
        gameDataHandler = FindObjectOfType<GameDataHandler>();
        dropdownAssetsOld = gameDataHandler.loadedLevelDropdown;
        dropdownAutoOld = gameDataHandler.loadedLevelAutoDropdown;
        gameDataHandler.loadedLevelAutoDropdown = dropdownAuto;
        gameDataHandler.loadedLevelDropdown = dropdownAssets;
        

    }
    public void GoVR()
    {
        gameDataHandler.loadedLevelAutoDropdown = dropdownAutoOld;
        gameDataHandler.loadedLevelDropdown = dropdownAssetsOld;
        uiScript = FindObjectOfType<UIScript>();
        uiScript.go3Dbool = true;
        SceneLoader.Instance.LoadNewScene("Menu");        
    }

    public void OnApplicationClose()
    {
        Application.Quit();
    }

    public void DeleteDirectoryAuto()
    {
        if (Directory.GetFiles(Application.persistentDataPath + "/Data_Autosaves").Length > 0)
        {
            int indexAuto = dropdownAuto.value;
            File.Delete(Application.persistentDataPath + "/Data_Autosaves/" + dropdownAuto.options[indexAuto].text + "_DataInputField");
            Directory.Delete(Application.persistentDataPath + "/Autosaves/" + dropdownAuto.options[indexAuto].text, true);
            gameDataHandler.LoadStrings();
        }
    }
    public void DeleteDirectoryAssets()
    {
        if (Directory.GetFiles(Application.persistentDataPath + "/Data").Length > 0)
        {
            int index = dropdownAssets.value;
            File.Delete(Application.persistentDataPath + "/Data/" + dropdownAssets.options[index].text + "_DataInputField");
            Directory.Delete(Application.persistentDataPath + "/SavedAssets/" + dropdownAssets.options[index].text, true);
            gameDataHandler.LoadStrings();
        }
    }
    public void AssignDropdownsBack()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Data"))
        {
            if (Directory.GetFiles(Application.persistentDataPath + "/Data").Length > 0)
            {
                int index = dropdownAssets.value;
                gameDataHandler.changeScene = dropdownAssets.options[index].text;

            }
        }
        if (Directory.Exists(Application.persistentDataPath + "/Data_Autosaves"))
        {

            if (Directory.GetFiles(Application.persistentDataPath + "/Data_Autosaves").Length > 0)
            {
                int indexAuto = dropdownAuto.value;
                gameDataHandler.changeSceneAuto = dropdownAuto.options[indexAuto].text;
            }
        }
        
        
        gameDataHandler.loadedLevelAutoDropdown = dropdownAutoOld;
        gameDataHandler.loadedLevelDropdown = dropdownAssetsOld;
        uiScript = FindObjectOfType<UIScript>();
        uiScript.go3Dbool = true;
    }
    

    public void LoadingAssets()
    {
        uiScript = FindObjectOfType<UIScript>();
        uiScript.mainCamera.SetActive(false);
        uiScript.camera3D.SetActive(true);
        SceneLoader.Instance.LoadAssets();
    }
    public void LoadingAuto()
    {
        uiScript = FindObjectOfType<UIScript>();
        uiScript.mainCamera.SetActive(false);
        uiScript.camera3D.SetActive(true);
        SceneLoader.Instance.LoadAutoSave();
    }
}
