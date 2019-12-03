using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonLobby : MonoBehaviour // PunCallbacks //Photon.PunBehaviour //PunCallbacks
{
        public static PhotonLobby lobby;

        public GameObject randomJoinButton;
        public GameObject leaveButton;

        void Awake()
        {
            lobby = this;
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            randomJoinButton.SetActive(false);
            leaveButton.SetActive(false);
        }

        public void OnConnectedToMaster(PhotonLobby instance) //static override
        {
            //Debug.Log("PhotonLobby :: Connected to Master Server.");
            PhotonNetwork.AutomaticallySyncScene = true;
            randomJoinButton.SetActive(true);
        }

        public void OnRandomJoinClick()
        {
            //Debug.Log("PhotonLobby :: Clicked on the Random Join Button.");
            randomJoinButton.SetActive(false);
            leaveButton.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public void OnLeaveClick()
        {
            //Debug.Log("PhotonLobby :: Clicked on the Leave Button.");
            randomJoinButton.SetActive(true);
            leaveButton.SetActive(false);
            PhotonNetwork.LeaveRoom();
        }

        public void OnJoinRandomFailed(short returnCode, string message) //override
        {
            //Debug.Log("PhotonLobby :: Couldn't join random room, creating new room...");
            CreateRandomRoom();
        }

        public void OnCreateRoomFailed(short returnCode, string message) //override
        {
            //Debug.Log("PhotonLobby :: Couldn't create room, is might be a room with the same name. Retrying...");
            CreateRandomRoom();
        }

        void CreateRandomRoom()
        {
            int randomRoomName = Random.Range(0, 10000);
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 }; //(byte)MultiplayerSettings.mpS.maxPlayers }; //max players 4
            PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
        }

        public void OnJoinedRoom() //override
        {
            Debug.Log("Joined Photon lobby.");
        }
}

public class MonoBehaviourPunCallbacks
{
}