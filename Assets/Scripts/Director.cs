using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public Ingredient[] ingredients;
    public Tool[] tools;
    public int initial;
    public int final;
    public State[] states;
    public string nextLevel;
}

[System.Serializable]
public class Ingredient
{
    public string name;
    public string image;
}


[System.Serializable]
public class Tool
{
    public string name;
    public string image;
}

[System.Serializable]
public class State
{
    public int id;
    public string image;
    public string[] allow_tool_id;
    public string[] crit_tool_id;
    public string crit_tool_image;
    public string jsonToLoad;
}

public class Director : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ingredients;
    public GameObject Tools;
    public GameObject LoseTitle;
    public GameObject WinTitle;
    public Button WinButton;
    public RectTransform PlaceForIngredients;
    public RectTransform PlaceForTools;
    public AudioSource Success;
    public AudioSource Failure;

    public Text DebugsText;

    Level CurrentLevel;
    int CurrentState;
    Bench currentBench;
    string currentJson;
    bool hasActiveLevel;

    public void LoadJson(string path)
    {
        if (hasActiveLevel == true)
        {
            ResetItems();
        }
        StreamReader reader = new StreamReader(path, true);
        string json = reader.ReadToEnd();
        CurrentLevel = JsonUtility.FromJson<Level>(json);

        PutIngredients();
        PutTools();

        currentBench = FindObjectOfType<Bench>();
        currentBench.SetStage(CurrentLevel.states[--CurrentLevel.initial]);
        CurrentState = CurrentLevel.initial;
        hasActiveLevel = true;
        currentJson = path;
    }
    void ResetItems()
    {
        foreach (RectTransform childs in PlaceForIngredients)
        {
            Destroy(childs.gameObject);
        }
        foreach (RectTransform childs in PlaceForTools)
        {
            Destroy(childs.gameObject);
        }
        gameHasEnded = false;
        gameWasWon = false;
        LoseTitle.SetActive(false);
        WinTitle.SetActive(false);
        WinButton.interactable = false;
    }

    void PutIngredients()
    {
        int numberOfItems = CurrentLevel.ingredients.Length;
        float startPoint = 50 + PlaceForIngredients.position.x - PlaceForIngredients.rect.width / 2;
        float sizeForPlace = PlaceForIngredients.rect.width;
        float spaceBetweenForEachItem = sizeForPlace / numberOfItems;

        for (int i = 0; i < numberOfItems; i++)
        {
            Vector2 positionInSpace = new Vector2((spaceBetweenForEachItem * i) + startPoint + spaceBetweenForEachItem / 2, PlaceForIngredients.position.y);
            GameObject g = Instantiate(Ingredients, positionInSpace, Quaternion.identity) as GameObject;
            g.transform.SetParent(PlaceForIngredients.transform);
        }
    }

    void PutTools()
    {
        int numberOfItems = CurrentLevel.tools.Length;
        float startPoint = PlaceForTools.position.y + PlaceForTools.rect.width / 2 + 150;
        float sizeForPlace = PlaceForTools.rect.height;
        float spaceBetweenForEachItem = sizeForPlace / numberOfItems;

        for (int i = 0; i < numberOfItems; i++)
        {
            Vector2 positionInSpace = new Vector2(PlaceForTools.position.x, startPoint - (spaceBetweenForEachItem * i));
            GameObject g = Instantiate(Tools, positionInSpace, Quaternion.identity) as GameObject;
            g.transform.SetParent(PlaceForTools.transform);
            g.GetComponent<ToolsBasic>().SetTool(CurrentLevel.tools[i]);
        }
    }

    void Start()
    {
        currentJson = LevelSelect.JsonToLoad;
        hasActiveLevel = false;
        LoadJson(currentJson);        
    }


    // Update is called once per frame
    void Update()
    {
        if(currentBench.IsSuccess)
        {
            if (CurrentState > 0)
            {
                currentBench.SetStage(CurrentLevel.states[--CurrentState]);
            }
            if(CurrentLevel.final  == CurrentState)
            {
                WinButton.interactable = true;
            }
        }
        if (currentBench.IsBad)
        {
            if(CurrentState < CurrentLevel.states.Length - 1)
                currentBench.SetStage(CurrentLevel.states[++CurrentState]);
            if(CurrentState == CurrentLevel.states.Length -1)
            {
                currentBench.IsBroken = true;
                EndGame();
            }
            if (CurrentLevel.final < CurrentState)
            {
                WinButton.interactable = false;
            }
        }
        DebugsText.text = "Json: " + currentJson + "\nStates: " + CurrentState + "\n" +
                           CurrentLevel.states[CurrentState].image + "\n" +
                           "Tools Size: " + CurrentLevel.tools.Length + "\n" +
                           "Scene Size: " + CurrentLevel.states.Length + "\n" +
                           CurrentLevel.states[CurrentState].crit_tool_id + "\n" ;
    }

    bool gameHasEnded = false;
    bool gameWasWon = false;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            LoseTitle.SetActive(true);
            Failure.Play();
            Invoke("BackToMainMenu", 2f);
        }
    }

    public void WinGame()
    {
        if (gameWasWon == false)
        {
            gameWasWon = true;
            WinTitle.SetActive(true);
            Success.Play();
            Invoke("BackToMainMenu", 2f);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
