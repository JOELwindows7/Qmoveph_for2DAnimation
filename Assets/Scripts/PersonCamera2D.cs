using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCamera2D : MonoBehaviour
{
    [SerializeField] Transform targetLocator;
    public Playering playering;
    public float LerpDampen = 10f;

    private void Awake()
    {
        GameObject[] PersonCams = GameObject.FindGameObjectsWithTag("MainCamera");

        if (PersonCams.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void FixedUpdate()
    {
        if (playering)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, playering.transform.position.x, Time.deltaTime * LerpDampen), Mathf.Lerp(transform.position.y, playering.transform.position.y, Time.deltaTime * LerpDampen));
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!playering)
        //{
        //    GameObject go = GameObject.FindGameObjectWithTag("Player");
        //    if (go)
        //    {
        //        playering = go.GetComponent<Playering>();
        //    }
        //}
        if (playering)
        {
            targetLocator = playering.transform;

        }
    }
}
