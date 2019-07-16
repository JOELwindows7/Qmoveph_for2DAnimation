using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CanvasCore : MonoBehaviour
{
    [SerializeField] GameObject Titler;
    [SerializeField] GameObject LoopingArea;
    public enum MenuLocation { Main = 0, LevelSelect=1, Setting=2,Unknown=3,Extras=4, Gameplay = 5}; //wtfWhynotwork
    public MenuLocation MenuRightNow;
    public bool isPauseGame = false;
    public GameObject MainMenuItself, LevelSelectMenu, SettingMenu, UnknownMenu, ExtrasMenu, GameplayMenu;
    public GameObject dialoging;

    void Awake()
    {
        GameObject[] HexEngineCores = GameObject.FindGameObjectsWithTag("HexagonEngineCore");

        if (HexEngineCores.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        MainMenuItself.GetComponent<MenuArea>().canvasCore = this;
    }

    // Update is called once per frame
    void Update()
    {
        switch (MenuRightNow)
        {
            case MenuLocation.Main:
                MainMenuItself.SetActive(true);
                LevelSelectMenu.SetActive(false);
                SettingMenu.SetActive(false);
                ExtrasMenu.SetActive(false);
                UnknownMenu.SetActive(false);
                GameplayMenu.SetActive(false);
                break;
            case MenuLocation.LevelSelect:
                MainMenuItself.SetActive(false);
                LevelSelectMenu.SetActive(true);
                SettingMenu.SetActive(false);
                ExtrasMenu.SetActive(false);
                UnknownMenu.SetActive(false);
                GameplayMenu.SetActive(false);
                break;
            case MenuLocation.Setting:
                MainMenuItself.SetActive(false);
                LevelSelectMenu.SetActive(false);
                SettingMenu.SetActive(true);
                ExtrasMenu.SetActive(false);
                UnknownMenu.SetActive(false);
                GameplayMenu.SetActive(false);
                break;
            case MenuLocation.Extras:
                MainMenuItself.SetActive(false);
                LevelSelectMenu.SetActive(false);
                SettingMenu.SetActive(false);
                ExtrasMenu.SetActive(true);
                UnknownMenu.SetActive(false);
                GameplayMenu.SetActive(false);
                break;
            case MenuLocation.Unknown:
                MainMenuItself.SetActive(false);
                LevelSelectMenu.SetActive(false);
                SettingMenu.SetActive(false);
                ExtrasMenu.SetActive(false);
                UnknownMenu.SetActive(true);
                GameplayMenu.SetActive(false);
                break;
            case MenuLocation.Gameplay:
                MainMenuItself.SetActive(false);
                LevelSelectMenu.SetActive(false);
                SettingMenu.SetActive(false);
                ExtrasMenu.SetActive(false);
                UnknownMenu.SetActive(false);
                GameplayMenu.SetActive(true);
                break;
        }

        if (OnGoingLoading)
        {
            if (rawProgress >= 100f)
            {
                OnGoingLoading = false;
            }
            if (loadingScreen) loadingScreen.SetActive(true);
        } else
        {
            if (loadingScreen) loadingScreen.SetActive(false);
        }

        if (isPlayingGame)
        {
            Titler.SetActive(false);
            LoopingArea.SetActive(false);
        }

        if(MenuRightNow != MenuLocation.Main)
        {
            Titler.SetActive(false);
        } else
        {
            Titler.SetActive(true);
        }
    }

    [SerializeField] string CurrentLevelName;
    [SerializeField] bool isPlayingGame;
    public void PlayTheLevel(string PassName)
    {
        LoadLevel(PassName);
        CurrentLevelName = PassName;
        isPlayingGame = true;
        MenuRightNow = MenuLocation.Gameplay;
    }
    public void LeaveTheLevel()
    {
        UnloadLevel(CurrentLevelName);
        isPlayingGame = false;
        MenuRightNow = MenuLocation.Main;
    }

    //Dialog
    public void InvokeDialog()
    {
        dialoging.SetActive(true);
    }
    public void YesDialog()
    {
        dialoging.SetActive(true);
        MainMenuItself.GetComponent<MenuArea>().ConfirmQuit();
    }
    public void NoDialog()
    {
        dialoging.SetActive(false);
    }

    //ButtonPassing


    //Loading Level
    [SerializeField] bool OnGoingLoading;
    [SerializeField] Slider slider;
    [SerializeField] float rawProgress;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] GameObject loadingScreen;
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex, LoadSceneMode.Single));
    }

    public void LoadLevel(string sceneName) //This is JOELwindows7's mod. sometimes you will reffer scene by its name in the build.
    {
        StartCoroutine(LoadAsynchronously(sceneName, LoadSceneMode.Single));
    }

    public void LoadLevel(int sceneIndex, LoadSceneMode modus)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex, modus));
    }

    public void LoadLevel(string sceneName, LoadSceneMode modus) //This is JOELwindows7's mod. sometimes you will reffer scene by its name in the build.
    {
        StartCoroutine(LoadAsynchronously(sceneName, modus));
    }

    public void UnloadLevel(int sceneIndex)
    {
        StartCoroutine(UnLoadAsynchronously(sceneIndex));
    }

    public void UnloadLevel(string sceneName)
    {
        StartCoroutine(UnLoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, LoadSceneMode loadSceneModing) //Brackeys Async loading
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneModing);
        OnGoingLoading = true;

        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = progress;
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }

    IEnumerator LoadAsynchronously(string sceneName, LoadSceneMode loadSceneModing) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneModing);
        OnGoingLoading = true;

        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(int sceneIndex) //based on previous but this unloads
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneIndex);
        OnGoingLoading = true;

        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = progress;
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(string sceneName) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        OnGoingLoading = true;

        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = progress;
            //Debug.Log(progress);

            if (slider) slider.value = progress;
            if (progressText) progressText.text = progress * 100f + "%";

            if (slider)
            {
                if (slider.value >= 50)
                {
                    //HajiyevMusicManager.instance.ForcePause();
                    //MusicPlayer.ForcePause();
                }

            }

            yield return null;
        }
    }
}
