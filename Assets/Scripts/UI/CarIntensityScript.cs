using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarIntensityScript : MonoBehaviour {


	public UnityEngine.UI.Slider sliderCars;	
	public UnityEngine.UI.Text carsText;
	public UnityEngine.UI.Button button;
	private TurnTheGameOn.SimpleTrafficSystem.AITrafficController trafficController;
	private float carIntensity;
	private WaypointsRoute[] route;	
	private UIScript uiScript;

	public void Start()
	{		
		carIntensity = sliderCars.value;

		if (FindObjectOfType<TurnTheGameOn.SimpleTrafficSystem.AITrafficController>() != null)
		{			
			trafficController = FindObjectOfType<TurnTheGameOn.SimpleTrafficSystem.AITrafficController>();
			carIntensity = trafficController._AITrafficPool.density;
			button.onClick.AddListener(delegate { trafficController.MoveAllCarsToPool(); });
			sliderCars.onValueChanged.AddListener(delegate { trafficController.MoveAllCarsToPool(); });
		}

		carsText.text = trafficController._AITrafficPool.density * 8 + " mv/u";

	}
		
	
	public void ChangeCarSlider () 
	{
		carIntensity = sliderCars.value;		
		trafficController._AITrafficPool.density = (int) carIntensity;
		carsText.text = trafficController._AITrafficPool.density * 8 + " mv/u";
	}
	
	public void OnValueChanged()
    {
		trafficController = FindObjectOfType<TurnTheGameOn.SimpleTrafficSystem.AITrafficController>();
		carIntensity = trafficController._AITrafficPool.density;
		sliderCars.onValueChanged.AddListener(delegate { trafficController.MoveAllCarsToPool(); });
		route = FindObjectsOfType<WaypointsRoute>();		
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}		
	}
	public void ResetCars()
    {
		trafficController = FindObjectOfType<TurnTheGameOn.SimpleTrafficSystem.AITrafficController>();
		carIntensity = trafficController._AITrafficPool.density;
		button.onClick.AddListener(delegate { trafficController.MoveAllCarsToPool(); });
		route = FindObjectsOfType<WaypointsRoute>();		
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}
		for (int i = 0; i < route.Length; i++)
		{
			route[i].stopCycling = false;
		}
		uiScript = FindObjectOfType<UIScript>();
		uiScript.ActivateForthTask();
	}
}
