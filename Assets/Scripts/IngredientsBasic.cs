using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class IngredientsBasic : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Canvas canvas;
    RectTransform personalImageTransform;
    CanvasGroup canvasGroup;
    RawImage currentImage;

    Vector3 InitialPosition;

    public void Start()
    {
        InitialPosition = transform.position;
        currentImage = GetComponent<RawImage>();
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
        personalImageTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
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
