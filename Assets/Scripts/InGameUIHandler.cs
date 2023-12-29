using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.UI;

public class InGameUIHandler : NetworkBehaviour
{
    public TextMeshProUGUI bluePoint;
    public TextMeshProUGUI redPoint;

    public amethystcollector redCollector;
    public amethystcollector blueCollector;

    private PlayerState _player;
    public TextMeshProUGUI HPTMP;

    public GameObject bombSelecter;
    public RawImage bombBox0;
    public RawImage bombBox1;
    public RawImage bombBox2;
    public TextMeshProUGUI bombText0;
    public TextMeshProUGUI bombText1;
    public TextMeshProUGUI bombText2;
    private int _selectedMap;

    // Start is called before the first frame update
    void Start()
    {
        PlayerState[] allPlayer = GameObject.FindObjectsOfType<PlayerState>();
        foreach (PlayerState player in allPlayer)
        {
            if (player.Object.HasInputAuthority)
            {
                _player = player;
            }
            player.ChangeState(PlayerState.GameState.gameStart);
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

    public void changeBomb(int code)
    {
        Color yello = new Color(1.0f, 0.5f, 0.0f);
        Color unselected = Color.black;
        _selectedMap = code;
        if (code == 1)
        {
            bombBox0.color = yello;
            bombText0.color = yello;

            bombBox1.color = unselected;
            bombText1.color = unselected;
            bombBox2.color = unselected;
            bombText2.color = unselected;
        }
        else if (code == 2)
        {
            bombBox1.color = yello;
            bombText1.color = yello;

            bombBox0.color = unselected;
            bombText0.color = unselected;
            bombBox2.color = unselected;
            bombText2.color = unselected;
        }
        else if (code == 3)
        {
            bombBox2.color = yello;
            bombText2.color = yello;

            bombBox1.color = unselected;
            bombText1.color = unselected;
            bombBox0.color = unselected;
            bombText0.color = unselected;
        }
    }
}
