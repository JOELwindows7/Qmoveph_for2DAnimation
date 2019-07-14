using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCard : MonoBehaviour
{
    [SerializeField] CanvasCore canvasCore;
    [SerializeField] string PlayLevelOfName = "MockupScene";
    void awake()
    {
        canvasCore = gameObject.transform.parent.parent.parent.parent.parent.GetComponent<CanvasCore>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLevelButton()
    {
        canvasCore.PlayTheLevel(PlayLevelOfName);
    }
}
