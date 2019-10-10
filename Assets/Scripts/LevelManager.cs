using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{


    public Object nextLevel;


    //Singleton
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Darkness.instance.SetToBlack();
        Darkness.instance.FadeOut();
    }


    public IEnumerator EndLevel ()
    {
        Darkness.instance.FadeIn();
        while (Darkness.instance.fading) { yield return null; }

        SceneManager.LoadSceneAsync(nextLevel.name);
    }

}
