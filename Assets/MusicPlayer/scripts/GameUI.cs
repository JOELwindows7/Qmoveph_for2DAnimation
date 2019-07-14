using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Playering player;
    public float Deadzoning = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (!player)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                player = go.GetComponent<Playering>();
            }
        }

        if(Input.GetAxisRaw("Horizontal") < -Deadzoning ||Input.GetAxisRaw("Horizontal") > Deadzoning)
        {
            player.MoveDirect(Input.GetAxisRaw("Horizontal"));
        } else
        {
            player.StopMove();
        }

        if(Input.GetAxisRaw("Jump") > .5f)
        {
            player.JumpNow();
        } else
        {
            player.StopJump();
        }
    }
}
