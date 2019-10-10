using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : MonoBehaviour
{

    //Unit that is starting on this tile
    private GameObject unit = null;
    public GameObject GetUnit () { return unit; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetUnit (GameObject u)
    {
        if (u)
        {
            unit = u;
            u.transform.position = this.transform.position;
        }
    }


    public void SwitchUnit (StartTile s)
    {
        if (s.GetUnit() != null && GetUnit() != null)
        {
            GameObject u = unit;
            SetUnit(s.GetUnit());
            s.SetUnit(u);
        }
        else if (s.GetUnit() == null && GetUnit() == null)
        {

        }
        else if (s.GetUnit() == null)
        {
            s.SetUnit(unit);
            ClearUnit();
        }
        else if (GetUnit() == null)
        {
            SetUnit(s.GetUnit());
            s.ClearUnit();
        }
    }

    public void ClearUnit ()
    {
        //unit.transform.position = new Vector3(-1, -1, 0);
        unit = null;
    }

}
