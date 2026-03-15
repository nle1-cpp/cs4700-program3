#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class LoadScene : MonoBehaviour
{
    static LoadScene()
    {
        //Open the Scene in the Editor (do not enter Play Mode)
        EditorApplication.delayCall += () =>
				EditorSceneManager.OpenScene("Assets/Scenes/Playfield.unity");
    }
}
#endif
