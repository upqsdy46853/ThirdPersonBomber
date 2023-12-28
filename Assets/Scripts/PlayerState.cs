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
    public byte HP {get; set;}

    [Networked(OnChanged = nameof(OnTeamChanged))]
    public Color Team { get; set; }
    public MeshRenderer MeshRenderer;

    [Networked(OnChanged = nameof(OnNicknameChanged))] 
    public NetworkString<_16> nickName {get; set;}
    public TextMeshProUGUI playerNickNameTM;

    [Networked(OnChanged = nameof(OnAmethystChanged))]
    public int amethystCount {get; set;}

    private BasicSpawner _basicSpawner;
    private byte maxHP;

    private float respawnCD;
    Animator a;


    void Start()
    {
        maxHP = 2;
        respawnCD = 3.0f;
        HP = maxHP;
        amethystCount = 0;
        _basicSpawner = FindObjectOfType<BasicSpawner>(true);

        System.Random r = new System.Random();
        if (r.Next() % 2 == 0)
            Team = Color.red;
        else
            Team = Color.blue;

        a = transform.Find("MaleCharacterPolyart").GetComponent<Animator>();
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

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //transform.forward;

            if (Object.HasStateAuthority && data.isThrow && amethystCount>0)
            {
                throw_amethyst();
            }

        }
    }

    public void OnTakeDamage()
    {
        HP -= 1;
        if (HP <= 0 || HP == 255)
        {
            a.SetBool("die", true);
            if(HasInputAuthority){
                transform.Find("landingPoint").gameObject.SetActive(false);
                transform.Find("body").Find("hand").GetComponent<LineRenderer>().enabled = false;
            }
            StartCoroutine(Respawn(respawnCD));
        }
        else
        {
            a.SetBool("hit", true);
        }
    }

    public IEnumerator Respawn(float respawnCD)
    {
        Debug.Log(gameObject.transform.position);
        yield return new WaitForSeconds(respawnCD);
        a.SetBool("die", false);
        if(HasInputAuthority){
            transform.Find("landingPoint").gameObject.SetActive(true);
            transform.Find("body").Find("hand").GetComponent<LineRenderer>().enabled = true;
        }
        
        if (gameObject.TryGetComponent<movement>(out var movement))
        {
            HP = maxHP;
            // throw all amethyst
            while(amethystCount>0)
            {
                throw_amethyst(true);
                Debug.Log("throw amethyst");
            }
            // debug: cant teleport to specified place
            
            if (gameObject.TryGetComponent<CharacterController>(out var cc)){
                cc.enabled = false;
                if(Team == Color.red){
                    movement.teleport(new Vector3(13.0f, 1.0f, Random.Range(-7.0f, 7.0f)));
                    Debug.Log(gameObject.transform.position);
                }
                else{
                    movement.teleport(new Vector3(-15.0f, 1.0f, Random.Range(-7.0f, 7.0f)));
                    Debug.Log(gameObject.transform.position);
                }
                cc.enabled = true;
            }
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
        changed.Behaviour.playerNickNameTM.color = changed.Behaviour.Team;
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

    public void throw_amethyst(bool use_rand = false)
    {
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
        if (use_rand)
        {
            scale = new Vector3(Random.Range(-1,1), Random.Range(0.5f, 1), Random.Range(-1, 1));
        }

        Runner.Spawn(amethyst,
                transform.position + Vector3.Scale((transform.forward * 0.8f + transform.up * 0.5f), scale),
                Quaternion.identity,
                Object.InputAuthority,
                (runner, o) =>
                {
                    o.GetComponent<Amethyst>().Init(transform.forward * 0.8f + transform.up * 0.5f);
                });
        amethystCount -= 1;
    }
    public float getHP()
    {
        return (float)HP / (float)maxHP;
    }
}

