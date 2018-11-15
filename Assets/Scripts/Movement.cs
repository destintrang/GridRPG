using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


    public float speed;
    public bool moving;

    GameObject unit;
    List<Vector3Int> path;
    int pathIndex;


    //Singleton
    public static Movement instance;
    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
        moving = false;
        pathIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
        {
            Vector3 targetLocation = path[pathIndex] + new Vector3(0.5f, 0.5f, 0);
            FaceDirection(unit, targetLocation);
            unit.transform.position = Vector3.MoveTowards(unit.transform.position, targetLocation, speed * Time.deltaTime);
            if (unit.transform.position == (path[pathIndex] + new Vector3(0.5f, 0.5f, 0)))
            {
                pathIndex++;
                if (pathIndex == path.Count) { moving = false; }
            }
        }
	}


    public void BeginMoving (GameObject _unit, List<Vector3Int> _path)
    {
        if (_path.Count == 0) {
            return; }
        unit = _unit;
        path = _path;
        pathIndex = 0;
        moving = true;
    }

    void FaceDirection (GameObject unit, Vector3 targetLocation)
    {
        Animator a = unit.GetComponent<Animator>();
        Vector3 direction = unit.transform.position - targetLocation;
        if (a != null)
        {
            if (direction.x > 0 && direction.y == 0)
            {
                a.SetTrigger("Left");
            }
            else if (direction.x < 0 && direction.y == 0)
            {
                a.SetTrigger("Right");
            }
            else if (direction.x == 0 && direction.y > 0)
            {
                a.SetTrigger("Down");
            }
            else if (direction.x == 0 && direction.y < 0)
            {
                a.SetTrigger("Up");
            }
        }
    }
}
