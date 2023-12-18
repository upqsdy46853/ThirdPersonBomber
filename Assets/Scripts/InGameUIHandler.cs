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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bluePoint.text = blueCollector.getCount().ToString();
        redPoint.text = redCollector.getCount().ToString();
    }
}
