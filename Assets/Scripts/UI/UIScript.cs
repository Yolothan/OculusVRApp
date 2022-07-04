using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class UIScript : MonoBehaviour
{
    [SerializeField]
    GameObject panel, menuPanel, keyBoard, areYouSureSave, savePanel, loadPanel, loadAutoSavePanel, drivingUI, weatherIcon, homeIcon, drivingIcon, leftHandPresence,
        mapUI, mapUIIcon, minimapSpawnTutorial, leftHandController, rightHandController, leftControllerPrefab, rightControllerPrefab,
        indicatorSphere1, indicatorSphere2, indicatorSphere3, indicatorSphere4, indicatorSphereUI1, indicatorSphereUI2, indicatorSphereUI3,
        indicatorSphereUI4, indicatorSphereUI5, indicatorSphereUI6, indicatorSphereUI7, allIndicators, weatherPrefab, heightUI, spawnObjectsUI, newSituationUI;
    public GameObject weatherUI, wristUI, minimapSpawnLonneker, minimapSpawnHazerswoude, canvasVR, canvas3D, camera3D, mainCamera, cylinderLonneker, cylinderHazerswoude;
    public Button weatherButton;
    public HoldRaycasts holdRaycasts;
    public EnviroSamples.DemoUI demoUI;
    public SceneLoader sceneLoader;    
    public CharacterController characterController;
    public Zenva.VR.ButtonController buttonController;
    public DeviceBasedSnapTurnProvider snapTurn;    
    public XRController leftXRController, rightXRController;
    public XRRayInteractor leftRayInteractor, rightRayInteractor;
    public LineRenderer leftLineRenderer, rightLineRenderer;
    public XRInteractorLineVisual leftLineVisual, rightLineVisual;
    public HandPresence leftHandPresenceScript, rightHandPresenceScript;
    public CarIntensityScriptNewSystem carIntensity;
    public MeshRenderer cylinderL, cylinderR;
    public bool testing, testingTutorial, tutorialIncluded = false;    
    private BlinkingMaterial blinkMat1, blinkMat2, blinkMat3, blinkMat4, blinkMatUI1, blinkMatUI2, blinkMatUI3, blinkMatUI4, blinkMatUI5, blinkMatUI6, blinkMatUI7;
    private Button weatherUIButton, drivingUIButton, mapUIButton, homeButton;
    private bool invokeOnce = true, allowedOpenWeatherText = true, allowedOpenDrivingUI = true, allowDrivingCarUI = true,
        allowedOpenMap = true, allowedForthTask = true, allowedOpenMap2 = true, allowedGoHome = true;
    private GameObject ui_Assistant, routeX, routeY;
    [HideInInspector]
    public bool allowedDeactivateTrigger = false, allowedDeactivateButton = false, allowInstantiate = true, go3Dbool = true;
    public bool goInThreeD = false;
    private UI_Assistant assistant;
    private Vector3 initialPosition, initialScale;  
    private TutorialUI uiTutorial;    
   

    private void Start()
    {        
        camera3D.SetActive(false);
        canvas3D.SetActive(false);
        wristUI.SetActive(false);
            weatherUI.SetActive(false);
            drivingUI.SetActive(false);
            mapUI.SetActive(false);        
        loadPanel.SetActive(false);
        newSituationUI.SetActive(false);
        heightUI.SetActive(false);
        savePanel.SetActive(false);
        loadAutoSavePanel.SetActive(false);
        spawnObjectsUI.SetActive(false);
        keyBoard.SetActive(false);
        areYouSureSave.SetActive(false);
        indicatorSphere1.SetActive(false);
        indicatorSphere2.SetActive(false);
        indicatorSphere3.SetActive(false);
        indicatorSphere4.SetActive(false);
        indicatorSphereUI1.SetActive(false);
        indicatorSphereUI2.SetActive(false);
        indicatorSphereUI3.SetActive(false);
        indicatorSphereUI4.SetActive(false);
        indicatorSphereUI5.SetActive(false);
        indicatorSphereUI6.SetActive(false);
        indicatorSphereUI7.SetActive(false);
        menuPanel.SetActive(false);
        if (!testing && tutorialIncluded)
        {
            characterController.enabled = false;
            buttonController.enabled = false;
            snapTurn.enabled = false;
            leftHandController.SetActive(false);
            rightHandController.SetActive(false);
            leftControllerPrefab.SetActive(false);
            rightControllerPrefab.SetActive(false);
            DeactivateTeleport();
        }
        
        if(testingTutorial)
        {
            ActiveInTutorial();
        }       
        if(!tutorialIncluded)
        {
            buttonController.enabled = false;
            snapTurn.enabled = false;
            DeactivateTeleport();
            Destroy(allIndicators);
        }
    }
    public void EnableRaycasts()
    {   
        for (int i = 0; i < holdRaycasts.raycasts.Length; i++)
        {
            holdRaycasts.raycasts[i].enabled = true;
        }
    }
    public void LoadAssets()
    {
        wristUI.SetActive(false);
        weatherUI.SetActive(false);
        drivingUI.SetActive(false);
        mapUI.SetActive(false);
        loadPanel.SetActive(false);
        newSituationUI.SetActive(false);
        heightUI.SetActive(false);
        savePanel.SetActive(false);
        loadAutoSavePanel.SetActive(false);
        spawnObjectsUI.SetActive(false);
        keyBoard.SetActive(false);
        areYouSureSave.SetActive(false);
    }
    
    public void ApplicationQuit()
    {
        Application.Quit();        
    }

    public void ReturnHome(string sceneToLoad)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            
            MenuActivateTutorial();
            SetAllFalse();
            DeactivateTeleport();
            
        }
        else
        {
            MenuActivate();
            DeactivateTeleport();
        }

        sceneLoader.LoadNewScene(sceneToLoad);
        allowInstantiate = true;
    }
    

    public void ToggleOnOff()
    {
        wristUI.SetActive(!wristUI.activeSelf);
        if(wristUI.activeSelf)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }       
    }
    

    public void UIFunction() 
    {        
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
            weatherUI.SetActive(false);            
            drivingUI.SetActive(false); 
            mapUI.SetActive(false);
            loadPanel.SetActive(false);
            if(areYouSureSave.activeSelf)
            {
                GameDataHandler handler = FindObjectOfType<GameDataHandler>();
                handler.Nee();
                areYouSureSave.SetActive(false);
            }
            loadAutoSavePanel.SetActive(false);
            heightUI.SetActive(false);
            savePanel.SetActive(false);
            spawnObjectsUI.SetActive(false);
            keyBoard.SetActive(false);
            newSituationUI.SetActive(false);
            menuPanel.SetActive(false);
        }
    }

    public void ActiveInScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;        

        if (sceneName == "Menu")
        {
            weatherIcon.SetActive(false);
            homeIcon.SetActive(false);
            drivingIcon.SetActive(false);
            mapUIIcon.SetActive(false);
            leftHandPresenceScript.handModelPrefab = leftHandPresenceScript.firstHandModel;
            rightHandPresenceScript.handModelPrefab= rightHandPresenceScript.firstHandModel;
        }
        else
        {
            weatherIcon.SetActive(true);
            homeIcon.SetActive(true);
            drivingIcon.SetActive(true);
            mapUIIcon.SetActive(true);
        }
    }
    public void ActiveInTutorial()
    {
        leftHandPresenceScript.handModelPrefab = leftControllerPrefab;
        rightHandPresenceScript.handModelPrefab = rightControllerPrefab;
        leftHandPresenceScript.spawnedHandModel.SetActive(false);
        rightHandPresenceScript.spawnedHandModel.SetActive(false);
        minimapSpawnTutorial.SetActive(true);
        leftHandController.SetActive(true);
        rightHandController.SetActive(true);
        leftControllerPrefab.SetActive(true);
        rightControllerPrefab.SetActive(true);
        characterController.enabled = true;
        minimapSpawnHazerswoude.SetActive(false);
        minimapSpawnLonneker.SetActive(false);
    }
    public void ActivateFirstTask()
    {
        indicatorSphere1.SetActive(true);
        indicatorSphere3.SetActive(true);
        blinkMat1 = indicatorSphere1.GetComponent<BlinkingMaterial>();
        blinkMat1.StartFading();
        blinkMat3 = indicatorSphere3.GetComponent<BlinkingMaterial>();
        blinkMat3.StartFading();
        ActivateTeleportRay();
    }

    public void ActivateSecondTask()
    {
        indicatorSphere1.SetActive(false);
        indicatorSphere3.SetActive(false);
        indicatorSphere2.SetActive(true);
        blinkMat2 = indicatorSphere2.GetComponent<BlinkingMaterial>();
        blinkMat2.StartFading();
        buttonController.enabled = true;        
    }
    public void ActivateThirdTask()
    {
        if (invokeOnce)
        {
            indicatorSphere2.SetActive(false);

            indicatorSphereUI6.SetActive(true);
            blinkMatUI6 = indicatorSphereUI6.GetComponent<BlinkingMaterial>();
            blinkMatUI6.StartFading();

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Klik op de pijl om terug te gaan naar het menu");
            invokeOnce = false;
        }
        

    }

    public void ActivateUIThirdTask()
    {
        if (allowedOpenDrivingUI)
        {
            indicatorSphereUI6.SetActive(false);

            indicatorSphereUI2.SetActive(true);
            blinkMatUI2 = indicatorSphereUI2.GetComponent<BlinkingMaterial>();
            blinkMatUI2.StartFading();

            weatherUIButton = weatherIcon.GetComponent<Button>();
            weatherUIButton.enabled = false;

            drivingUIButton = drivingIcon.GetComponent<Button>();
            drivingUIButton.enabled = true;

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Klik op het stuur om het menu voor het verkeer te openen");

            allowedOpenDrivingUI = false;
        }
    }

    public void ActivateUIThirdTaskCar()
    {
        if (allowDrivingCarUI)
        {
            indicatorSphereUI2.SetActive(false);

            uiTutorial = FindObjectOfType<TutorialUI>();

            routeX = uiTutorial.routeX;
            routeY = uiTutorial.routeY;

            routeX.SetActive(true);
            routeY.SetActive(true);

            indicatorSphereUI4.SetActive(true);
            blinkMatUI4 = indicatorSphereUI4.GetComponent<BlinkingMaterial>();
            blinkMatUI4.StartFading();

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Druk op de 'reset-knop' om al het verkeer te resetten. Dit kan handig zijn voor als het verkeer vastloopt");

            allowDrivingCarUI = false;
        }
    }


    public void ActivateUISecondTask()
    {
        if (allowDrivingCarUI && tutorialIncluded)
        {
            indicatorSphere2.SetActive(false);

            indicatorSphereUI1.SetActive(true);
            blinkMatUI1 = indicatorSphereUI1.GetComponent<BlinkingMaterial>();
            blinkMatUI1.StartFading();

            mapUIButton = mapUIIcon.GetComponent<Button>();
            mapUIButton.enabled = false;

            drivingUIButton = drivingIcon.GetComponent<Button>();
            drivingUIButton.enabled = false;

            homeButton = homeIcon.GetComponent<Button>();
            homeButton.enabled = false;

            
                assistant = FindObjectOfType<UI_Assistant>();
                assistant.WriteText("Klik op het weer om het weermenu te openen");
                allowedOpenWeatherText = true;               

            buttonController.enabled = false;

            allowedDeactivateButton = false;
        }
    }
    public void AllowedOpenWeatherText()
    {
        if(allowedOpenWeatherText && tutorialIncluded)
        {
            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Laat het regenen!");
            allowedOpenWeatherText = false;
        }
    }

    public void SetAllFalse()
    {
        allowedOpenWeatherText = false;
        allowedDeactivateButton = false;
        allowedOpenDrivingUI = false;
        invokeOnce = false;
        allowedForthTask = false;
        allowedOpenMap = false;
        allowedGoHome = false;
        allowedDeactivateTrigger = false;
        allowDrivingCarUI = false;
        allowedOpenMap2 = false;
        
    }

    public void ActivateForthTask()
    {
        if (allowedForthTask)
        {

            indicatorSphereUI4.SetActive(false);

            indicatorSphereUI5.SetActive(true);
            blinkMatUI5 = indicatorSphereUI5.GetComponent<BlinkingMaterial>();
            blinkMatUI5.StartFading();

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Druk weer op de pijl om terug te gaan naar het menu");

            allowedForthTask = false;
        }
    }

    public void ActivateForthTaskMap()
    {
        if (allowedOpenMap)
        {
            GameObject assistantCanvas;
            assistantCanvas = GameObject.Find("AssistantCanvas");

            initialPosition = new Vector3(assistantCanvas.transform.localPosition.x, assistantCanvas.transform.localPosition.y, assistantCanvas.transform.localPosition.z);
            initialScale = new Vector3(assistantCanvas.transform.localScale.x, assistantCanvas.transform.localScale.y, assistantCanvas.transform.localScale.z);

            indicatorSphereUI5.SetActive(false);

            indicatorSphereUI3.SetActive(true);
            blinkMatUI3 = indicatorSphereUI3.GetComponent<BlinkingMaterial>();
            blinkMatUI3.StartFading();

            mapUIButton = mapUIIcon.GetComponent<Button>();
            mapUIButton.enabled = true;

            drivingUIButton = drivingIcon.GetComponent<Button>();
            drivingUIButton.enabled = false;

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Druk op de kaart om de minimap te openen");

            allowedOpenMap = false;
        }
    }

    public void OpenMap()
    {
        if(allowedOpenMap2)
        {
            GameObject assistantCanvas;                     
            assistantCanvas = GameObject.Find("AssistantCanvas");
            assistantCanvas.transform.localPosition = new Vector3(-0.17f, 1.55f, 3.7f);
            assistantCanvas.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Teleporteer naar de blauwe stip op de map");

            allowedOpenMap2 = false;
        }
    }

    public void ActivateFifthTask()
    {
        if (allowedGoHome)
        {
            minimapSpawnHazerswoude.SetActive(true);
            minimapSpawnLonneker.SetActive(true);

            GameObject assistantCanvas;
            assistantCanvas = GameObject.Find("AssistantCanvas");

            assistantCanvas.transform.localPosition = initialPosition;
            assistantCanvas.transform.localScale = initialScale;

            mapUIButton = mapUIIcon.GetComponent<Button>();
            mapUIButton.enabled = false;

            homeButton = homeIcon.GetComponent<Button>();
            homeButton.enabled = true;

            indicatorSphere4.SetActive(false);

            indicatorSphereUI7.SetActive(true);
            blinkMatUI7 = indicatorSphereUI7.GetComponent<BlinkingMaterial>();
            blinkMatUI7.StartFading();

            assistant = FindObjectOfType<UI_Assistant>();
            assistant.WriteText("Druk op het huisje om de tutorial af te sluiten en naar het menu van de applicatie te gaan");

            

            allowedGoHome = false;

            
        }
    }

    public void ActivateFifthTaskArrow()
    {
        indicatorSphere4.SetActive(true);
        blinkMat4 = indicatorSphere4.GetComponent<BlinkingMaterial>();
        blinkMat4.StartFading();

        assistant = FindObjectOfType<UI_Assistant>();
        assistant.WriteText("Druk weer op de pijl om terug te gaan naar het menu");
    }
    

    public void DeactivateInTutorial()
    {
        Destroy(minimapSpawnTutorial);
        minimapSpawnLonneker.SetActive(true);
        minimapSpawnHazerswoude.SetActive(true);
    }


    public void DeactivateInScene1()
    {        
        wristUI.SetActive(false);
        leftHandPresence.SetActive(true);              
    }    

    public void Deactivate()
    {
        
        leftHandPresence.SetActive(false);        
        
    }
    public void Activate()
    {
        leftHandPresence.SetActive(true);            
    }       

    public void MenuActivateTutorial()
    {
        GameObject assistantCanvas;
        assistantCanvas = GameObject.Find("AssistantCanvas");
        Destroy(assistantCanvas);

        characterController.enabled = true;
        buttonController.enabled = true;

        leftControllerPrefab.SetActive(false);
        rightControllerPrefab.SetActive(false);

        leftHandController.SetActive(true);
        rightHandController.SetActive(true);

        leftHandPresenceScript.firstHandModel.SetActive(true);
        rightHandPresenceScript.firstHandModel.SetActive(true);

        mapUIButton = mapUIIcon.GetComponent<Button>();
        mapUIButton.enabled = true;

        weatherUIButton = weatherIcon.GetComponent<Button>();
        weatherUIButton.enabled = true;

        drivingUIButton = drivingIcon.GetComponent<Button>();
        drivingUIButton.enabled = true;

        if (GameObject.Find("Enviro Sky Manager (1)") != null)
        {
            wristUI.SetActive(true);
            weatherUI.SetActive(true);

            demoUI.SetWeatherID(0);
            demoUI.SetSeason(1);

            weatherUI.SetActive(false);
            wristUI.SetActive(false);
        }

        Destroy(allIndicators);
    }
    public void MenuActivate()
    {
        characterController.enabled = true;
        buttonController.enabled = true;
        leftHandController.SetActive(true);
        rightHandController.SetActive(true);
    }

    public void GameActivate()
    {
        ActivateTeleportRay();
        snapTurn.enabled = true;
    }

    public void DeactivateTeleport()
    {
        leftXRController.enabled = false;
        rightXRController.enabled = false;
        leftRayInteractor.enabled = false;
        rightRayInteractor.enabled = false;
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        leftLineVisual.enabled = false;
        rightLineVisual.enabled = false;
        cylinderL.enabled = false;
        cylinderR.enabled = false;
        
    }
    
    public void ActivateTeleportRay()
    {
        leftXRController.enabled = true;
        rightXRController.enabled = true;
        leftRayInteractor.enabled = true;
        rightRayInteractor.enabled = true;
        leftLineRenderer.enabled = true;
        rightLineRenderer.enabled = true;
        leftLineVisual.enabled = true;
        rightLineVisual.enabled = true;
        cylinderR.enabled = true;
        cylinderL.enabled = true;
    }

    public void DeactivateUI()
    {
        if (GameObject.Find("UI_Assistant") != null && allowedDeactivateTrigger)
        {
            ui_Assistant = GameObject.Find("UI_Assistant");
            ui_Assistant.SetActive(false);
            allowedDeactivateTrigger = false;
        }
    }

    public void InstantiateWeatherPrefab()
    {
        if (allowInstantiate && tutorialIncluded)
        {
            demoUI.SetWeatherID(0);
            demoUI.SetSeason(1);
            GameObject go = Instantiate(weatherPrefab);
            go.transform.parent = wristUI.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            Vector3 rotation = new Vector3(9.346f, -1.976f, 1.217f);
            go.transform.localRotation = Quaternion.Euler(rotation);
            Destroy(weatherUI);
            weatherUI = go;
            weatherButton.onClick.RemoveAllListeners();
            weatherButton.onClick.AddListener(delegate { weatherUI.SetActive(true); });
            weatherButton.onClick.AddListener(delegate { panel.SetActive(false); });
            demoUI = weatherUI.GetComponent<EnviroSamples.DemoUI>();
            Button returnButton = demoUI.returnButton;
            returnButton.onClick.AddListener(delegate { panel.SetActive(true); });

            go.SetActive(false);
        }
        if(!tutorialIncluded)
        {
            GameObject go = Instantiate(weatherPrefab);
            go.transform.parent = wristUI.transform;
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            Vector3 rotation = new Vector3(9.346f, -1.976f, 1.217f);
            go.transform.localRotation = Quaternion.Euler(rotation);
            Destroy(weatherUI);
            weatherUI = go;
            weatherButton.onClick.AddListener(delegate { weatherUI.SetActive(true); });
            demoUI = weatherUI.GetComponent<EnviroSamples.DemoUI>();
            Button returnButton = demoUI.returnButton;
            returnButton.onClick.AddListener(delegate { panel.SetActive(true); });
            weatherUI.SetActive(false);
        }                  
    }
    public void Update()
    {       
        if (Keyboard.current[Key.Space].wasPressedThisFrame && go3Dbool && !sceneLoader.isLoading)
        {
            LoadAssets();
            goInThreeD = true;            
            SceneLoader.Instance.LoadNewScene("MenuComputer");   
            go3Dbool = false;            
        }
    }
}


