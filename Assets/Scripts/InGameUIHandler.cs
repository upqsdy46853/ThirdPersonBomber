using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class InGameUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI bluePoint;
    public TextMeshProUGUI redPoint;
    public TextMeshProUGUI HPTMP;

    public amethystcollector redCollector;
    public amethystcollector blueCollector;

    private PlayerState _player;

    // Start is called before the first frame update
    void Start()
    {
        // Find player
        PlayerState[] allPlayers = GameObject.FindObjectsOfType<PlayerState>();
        foreach (PlayerState player in allPlayers)
        {
            if (player.Object.HasStateAuthority)
            {
                _player = player;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hp = _player.getHP();
        bluePoint.text = blueCollector.getCount().ToString();
        redPoint.text = redCollector.getCount().ToString();
        HPTMP.text = (hp * 100.0f).ToString() + "%";
        if(hp <= 0.3f)
        {
            HPTMP.color = new Color(255.0f, 0.0f, 0.0f, 60.0f);
        }
        else
        {
            HPTMP.color = new Color(0.0f, 255.0f, 0.0f, 60.0f);
        }
    }
}
