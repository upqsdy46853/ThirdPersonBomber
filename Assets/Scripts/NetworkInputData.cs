using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float hInput;
    public float vInput;
    public bool isJump;
    public float xAxis;
    public float yAxis;
    public const byte MOUSEBUTTON1 = 0x01;
    public byte buttons;
    public Vector3 startingVelocity;


}
