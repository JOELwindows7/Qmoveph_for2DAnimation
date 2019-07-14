using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRun : MonoBehaviour
{
    public float runningSpeed = 5f;
    public Transform LoopPoint;
    public Transform EndPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= LoopPoint.position.x)
        {
            transform.position = EndPoint.position;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * runningSpeed * Time.deltaTime);
    }
}
