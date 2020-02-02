using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSelect : MonoBehaviour
{

    public static string JsonToLoad;
    public static int MaxLevel;

    public string[] Levels;

    // Start is called before the first frame update
    public void SelectLevel1()
    {
        JsonToLoad = "Assets/Json/vase_broken.json";
        SceneManager.LoadScene("LevelLoader");
    }

    public void SelectLevel2()
    {
        JsonToLoad = "Assets/Json/time_broken.json";
        SceneManager.LoadScene("LevelLoader");
    }

    public void SelectLevel3()
    {
        JsonToLoad = "Assets/Json/kite_broken.json";
        SceneManager.LoadScene("LevelLoader");
    }

    public void SelectExit()
    {
        Application.Quit();
    }

    public void Start()
    {
        
    }
}
