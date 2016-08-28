using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject playerPrefab;
    public SocketIOComponent socket;
    
    private Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();

    public GameObject SpawnPlayer(string id, Vector3 position)
    {
        var player = Instantiate(playerPrefab, position, Quaternion.identity) as GameObject;

        player.GetComponent<ClickFollow>().myPlayer = myPlayer;
        player.GetComponent<NetworkEntity>().id = id;

        AddPlayer(id, player);

        return player;
    }

    public GameObject FindPlayer(string id)
    {
        return _players[id];
    }

    public void AddPlayer(string id, GameObject player)
    {
        _players.Add(id, player);
    }

    public void RemovePlayer(string id)
    {
        _players.Remove(id);
    }
}
