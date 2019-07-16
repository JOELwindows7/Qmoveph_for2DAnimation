using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroyEvent : MonoBehaviour
{

    [SerializeField] string tagName;
    void Awake()
    {
        tagName = gameObject.tag; //may not null

        GameObject[] Similars = GameObject.FindGameObjectsWithTag(tagName);

        if (Similars.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
