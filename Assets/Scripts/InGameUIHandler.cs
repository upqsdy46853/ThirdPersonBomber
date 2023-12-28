using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class InGameUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI bluePoint;
    public TextMeshProUGUI redPoint;

    public amethystcollector redCollector;
    public amethystcollector blueCollector;

    private PlayerState _player;
    public TextMeshProUGUI HPTMP;

    // Start is called before the first frame update
    void Start()
    {
        PlayerState[] allPlayer = GameObject.FindObjectsOfType<PlayerState>();
        foreach (PlayerState player in allPlayer)
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
        HPTMP.color = new Color((1 - hp), hp, 0.0f);
        /*
        if (hp <= 0.3f)
        {
            HPTMP.color = new Color(255.0f, 0.0f, 0.0f, 60.0f);
        }
        else
        {
            HPTMP.color = new Color(0.0f, 255.0f, 0.0f, 60.0f);
        }*/
    }
}
