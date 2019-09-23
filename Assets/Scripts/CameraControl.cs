using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


    const float z = -10;

    Vector2 bounds;
    Vector2 cameraSize;

    public float border;
    public float panSpeed;


	// Use this for initialization
	void Start () {
        UpdateCameraSize();
        //SetBounds(TileManager.instance.GetLevelDimensions());
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update () {
        //The player can move the camera only on their turn
        if (MouseControl.instance.currentState != MouseControl.MouseState.DISABLED && TurnManager.instance.currentState == TurnManager.TurnState.BLUE)  
        {
            UpdateCameraPosition();
        }
    }


    void UpdateCameraPosition ()
    {
        if ((Input.mousePosition.x >= Screen.width - border) && (Input.mousePosition.y >= Screen.height - border))
        {
            // Move the camera up/right
            float x = transform.position.x + panSpeed * Time.deltaTime;
            float y = transform.position.y + panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(x, y, z);
        }
        else if ((Input.mousePosition.y >= Screen.height - border) && (Input.mousePosition.x <= border))
        {
            // Move the camera up/left
            float x = transform.position.x - panSpeed * Time.deltaTime;
            float y = transform.position.y + panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(x, y, z);
        }
        else if ((Input.mousePosition.y <= border) && (Input.mousePosition.x >= Screen.width - border))
        {
            // Move the camera down/right
            float x = transform.position.x + panSpeed * Time.deltaTime;
            float y = transform.position.y - panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(x, y, z);
        }
        else if ((Input.mousePosition.x <= border) && (Input.mousePosition.y <= border))
        {
            // Move the camera down/left
            float x = transform.position.x - panSpeed * Time.deltaTime;
            float y = transform.position.y - panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(x, y, z);
        }
        else if (Input.mousePosition.x >= Screen.width - border)
        {
            // Move the camera right
            float x = transform.position.x + panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            transform.position = new Vector3(x, transform.position.y, z);
        }
        else if (Input.mousePosition.x <= border)
        {
            // Move the camera left
            float x = transform.position.x - panSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
            transform.position = new Vector3(x, transform.position.y, z);
        }
        else if (Input.mousePosition.y >= Screen.height - border)
        {
            // Move the camera up
            float y = transform.position.y + panSpeed * Time.deltaTime;
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(transform.position.x, y, z);
        }
        else if (Input.mousePosition.y <= border)
        {
            // Move the camera down
            float y = transform.position.y - panSpeed * Time.deltaTime;
            y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
            transform.position = new Vector3(transform.position.x, y, z);
        }
        else
        {

        }
    }


    public void SetBounds(Vector2Int b)
    {
        bounds = b;
    }

    void UpdateCameraSize ()
    {
        cameraSize = new Vector2(2f * GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect, 2f * GetComponent<Camera>().orthographicSize);
        //x = Mathf.Clamp(x, 0 + (cameraSize.x / 2), bounds.x - (cameraSize.x / 2));
        //y = Mathf.Clamp(y, 0 + (cameraSize.y / 2), bounds.y - (cameraSize.y / 2));
    }
}
