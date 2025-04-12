using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public const string TITLE = "Title";
    public const string TUTORIAL = "Tutorial";
    public const string NATURE = "Nature";
    public const string CITY = "City";
    public const string DESERT = "Desert";
    public const string SPACE = "Space";

    private static GameplayManager instance;

    public GameObject loadingScreen;

    private CoreGeneration levelGenerator;

    private bool ready = false;

    private static readonly Dictionary<string, string> SCENE_ORDER = new Dictionary<string, string>()
    {
        { TITLE, TUTORIAL }, {TUTORIAL, NATURE}, {NATURE, CITY}, {CITY, DESERT}, {DESERT, SPACE}, {SPACE, TITLE}
    };


    public static GameplayManager Instance()
    {
        return instance;
        
    }
    
    public bool IsReady()
    {
        return ready;
    }
    
    private IEnumerator Start()
    {
        loadingScreen.SetActive(true);
        instance = this;
        Time.timeScale = 0f;
        levelGenerator = FindFirstObjectByType(typeof(CoreGeneration)) as CoreGeneration;
        yield return levelGenerator==null || levelGenerator.didStart ;
        InitWhenReady();
        
        StartCoroutine("DelayFinishLoad");

    }

    private IEnumerator DelayFinishLoad()
    {
        yield return new WaitForSecondsRealtime(5f);
        loadingScreen.SetActive(false);
        ready = true;
        Time.timeScale = 1f;
    }

    private void InitWhenReady()
    {
        if (levelGenerator == null) return;
        
        PostLoadScene();
    }
    
    private static void PostLoadScene()
    {
        var levelManagerObject = new GameObject("LevelManagerObject", typeof(LevelManager));
        var levelManager = levelManagerObject.GetComponent<LevelManager>();
        levelManager.Initialize();
    }
    
    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneAsync(SCENE_ORDER[name]));
    }

    

    private float sinceStartedLoad;
    IEnumerator LoadSceneAsync(string name)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.£

        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        loadingScreen.SetActive(true);
        // Time.timeScale = 0;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            loadingScreen.GetComponentInChildren<Slider>().value = asyncLoad.progress;
            yield return null;
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));

       

        // Time.timeScale = 1;
    }
    


    
}