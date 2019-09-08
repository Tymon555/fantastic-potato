using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class GameManager : NetworkManager
{
    public GameObject[] playerPrefabs;

    private int chosenPrefabNr = 0;

    public class NetworkMessage : MessageBase
    {
        public int chosenVehicle;
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SetChosenPrefab(int nr)
    {
        chosenPrefabNr = nr;
        Debug.Log("set prefab nr " + nr);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedVehicle = message.chosenVehicle;
        GameObject player;
        Transform startPos = GetStartPosition();
        if(startPos != null)
            player = (GameObject)Instantiate(playerPrefabs[selectedVehicle], startPos.position, startPos.rotation);
        else
            player = (GameObject)Instantiate(playerPrefabs[selectedVehicle], Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenVehicle = chosenPrefabNr;

        ClientScene.AddPlayer(conn, 0, test);
    }   


}