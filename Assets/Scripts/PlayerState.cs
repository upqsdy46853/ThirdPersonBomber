using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;


public class PlayerState : NetworkBehaviour
{
    // Original NetworkPlayer
    public static PlayerState Local { get; set; }

    public GameObject amethyst;

    // public Material material;
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP {get; set;}

    [Networked(OnChanged = nameof(OnTeamChanged))]
    public Color Team { get; set; }
    public MeshRenderer MeshRenderer;

    [Networked(OnChanged = nameof(OnNicknameChanged))] 
    public NetworkString<_16> nickName {get; set;}
    public TextMeshProUGUI playerNickNameTM;

    [Networked(OnChanged = nameof(OnAmethystChanged))]
    public int amethystCount {get; set;}

    private BasicSpawner _basicSpawner;

    void Start()
    {
        HP = 10;
        amethystCount = 0;
        _basicSpawner = FindObjectOfType<BasicSpawner>(true);
    }

    void Update()
    {
        if(Object.HasInputAuthority)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RPC_SetTeam(Color.red);
                _basicSpawner.ChangeTeam(this, Color.red);
            }
            if(Input.GetKeyDown(KeyCode.B))
            {
                RPC_SetTeam(Color.blue);
                _basicSpawner.ChangeTeam(this, Color.blue);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //transform.forward;

            if (Object.HasStateAuthority && data.isThrow && amethystCount>0)
            {
                Runner.Spawn(amethyst,
                transform.position + transform.forward*0.8f + transform.up*0.5f,
                Quaternion.identity,
                Object.InputAuthority,
                (runner, o) =>
                {
                    o.GetComponent<Amethyst>().Init(transform.forward * 0.8f + transform.up * 0.5f);
                });
                amethystCount -= 1;
            }

        }
    }

    public void OnTakeDamage()
    {
        if (Object.HasStateAuthority)
        {
            HP -= 1;
        }
    }

    static void OnHPChanged(Changed<PlayerState> changed)
    {
        //a.SetBool( "hit", true );
        Debug.Log(changed.Behaviour.HP);
    }

    public void OnGetAmethyst()
    {
        if (Object.HasStateAuthority)
            amethystCount += 1;
    }

    static void OnAmethystChanged(Changed<PlayerState> changed)
    {
        Debug.Log(changed.Behaviour.amethystCount);
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
            Local = this;
        }
    }
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}

