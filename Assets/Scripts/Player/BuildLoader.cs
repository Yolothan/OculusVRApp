using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildLoader : MonoBehaviour
{
    private void Awake()
    {        
            LoadPersistent();
    }

    private void LoadPersistent()
    {
        SceneManager.LoadSceneAsync("StartScreen", LoadSceneMode.Additive);
    }
}
