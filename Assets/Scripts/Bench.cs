using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class Bench : MonoBehaviour, IDropHandler
{
    public AudioSource SoundChange;

    public bool IsSuccess = false;
    public bool IsBad = false;
    public bool IsBroken = false;

    RawImage currentImage;

    State currentState;

    void ChangeImage(string Image)
    {
        byte[] fileData;
        Texture2D currentImageforState;

        if (File.Exists(Image))
        {
            fileData = File.ReadAllBytes(Image);
            currentImageforState = new Texture2D(2, 2);
            currentImageforState.LoadImage(fileData);
            currentImage.texture = currentImageforState;
            GetComponent<RectTransform>().sizeDelta = new Vector2(currentImageforState.width / 2, currentImageforState.height / 2);
        }
    }

    public void SetStage(State state)
    {
        IsSuccess = false;
        IsBad = false;
        IsBroken = false;
        currentState = state; 

        if (null == currentImage)
        {
            currentImage = GetComponent<RawImage>();
        }

        ChangeImage(currentState.image);
    }

    public void Start()
    {
        currentImage = GetComponent<RawImage>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!IsBroken)
        {
            if (null != currentState.allow_tool_id)
            {
                foreach (string tool in currentState.allow_tool_id)
                    if (tool.Equals(eventData.pointerDrag.name))
                    {
                        IsSuccess = true;
                        SoundChange.Play();
                        return;
                    }
            }
            if (null != currentState.crit_tool_id)
            {
                foreach (string tool in currentState.crit_tool_id)
                    if (tool.Equals(eventData.pointerDrag.name))
                    {
                        if (currentState.crit_tool_image != null)
                        {
                            IsBroken = true;
                            ChangeImage(currentState.crit_tool_image);
                            FindObjectOfType<Director>().EndGame();
                        }
                        else if (currentState.jsonToLoad != null)
                        {
                            FindObjectOfType<Director>().LoadJson(currentState.jsonToLoad);
                            SoundChange.Play();
                        }
                        return;
                    }
            }
            if (IsSuccess != true)
            {
                SoundChange.Play();
                IsBad = true;
            }
            
        }
    } 
}
