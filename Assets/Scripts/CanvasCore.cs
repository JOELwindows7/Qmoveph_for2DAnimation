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
    public PersonCamera2D cameraing;

    void Awake()
    {
        CurrentLevelName = SceneManager.GetActiveScene().ToString();
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
            if (rawProgress >= 90f)
            {
                OnGoingLoading = false;
            }
            if (loadingScreen) loadingScreen.SetActive(true);
        } else
        {
            if (loadingScreen) loadingScreen.SetActive(false);
        }

        if (!cameraing)
        {
            GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
            if (go)
            {
                cameraing = go.GetComponent<PersonCamera2D>();
            }
        }

        if (isPlayingGame)
        {
            //Titler.SetActive(false);
            Titler.transform.position = new Vector3(cameraing.transform.position.x,cameraing.transform.position.y,0f);
            LoopingArea.SetActive(false);
            LoopingArea.transform.position = new Vector3(cameraing.transform.position.x, cameraing.transform.position.y, 0f);
            MainMenuItself.GetComponent<MenuArea>().pauseMenuMode = true;
        } else
        {
            Titler.transform.position = Vector3.zero;
            LoopingArea.SetActive(true);
            LoopingArea.transform.position = Vector3.zero;
            MainMenuItself.GetComponent<MenuArea>().pauseMenuMode = false;

            cameraing.ZeroCamera();
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
        //UnloadLevel(CurrentLevelName);
        LoadLevel(PassName);
        CurrentLevelName = PassName;
        isPlayingGame = true;
        MenuRightNow = MenuLocation.Gameplay;
    }
    public void LeaveTheLevel()
    {
        //UnloadLevel(CurrentLevelName);
        LoadLevel("SampleScene");
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
        dialoging.SetActive(false);
        MainMenuItself.GetComponent<MenuArea>().ConfirmQuit();
    }
    public void NoDialog()
    {
        dialoging.SetActive(false);
    }

    //ButtonPassing
    public void BackToMainMenuButton()
    {
        MenuRightNow = MenuLocation.Main;
    }

    //Loading Level
    [SerializeField] bool OnGoingLoading;
    [SerializeField] Slider slider;
    [SerializeField] float rawProgress;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] GameObject loadingScreen;

    public bool OnGoingLoading1 { get => OnGoingLoading; set => OnGoingLoading = value; }

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
        rawProgress = operation.progress * 100f;

        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = operation.progress * 100f;
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
            if (operation.isDone)
            {
                OnGoingLoading = false;
            }
            yield return null;
        }
    }

    IEnumerator LoadAsynchronously(string sceneName, LoadSceneMode loadSceneModing) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneModing);
        OnGoingLoading = true;
        rawProgress = operation.progress * 100f;
        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = operation.progress * 100f;
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
            if (operation.isDone)
            {
                OnGoingLoading = false;
            }
            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(int sceneIndex) //based on previous but this unloads
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneIndex);
        OnGoingLoading = true;
        rawProgress = operation.progress * 100f;
        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = operation.progress * 100f;
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
            if (operation.isDone)
            {
                OnGoingLoading = false;
            }
            yield return null;
        }
    }

    IEnumerator UnLoadAsynchronously(string sceneName) //JOELwindows7
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        OnGoingLoading = true;
        rawProgress = operation.progress * 100f;
        //if (loadingScreen && slider.value < .985f) loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            rawProgress = operation.progress * 100f;
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
            if (operation.isDone)
            {
                OnGoingLoading = false;
            }
            yield return null;
        }
    }
}
