using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerState : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP {get; set;}
    const byte startingHP = 5;

    void Start(){
        HP = startingHP;
    }

    public void OnTakeDamage()
    {
        HP -= 1;
    }

    static void OnHPChanged(Changed<PlayerState> changed)
    {
        Debug.Log(changed.Behaviour.HP);
    }

}

