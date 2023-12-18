using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class gameLogic : NetworkBehaviour
{
    [Networked] private TickTimer gameTime { get; set; }
    bool has_set_timer = false;
    [Networked] public NetworkBool is_gameover { get; set; }

    public amethystcollector red_amethystcontroller;
    public amethystcollector blue_amethystcontroller;

    public int red_amethyst_count;
    public int blue_amethyst_count;
    [Networked] public string winner { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        has_set_timer = false;
        is_gameover = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (has_set_timer == false)
        {
            gameTime = TickTimer.CreateFromSeconds(Runner, 100.0f);
            has_set_timer = true;
        }
        red_amethyst_count = red_amethystcontroller.collect_count;
        blue_amethyst_count = blue_amethystcontroller.collect_count;

        Debug.Log("timer :" + gameTime.RemainingTime(Runner).ToString());
        if (gameTime.ExpiredOrNotRunning(Runner))
        {
            is_gameover = true;
            // freeze unity physx time, where network time will still run
            // a better way is to directly freeze player input
            Time.timeScale = 0.0f;
            // calculate winner
            if (red_amethyst_count > blue_amethyst_count)
                winner = "red";
            else if (blue_amethyst_count > red_amethyst_count)
                winner = "blue";
            else
                winner = "tie";

            Debug.Log("game over, winner team :" + winner);
            // do something, for example, show winning text on canvas etc.
            // reset timescale back to 1.0 if needed
            // Time.timeScale = 1.0f;
        }
    }
}
