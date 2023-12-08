using Fusion;
using UnityEngine;
using TMPro;

public class PlayerState : NetworkBehaviour
{
    public MeshRenderer MeshRenderer;
    public GameObject obj;
    public TextMeshProUGUI playerNicknameTM;

    [Networked(OnChanged = nameof(NetworkColorChanged))]
    public Color NetworkedColor { get; set; }
    
    [Networked(OnChanged = nameof(NetworkNicknameChanged))]
    public string NetworkedNickname { get; set; }

    [Networked(OnChanged = nameof(NetworkAmethystChanged))]
    public int NetworkedAmethyst { get; set; } = 0;

    [Networked(OnChanged = nameof(NetworkedHealthChanged))]
    public float NetworkedHealth { get; set; } = 100;

    public override void Spawned(){
        if (HasStateAuthority){
            NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            NetworkedNickname = PlayerPrefs.GetString("PlayerNickname");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
            NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }

    private static void NetworkColorChanged(Changed<PlayerState> changed)
    {
        changed.Behaviour.MeshRenderer.material.color = changed.Behaviour.NetworkedColor;
    }

    private static void NetworkNicknameChanged(Changed<PlayerState> changed)
    {
        changed.Behaviour.obj.name = changed.Behaviour.NetworkedNickname;
        changed.Behaviour.playerNicknameTM.text = changed.Behaviour.NetworkedNickname;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealAmethystRpc()
    {
        NetworkedAmethyst += 1;
    }

    private static void NetworkAmethystChanged(Changed<PlayerState> changed)
    {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Amethyst changed to: {changed.Behaviour.NetworkedAmethyst}");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        NetworkedHealth -= damage;
    }

    private static void NetworkedHealthChanged(Changed<PlayerState> changed)
    {
        // Here you would add code to update the player's healthbar.
        Debug.Log($"Health changed to: {changed.Behaviour.NetworkedHealth}");
    }
    
}
