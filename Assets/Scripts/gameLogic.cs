using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;


public class gameLogic : NetworkBehaviour
{
    private float _gameTime = 30.0f;
    [Networked] private TickTimer gameTime { get; set; }
    bool has_set_timer = false;
    public TextMeshProUGUI timerText;
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
            gameTime = TickTimer.CreateFromSeconds(Runner, _gameTime);
            has_set_timer = true;
        }
        red_amethyst_count = red_amethystcontroller.collect_count;
        blue_amethyst_count = blue_amethystcontroller.collect_count;

        int remainingTime = (int)gameTime.RemainingTime(Runner);
        Debug.Log("timer :" + remainingTime);
        timerText.text = string.Format("{0}:{1:00}", remainingTime / 60, remainingTime % 60);

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

            Runner.SetActiveScene("Final");
            Time.timeScale = 1.0f;
            PlayerPrefs.SetString("winner", winner);
            PlayerPrefs.SetInt("redPoint", red_amethyst_count);
            PlayerPrefs.SetInt("bluePoint", blue_amethyst_count);
            Debug.Log("Time Scale 1");
        }
    }
}
