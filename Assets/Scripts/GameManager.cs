using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spawnCanvas;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject disconnectCanvas;
    [SerializeField] private TMP_Text pingText;

    [SerializeField] private GameObject playerFeed;
    [SerializeField] private GameObject feedGrid;

    private void Awake()
    {
        spawnCanvas.SetActive(true);
    }

    private void Update()
    {
        pingText.text = "PING: " + PhotonNetwork.GetPing().ToString();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectCanvas.SetActive(!disconnectCanvas.activeSelf);
        }
    }

    public void SpawnPlayer()
    {
        Vector2 randomPos = new Vector2(Random.Range(-3f, 3f), 5);
        PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity, 0);
        spawnCanvas.SetActive(false);
        mainCamera.SetActive(false);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject o = Instantiate(playerFeed, Vector2.zero, Quaternion.identity);
        o.transform.SetParent(feedGrid.transform);
        TMP_Text txt = o.GetComponent<TMP_Text>();
        txt.text = player.NickName + " joined the game";
        txt.color = Color.green;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject o = Instantiate(playerFeed, Vector2.zero, Quaternion.identity);
        o.transform.SetParent(feedGrid.transform);
        TMP_Text txt = o.GetComponent<TMP_Text>();
        txt.text = player.NickName + " left the game";
        txt.color = Color.red;
    }
}
