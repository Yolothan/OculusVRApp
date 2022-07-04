using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonObject : MonoBehaviour
{    
    private GameObject wristUI;
    private UIScript handlingScene;

    
    public void LoadGame(string SceneName)         
    {
        handlingScene = FindObjectOfType<UIScript>();
        handlingScene.GameActivate();
        
        if (GameObject.Find("WristUI") != null)
        {
            wristUI = GameObject.Find("WristUI");
            wristUI.SetActive(false);
        }
        SceneLoader.Instance.LoadNewScene(SceneName);
        
    }    
}
