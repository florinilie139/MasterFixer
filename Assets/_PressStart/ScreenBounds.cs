using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------/
 * Written by Waldo at Press Start
 * Contribute: https://pressstart.vip/patreon
 * More: https://pressstart.vip/resources
 * Free For Commercial Use
 * ------------------------------*/

public class ScreenBounds : MonoBehaviour {
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private bool isOrthographic;

    // Use this for initialization
    void Start(){
        if(MainCamera == null){
            MainCamera = Camera.main;
        }
        isOrthographic = MainCamera.orthographic;
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    // Update is called once per frame
    void LateUpdate(){
        Vector3 viewPos = transform.position;
        if(isOrthographic){
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        }
        else{
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        }
        transform.position = viewPos;
    }
}
