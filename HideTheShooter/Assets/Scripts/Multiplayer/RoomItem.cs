using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    public Text maxPlayers;
    public Text currentPlayersText;
    LobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void setCurrentPlayers(int _currentPlayers)
    {
        currentPlayersText.text = _currentPlayers.ToString();
    }

    public void setMaxPlayers(int _maxPlayers)
    {
        maxPlayers.text = _maxPlayers.ToString();
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
