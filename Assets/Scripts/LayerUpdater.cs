using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerUpdater : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.FloorToInt(transform.position.y));
	}

}
