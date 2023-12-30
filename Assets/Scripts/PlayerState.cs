using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class PlayerState : NetworkBehaviour
{
    public enum GameState
    {
        gameReady,
        gameStart,
        gameOver
    }
    private GameState _state;

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

    // bomb control ========================
        [Networked(OnChanged = nameof(OnSmokeBombCountChanged))]
    public int smokeBombCount { get; set; }
    
    [Networked(OnChanged = nameof(OnBlackBombCountChanged))]
    public int blackBombCount { get; set; }
    int ini_smoke_bomb_count;
    int ini_black_bomb_count;
    // =====================================

    private BasicSpawner _basicSpawner;
    private byte maxHP;

    private float respawnCD;
    Animator a;
    private int _selectedCode;
    private int bomb_id;
    public ReadyUIHandler _readyUI;
    public InGameUIHandler _gameUI;
    public GameObject black_ui;
    // ================
    SkinnedMeshRenderer[] skinnedMeshRenderers;
    MeshRenderer[] meshRenderers;
    public GameObject playerModel;
    private List<Color> defaultColors = new List<Color>();
    private List<Color> defaultColors2 = new List<Color>();
    // ================

    void Start()
    {
        maxHP = 4;
        ini_smoke_bomb_count = 3;
        ini_black_bomb_count = 3;
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
        _selectedCode = 1;
        bomb_id = 1;

        skinnedMeshRenderers = playerModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers){
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
                defaultColors.Add(skinnedMeshRenderer.materials[i].color);
        }
        meshRenderers = playerModel.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers){
            for (int i = 0; i < meshRenderer.materials.Length; i++)
                defaultColors2.Add(meshRenderer.materials[i].color);
        }
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
            /*if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _selectedCode = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _selectedCode = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _selectedCode = 3;
            }*/
            if (_state == GameState.gameReady){
                _readyUI.selectMap(_selectedCode);
                
            }
            else if (_state == GameState.gameStart){
                // switch(_selectedCode){
                //     case 1:
                //         bomb_id = _selectedCode;
                //         break;
                //     case 2:
                //         if(smokeBombCount > 0){
                //             bomb_id = _selectedCode;
                //         }
                //         break;
                //     case 3:
                //         if(blackBombCount > 0){
                //             bomb_id = _selectedCode;
                //         }
                //         break;
                //     default:
                //         break;
                // }
                // bomb_id = _selectedCode;
                _gameUI.changeBomb(bomb_id);
                // trajectory traj = gameObject.GetComponentInChildren<trajectory>();
                // traj.setBombID(bomb_id-1);
                // Debug.Log("selected :" + bomb_id);
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
            if(_selectedCode<=3 && _selectedCode>=1){
                _selectedCode = data.bombID;
            }
            if (_state == GameState.gameReady){
                _readyUI.selectMap(_selectedCode);
                
            }
            if (_state == GameState.gameStart){
                switch(_selectedCode){
                    case 1:
                        bomb_id = _selectedCode;
                        break;
                    case 2:
                        if(smokeBombCount > 0){
                            bomb_id = _selectedCode;
                        }
                        else{
                            bomb_id = 1;
                        }
                        break;
                    case 3:
                        if(blackBombCount > 0){
                            bomb_id = _selectedCode;
                        }
                        else{
                            bomb_id = 1;
                        }
                        break;
                    default:
                        break;
                }
                
                //bomb_id = _selectedCode;
                trajectory traj = gameObject.GetComponentInChildren<trajectory>();
                traj.setBombID(bomb_id-1);
                Debug.Log("selected :" + bomb_id);
            }

        }
    }

    public void ChangeState(GameState newState)
    {
        _state = newState;
        if (newState == GameState.gameReady)
        {
            Cursor.lockState = CursorLockMode.Locked;
            HP = maxHP;
            amethystCount = 0;
            smokeBombCount = ini_smoke_bomb_count;
            blackBombCount = ini_black_bomb_count;
            _readyUI = GameObject.FindObjectOfType<ReadyUIHandler>();
        }
        else if(newState == GameState.gameStart)
        {
            HP = maxHP;
            amethystCount = 0;
            smokeBombCount = ini_smoke_bomb_count;
            blackBombCount = ini_black_bomb_count;
            _gameUI = GameObject.FindObjectOfType<InGameUIHandler>();
            
            if(HasInputAuthority){
                black_ui = GameObject.Find("black");
                if(black_ui!=null)
                    black_ui.SetActive(false);
            }           
            //if(black_ui == null){
            //    Debug.Log("cant find black ui");
            //}
        }
        else if (newState==GameState.gameOver){
            HP = maxHP;
            amethystCount = 0;
            smokeBombCount = ini_smoke_bomb_count;
            blackBombCount = ini_black_bomb_count;
            Runner.SessionInfo.IsOpen = true;
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

    public void OnBlackScreen(){
        StartCoroutine(black_screen_co());
    }

    IEnumerator black_screen_co(){
        if(Object.HasInputAuthority){
            black_ui.SetActive(true);
        }
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers){
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
                skinnedMeshRenderer.materials[i].color = Color.black;
        }
        foreach (MeshRenderer meshRenderer in meshRenderers){
            for (int i = 0; i < meshRenderer.materials.Length; i++)
                meshRenderer.materials[i].color = Color.black;
        }
            
        yield return new WaitForSeconds(5.0f);
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers){
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
                skinnedMeshRenderer.materials[i].color = defaultColors[i];
        }
        foreach (MeshRenderer meshRenderer in meshRenderers){
            for (int i = 0; i < meshRenderer.materials.Length; i++)
                meshRenderer.materials[i].color = defaultColors2[i];
        }
        if(Object.HasInputAuthority){
            black_ui.SetActive(false);
        }
    }

    public IEnumerator Respawn(float respawnCD)
    {
        
        if(respawnCD < 2.0f)
        {
            _readyUI = GameObject.FindObjectOfType<ReadyUIHandler>();
        }

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
        smokeBombCount = ini_smoke_bomb_count;
        blackBombCount = ini_black_bomb_count;
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

    // bomb control ========================
    static void OnBlackBombCountChanged(Changed<PlayerState> changed)
    {}

    static void OnSmokeBombCountChanged(Changed<PlayerState> changed)
    {}
    public void OnGetBlackBomb()
    {
        if (Object.HasStateAuthority)
            blackBombCount += 1;
    }
    public void OnGetSmokeBomb()
    {
        if (Object.HasStateAuthority)
            smokeBombCount += 1;
    }
    // =====================================
}

