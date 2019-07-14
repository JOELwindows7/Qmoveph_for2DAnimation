using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.AI;

public class GroundRayTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Colliding = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Colliding = true;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Colliding = false;
    }
}
