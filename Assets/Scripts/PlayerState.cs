using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerState : NetworkBehaviour
{
    // public Material material;
    [Networked(OnChanged = nameof(OnHPChanged))]

    byte HP {get; set;}
    const byte startingHP = 10;

    [Networked(OnChanged = nameof(OnTeamChanged))]
    public Color Team { get; set; }
    public MeshRenderer MeshRenderer;


    void Start(){
        HP = startingHP;
    }

    public void OnTakeDamage()
    {
        if (Object.HasStateAuthority)
            HP -= 1;
    }

    static void OnHPChanged(Changed<PlayerState> changed)
    {
        Debug.Log(changed.Behaviour.HP);
    }

    public void OnChangeTeam(int id)
    {
        if (Object.HasStateAuthority){
            if(id % 2 == 0)
                Team = Color.red;
            else
                Team = Color.blue;
        }

    }

    static void OnTeamChanged(Changed<PlayerState> changed)
    {
        changed.Behaviour.MeshRenderer.material.color = changed.Behaviour.Team;
    }



}

