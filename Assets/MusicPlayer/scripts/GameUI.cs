﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Playering player;
    public PersonCamera2D Cameraing;
    public CanvasCore canvasCore;
    public Slider HPSlider;
    public float Deadzoning = 0.1f;

    public void PauseButton()
    {
        if (canvasCore)
        {
            canvasCore.MenuRightNow = CanvasCore.MenuLocation.Main;
        }
    }
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
                Cameraing.playering = player;
            }
        }
        else
        {
            canvasCore.OnGoingLoading1 = false;
            HPSlider.value = player.HP1;
            if (Input.GetAxisRaw("Horizontal") < -Deadzoning || Input.GetAxisRaw("Horizontal") > Deadzoning)
            {
                player.MoveDirect(Input.GetAxisRaw("Horizontal"));
            }
            else
            {
                player.StopMove();
            }

            if (Input.GetAxisRaw("Jump") > .5f)
            {
                player.JumpNow();
            }
            else
            {
                player.StopJump();
            }
        }
    }
}
