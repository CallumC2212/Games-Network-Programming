using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class playerManager : NetworkBehaviour
{
    // unassigned team = 0, red team = 1, blue team = 2 
    public int team = 0;
    public NetworkVariable<ushort> redTeamSize;
    public NetworkVariable<ushort> blueTeamSize;
    private readonly ulong[] _targetClientsArray = new ulong[1];
    private bool doOnce = true;
    public override void OnNetworkSpawn()
    {
        if (doOnce == true)
        {
            doOnce = false;
            Debug.Log("networkspawn has been called by " + OwnerClientId);
            Debug.Log("doOnce has been called by" + OwnerClientId);
            if (IsOwner)
            {
                redTeamSize = new(0);
                blueTeamSize = new(0);
            };
            var Player = new PlayerData()
            {
                Id = OwnerClientId
            };
            ////this section will be triggered when a player enters / spawned into the game
            AssignTeamServerRPC(Player);
        }
    }

    
    [ServerRpc (RequireOwnership = false)]
    void AssignTeamServerRPC(PlayerData player) 
    {
        Debug.Log("serverRPC called by " + player.Id);
           _targetClientsArray[0] = player.Id;
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = _targetClientsArray
                }
            };
            //check how many players per team 
            // if red = blue assign random team 
            if (redTeamSize.Value != blueTeamSize.Value)
            {
                if (redTeamSize.Value < blueTeamSize.Value)
                {
                    AssignRedTeamClientRPC();
                }
                else
                {
                    AssignBlueTeamClientRPC();
                }
            }
            else
            {
                if (Random.Range(1, 2) == 1)
                {
                    AssignRedTeamClientRPC();
                }
                else
                {
                    AssignBlueTeamClientRPC();
                }
            }
            // if red != blue and < blue assign to red else assign to blue
            // pass variable and player id to client rpc
        
    }

    [ClientRpc]

    void AssignBlueTeamClientRPC() 
    {
        Debug.Log("blue team called");
        if (!IsOwner) return;
        blueTeamSize.Value += 1;
        team = 2;
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        // set player with id passed team equal to team passed in and colour
        Debug.Log("blue team applied");
    }

    [ClientRpc]

    void AssignRedTeamClientRPC()
    {
        Debug.Log("red team called");
        if (!IsOwner) return;
        redTeamSize.Value += 1;
        team = 1;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        // set player with id passed team equal to team passed in and colour
        Debug.Log("red team applied");
    }

    struct PlayerData : INetworkSerializable 
    {
        public ulong Id;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
        }
    }

    private void Start()
    {
        
    }
    
}
