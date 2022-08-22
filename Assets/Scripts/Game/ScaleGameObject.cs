using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ScaleGameObject : MonoBehaviour
{
    private GameObject controllerObject;
    private XRController controller;
    
    [SerializeField]
    private float scaleSpeed = 0.1f;


    private void Start()
    {
        if (GameObject.Find("LeftHandController") != null)
        {
            controllerObject = GameObject.Find("LeftHandController");
            controller = controllerObject.GetComponent<XRController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            {
                if ((position.y > position.x && position.y > 0.0f) || (position.y < position.x && position.y < 0.0f))
                {
                    if (position.y > 0.0f || position.y < 0.0f)
                    {
                        if (gameObject.transform.localScale.x >= 0.1f && gameObject.transform.localScale.x <= 10.0f)
                        {
                            Vector3 scale = new Vector3(gameObject.transform.localScale.x + (position.y * scaleSpeed),
                                gameObject.transform.localScale.y + (position.y * scaleSpeed), gameObject.transform.localScale.z + (position.y * scaleSpeed));
                            gameObject.transform.localScale = scale;
                        }
                        else if (gameObject.transform.localScale.x > 10.0f)
                        {
                            Vector3 scale = new Vector3(10.0f, 10.0f, 10.0f);
                            gameObject.transform.localScale = scale;
                        }
                        else if (gameObject.transform.localScale.x < 0.1f)
                        {
                            Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
                            gameObject.transform.localScale = scale;
                        }
                    }
                }
            }
        }

    }
}
