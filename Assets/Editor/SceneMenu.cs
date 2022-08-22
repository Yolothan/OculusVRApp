using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneMenu
{
    [MenuItem("Scenes/StartScreen")]
    public static void OpenMenu()
    {
        OpenScene("StartScreen");
    }

    public static void OpenGame()
    {
        OpenScene("Game");
    }

    private static void OpenScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity", OpenSceneMode.Additive);
    }
}