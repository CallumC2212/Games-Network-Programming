using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class playerMovement : NetworkBehaviour
{
    public float speed = 5f;

   // public override void OnNetworkSpawn()
  //  {
        //this section will be triggered when a player enters / spawned into the game
    //}

    void FixedUpdate()
    {
        if (IsOwner)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                HandleMovementServerRpc(1, this.NetworkObjectId);
                //               transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                HandleMovementServerRpc(2, this.NetworkObjectId);
                //               transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                HandleMovementServerRpc(3, this.NetworkObjectId);
                //                transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
                //                movementAnimator.SetBool("isJump", true);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                HandleMovementServerRpc(4, this.NetworkObjectId);
                //                transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
            }
        }
    }
    [ServerRpc]
    void HandleMovementServerRpc(int movementdirection, ulong theIDOftheCharacterThatMoves)
    {
       // Debug.Log("the player " + theIDOftheCharacterThatMoves + " just moves from position " + NetworkManager.Singleton.ConnectedClients[0].PlayerObject.transform.position);
        HandleMovementClientRpc(movementdirection);
    }

    [ClientRpc]

    void HandleMovementClientRpc(int movementdirection)
    {
        switch (movementdirection)
        {
            case 1:
                transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
                break;
            case 2:
                transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                break;
            case 3:
                transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
                //movementAnimator.SetBool("isJump", true);
                break;
            case 4:
                transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
                break;

        }
    }
}

