using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuArea : MonoBehaviour
{
    public RectTransform selfRecto;
    public bool DrawingSetOpen = false;
    public Animator Animated;
    
    public GameObject FirstSelecting;
    public GameObject currentSelectedGameObject_Recent;
    public GameObject lastSelectedGameObject;
    // https://forum.unity.com/threads/solved-last-selected-button.289357/
    public EventSystem eventSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        selfRecto = GetComponent<RectTransform>();
        Animated = GetComponent<Animator>();

        if(eventSystem) eventSystem.SetSelectedGameObject(FirstSelecting);
    }

    // Update is called once per frame
    void Update()
    {
        GetLastGameObjectSelected();
        DontDropSelection();
    }

    private void GetLastGameObjectSelected()
    {
        if (eventSystem.currentSelectedGameObject != currentSelectedGameObject_Recent)
        {

            lastSelectedGameObject = currentSelectedGameObject_Recent;

            currentSelectedGameObject_Recent = eventSystem.currentSelectedGameObject;
        }
    }

    private void DontDropSelection()
    {
        if (!eventSystem.currentSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(lastSelectedGameObject);
        }
    }

    public void OpenMenu()
    {
        DrawingSetOpen = true;
        Animated.SetBool("DrawingOpener", DrawingSetOpen);
        // https://docs.unity3d.com/Manual/AnimationParameters.html

    }

    public void CloseMenu()
    {
        DrawingSetOpen = false;
        Animated.SetBool("DrawingOpener", DrawingSetOpen);
    }

    public void ToggleMenu()
    {
        if (!DrawingSetOpen) DrawingSetOpen = true;
        else if (DrawingSetOpen) DrawingSetOpen = false;
        Animated.SetBool("DrawingOpener", DrawingSetOpen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
