using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject usernameCanvas;
    [SerializeField] private TMP_InputField joinGameInput;
    [SerializeField] private TMP_InputField createGameInput;
    [SerializeField] private Button joinGame;
    [SerializeField] private Button createGame;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Button startButton;
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    private void OnConnectedToMaster()                  //Chiamata da Photon
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void OnUsernameChange()
    {
        startButton.interactable = usernameInput.text.Length > 3;
    }

    public void SetUserName()
    {
        PhotonNetwork.playerName = usernameInput.text;
        usernameCanvas.SetActive(false);
    }

    public void OnJoinButton()
    {
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public void OnCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 10
        };

        PhotonNetwork.CreateRoom(createGameInput.text, roomOptions, null);
    }

    private void OnJoinedRoom()                             //Chiamata da Photon
    {
        PhotonNetwork.LoadLevel("MainGame");
    }

}
