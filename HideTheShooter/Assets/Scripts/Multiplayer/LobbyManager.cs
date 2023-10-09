using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //UI FIELDS
    public InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text roomName;
    //ROOM ITEM FIELDS
    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObjects;
    //UPDATING LOBBY LIST FIELDS
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;
    //PLAYER DISPLAY ITEM FIELDS
    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;

    //LOBBY CREATOR FIELDS
    public Text currentMaxPlayersText;
    public int currentMaxPlayers = 4;
    public const int maxPlayers = 5;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("TestingScene");
    }

    public void OnClickCreate()
    {
        if(roomInputField.text.Length >= 1)
        {
            RoomOptions roomOptions =
            new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = currentMaxPlayers,
            };
            Hashtable RoomCustomProps = new Hashtable();
            RoomCustomProps.Add("MAP", 0);
            //NO GAMEMODES YET ADDED BUT GOOD FOR FUTURE
            //RoomCustomProps.Add(GAME_MODE_PROP_KEY, desiredGamemode);
            roomOptions.CustomRoomProperties = RoomCustomProps;

            string[] customPropsForLobby = { "MAP" };

            roomOptions.CustomRoomPropertiesForLobby = customPropsForLobby;
            PhotonNetwork.CreateRoom(roomInputField.text, roomOptions);
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObjects);
            newRoom.SetRoomName(room.Name);
            newRoom.setMaxPlayers(room.MaxPlayers);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach(PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null) return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    
    public void OnClickAddMaxPlayers()
    {
        if (currentMaxPlayers != maxPlayers)
        {
            currentMaxPlayers++;
            currentMaxPlayersText.text = currentMaxPlayers.ToString();
        } 
    }

    public void OnClickRemoveMaxPlayers()
    {
        if (currentMaxPlayers != 2)
        {
            currentMaxPlayers--;
            currentMaxPlayersText.text = currentMaxPlayers.ToString();
        }
    }

}
