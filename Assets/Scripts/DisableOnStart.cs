using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //This is just to be able to keep some things enabled in the Editor mode for visualization
        this.gameObject.SetActive(false);
    }
}
