using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class ToolsBasic: MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{ 

    Canvas canvas;
    RectTransform personalImageTransform;
    CanvasGroup canvasGroup;
    RawImage currentImage;
    Vector3 InitialPosition;

    public Tool ThisTool;
    
    public void SetTool (Tool tool)
    {
        ThisTool = tool;
        byte[] fileData;
        Texture2D toolSprite;

        if(null == currentImage)
        {
            currentImage = GetComponent<RawImage>();
        }

        if (File.Exists(tool.image))
        {
            fileData = File.ReadAllBytes(tool.image);
            toolSprite = new Texture2D(2, 2);
            toolSprite.LoadImage(fileData);
            currentImage.texture = toolSprite;
        }
        name = tool.name;
    }
    
    public void Start()
    {
        InitialPosition = transform.position;
    }

    public void Awake()
    {
        personalImageTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        personalImageTransform.anchoredPosition += eventData.delta/ canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.position = InitialPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("This is a " + name);
    }
}
