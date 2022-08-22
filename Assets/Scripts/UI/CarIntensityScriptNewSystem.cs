using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;


public class CarIntensityScriptNewSystem : MonoBehaviour {

	public Text carsText, routeText;
	public Button button;
	private CarController carController;
	[SerializeField]
	private Slider sliderIntensity;
	[SerializeField]
	private Dropdown dropdownCars;		
	private WaypointsRoute[] route;	
	private UIScript uiScript;
	private GameObject[] allCars;
	private WaypointRouteManager[] routeManagers;	
	private List<string> routeManagersList = new List<string>();
	private bool setSliderValue = false;

	public void lateStart()
	{
		if (FindObjectOfType<CarController>() != null)
		{
			carController = FindObjectOfType<CarController>();
			routeManagers = carController.spawnRoutes;

			for (int i = 0; i < routeManagers.Length; i++)
			{
				routeManagersList.Add(routeManagers[i].name);
			}

			dropdownCars.ClearOptions();
			dropdownCars.AddOptions(routeManagersList);

			DropdownSelected();

			dropdownCars.onValueChanged.AddListener(delegate { DropdownSelected(); });

			sliderIntensity.onValueChanged.AddListener(delegate { OnSliderChange(); });

			setSliderValue = true;
		}
		else
		{
			carsText.text = "";
			setSliderValue = false;
		}
	}
	

	
	private void DropdownSelected()
    {
		int index = dropdownCars.value;

		routeText.text = "Route " + dropdownCars.options[index].text;

		GameObject waypointRoutemanagerObject = GameObject.Find(dropdownCars.options[index].text);

		WaypointRouteManager routeManager = waypointRoutemanagerObject.GetComponent<WaypointRouteManager>();

		float avarage = (routeManager.minDelay + routeManager.maxDelay) / 2;		

		carsText.text = math.round(1.0f / avarage * 3600.0f) + " MV/U";

		if (setSliderValue)
		{
			sliderIntensity.onValueChanged.RemoveAllListeners();
			sliderIntensity.value = routeManager.oldDelayMax - routeManager.maxDelay;
			sliderIntensity.onValueChanged.AddListener(delegate { OnSliderChange(); });
		}
	}	    

	private void OnSliderChange()
    {
		int index = dropdownCars.value;		

		GameObject waypointRoutemanagerObject = GameObject.Find(dropdownCars.options[index].text);

		WaypointRouteManager routeManager = waypointRoutemanagerObject.GetComponent<WaypointRouteManager>();

		if (routeManager.minDelay > 2.0f)
		{
			routeManager.minDelay = (routeManager.oldDelayMin - sliderIntensity.value);
			
		}	
		if(routeManager.minDelay < routeManager.maxDelay)
        {
			routeManager.maxDelay = (routeManager.oldDelayMax - sliderIntensity.value);
		}

		float avarage = (routeManager.minDelay + routeManager.maxDelay) / 2;

		carsText.text = math.round(1.0f / avarage * 3600.0f) + " MV/U";
	}
   
	
	public void OnValueChanged()
    {		
		route = FindObjectsOfType<WaypointsRoute>();		
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}		
	}
	public void ResetCars()
    {		
		route = FindObjectsOfType<WaypointsRoute>();
		allCars = GameObject.FindGameObjectsWithTag("CarPrefab");
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}
		for(int i = 0; i < allCars.Length; i++)
        {
			Destroy(allCars[i]);
        }
		uiScript = FindObjectOfType<UIScript>();
		uiScript.ActivateForthTask();
	}
}
