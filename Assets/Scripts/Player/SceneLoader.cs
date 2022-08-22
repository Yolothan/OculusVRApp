using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.Mathematics;
using BayatGames.SaveGameFree;


public class SceneLoader : Singleton<SceneLoader>
{    
    public ScreenFader screenFaderVR = null, screenFader3D = null;
    public VideoPlayer videoPlayerVR, videoPlayer3D;
    public GameObject canvasVideoPlayerVR, canvasVideoPlayer3D, player, wristUI;
    public Slider sliderVR, slider3D;
    public Text progressTextVR, progressText3D;
    public UIScript uiScript;
    public GameDataHandler gameDataHandler;
    public Dropdown loadedLevelDropdown, loadedLevelAutoDropdown;    
    private SaveAndLoad saveLoad;
    
    [HideInInspector]
    public bool isLoading = false, loadAssets = false, loadAutoSave = false;
    private void Awake()
    {
        canvasVideoPlayerVR.SetActive(false);
        canvasVideoPlayer3D.SetActive(false);
        SceneManager.sceneLoaded += SetActiveScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;        
    }

    public void LoadAssets()
    {        
        int index = loadedLevelDropdown.value;
        if (SaveGame.Exists("Data/" + loadedLevelDropdown.options[index].text + "_DataInputField"))
        {
            MyData data = SaveGame.Load<MyData>("Data/" + loadedLevelDropdown.options[index].text + "_DataInputField");
            string sceneName = data.sceneString;
            loadAssets = true;
            uiScript.GameActivate();
            uiScript.LoadAssets();
            if (uiScript.camera3D.activeSelf)
            {
                uiScript.goInThreeD = true;
            }
            
            LoadNewScene(sceneName);
        }
        
    }
    public void LoadAutoSave()
    {
        int index = loadedLevelAutoDropdown.value;
        if (SaveGame.Exists("Data_Autosaves/" + loadedLevelAutoDropdown.options[index].text + "_DataInputField"))
        {
            MyData data = SaveGame.Load<MyData>("Data_Autosaves/" + loadedLevelAutoDropdown.options[index].text + "_DataInputField");
            string sceneName = data.sceneString;
            loadAutoSave = true;
            uiScript.GameActivate();
            uiScript.LoadAssets();
            gameDataHandler.autoSave = true;
            if (uiScript.camera3D.activeSelf)
            {
                uiScript.goInThreeD = true;
            }
            
            LoadNewScene(sceneName);
        }
        
    }

    public void LoadNewScene(string sceneName)
    {        
        if (!isLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }        
    }

    private IEnumerator LoadScene(string sceneName)
    {
        isLoading = true;
        gameDataHandler.ClearAllLists();
        if (sceneName != "Tutorial")
        {
            uiScript.DeactivateInTutorial();
        }
        if (!uiScript.testing)
        {
            if (sceneName == "Menu")
            {
                if(GameObject.Find("StartScreen") != null)
                {
                    GameObject.Find("StartScreen").SetActive(false);
                }
                uiScript.InstantiateWeatherPrefab();
                uiScript.buttonController.enabled = true;
                uiScript.snapTurn.enabled = true;
            }
        }
        
        if(sceneName == "MenuComputer" && uiScript.goInThreeD)
        {
            FlyController flyController = uiScript.camera3D.GetComponent<FlyController>();
            flyController.enabled = false;
        }
        else
        {
            FlyController flyController = uiScript.camera3D.GetComponent<FlyController>();
            flyController.enabled = true;
        }

        uiScript.DeactivateInScene1();
        uiScript.DeactivateTeleport();
        
        if (uiScript.goInThreeD)
        {            
            uiScript.mainCamera.SetActive(false);
            uiScript.camera3D.SetActive(true);
            uiScript.canvasVR.SetActive(false);
            uiScript.canvas3D.SetActive(true);
            yield return screenFader3D.StartFadeIn();
            
            canvasVideoPlayer3D.SetActive(true);
            slider3D.value = 0;
            progressText3D.text = "0%";
            videoPlayer3D.Play();            
        }
        else
        {            
            uiScript.mainCamera.SetActive(true);
            uiScript.camera3D.SetActive(false);
            uiScript.canvasVR.SetActive(true);
            uiScript.canvas3D.SetActive(false);
            yield return screenFaderVR.StartFadeIn();
            
            canvasVideoPlayerVR.SetActive(true);
            sliderVR.value = 0;
            progressTextVR.text = "0%";
            videoPlayerVR.Play();
        }
        
        yield return StartCoroutine(UnloadCurrent());
                
        yield return new WaitForSeconds(3.0f);

        yield return StartCoroutine(LoadNew(sceneName));
        if (uiScript.goInThreeD)
        {            
            canvasVideoPlayer3D.SetActive(false);
            yield return screenFader3D.StartFadeOut();            
        }
        else
        {
            canvasVideoPlayerVR.SetActive(false);
            yield return screenFaderVR.StartFadeOut();
        }
        
        
        if (sceneName != "Menu")
        {
            uiScript.ActivateTeleportRay();
        }
        if(sceneName == "Tutorial")
        {
            uiScript.ActiveInTutorial();
            uiScript.DeactivateTeleport();            
        }

        if (sceneName == "Hazerswoude")
        {
            uiScript.minimapSpawnHazerswoude.SetActive(true);
            uiScript.minimapSpawnLonneker.SetActive(false);
            uiScript.cylinderHazerswoude.SetActive(true);
            uiScript.cylinderLonneker.SetActive(false);
        }
        else if (sceneName == "Lonneker")
        {
            uiScript.minimapSpawnHazerswoude.SetActive(false);
            uiScript.minimapSpawnLonneker.SetActive(true);
            uiScript.cylinderHazerswoude.SetActive(false);
            uiScript.cylinderLonneker.SetActive(true);
        }

        if (uiScript.goInThreeD)
        {           
            saveLoad = FindObjectOfType<SaveAndLoad>();
            saveLoad.AssignDropdowns();
            gameDataHandler.LoadStrings();
            uiScript.goInThreeD = false;           
        }

        if (loadAssets)
        {                     
            gameDataHandler.Load();

            loadAssets = false;
        }
        if(loadAutoSave)
        {                       
            gameDataHandler.LoadAutoSave();

            loadAutoSave = false;
        }
        

            isLoading = false;        

		yield return null;
    }

    private IEnumerator UnloadCurrent()
    {
        if (SceneManager.GetActiveScene().name != "Persistent")
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            while (!unloadOperation.isDone)

                yield return null;
        }
    }

    private IEnumerator LoadNew(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / .9f);
            if (!uiScript.goInThreeD)
            {
                sliderVR.value = progress;
                progressTextVR.text = math.round((double)(progress * 100)) + "%";
            }
            else
            {
                slider3D.value = progress;
                progressText3D.text = math.round((double)(progress * 100)) + "%";
            }

            yield return null;
        }
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }   

    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    } 
}

