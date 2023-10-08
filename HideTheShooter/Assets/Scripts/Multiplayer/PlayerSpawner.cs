using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{

    public Transform spawnpoint;

    void Start()
    {
        PhotonNetwork.Instantiate("Player", spawnpoint.position, Quaternion.identity);
    }
}
