using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;


public class PlayerState : NetworkBehaviour
{
    // public Material material;
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP {get; set;}
    const byte startingHP = 10;

    [Networked(OnChanged = nameof(OnTeamChanged))]
    public Color Team { get; set; }
    public MeshRenderer MeshRenderer;

    [Networked(OnChanged = nameof(OnNicknameChanged))] 
    public NetworkString<_16> nickName {get; set;}
    public TextMeshProUGUI playerNickNameTM;

    void Start()
    {
        HP = startingHP;
    }

    void Update()
    {
        if(Object.HasInputAuthority)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RPC_SetTeam(Color.red);
            }
            if(Input.GetKeyDown(KeyCode.B))
            {
                RPC_SetTeam(Color.blue);
            }
        }
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
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetTeam(Color color)
    {
        Team = color;
    }

    static void OnTeamChanged(Changed<PlayerState> changed)
    {
        changed.Behaviour.MeshRenderer.material.color = changed.Behaviour.Team;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        this.nickName = nickName;
    }

   

    static void OnNicknameChanged(Changed<PlayerState> changed)
    {
        changed.Behaviour.playerNickNameTM.text = changed.Behaviour.nickName.ToString();
    }

    public override void Spawned()
    {
        if(Object.HasInputAuthority)
        {
            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));
        }
    }


}

