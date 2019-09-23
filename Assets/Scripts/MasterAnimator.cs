using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAnimator : MonoBehaviour
{


    //Singleton
    public static MasterAnimator instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ResetIdleAnimations ()
    {
        foreach (CharacterStats character in FindObjectsOfType<CharacterStats>())
        {
            character.gameObject.GetComponent<Animator>().speed = 0.0f;
            character.gameObject.GetComponent<Animator>().Play("Idle", 0, 0);
            character.gameObject.GetComponent<Animator>().speed = 1.0f;
        }
    }


}
