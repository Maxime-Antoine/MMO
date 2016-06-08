using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;
using System;

public class Network : MonoBehaviour {

    private static SocketIOComponent _socket;
    private Dictionary<string, GameObject> _players;

    public GameObject playerPrefab;

	// Use this for initialization
	void Start ()
    {
        _players = new Dictionary<string, GameObject>();

        _socket = GetComponent<SocketIOComponent>();
        _socket.On("spawn", OnSpawn);
        _socket.On("move", OnMove);
        _socket.On("clientDisconnected", OnDisconnect);
    }

    #region Event Handlers

    private void OnSpawn(SocketIOEvent evt)
    {
        Debug.Log("spawn - id: " + evt.data.GetField("id"));
        var player = Instantiate(playerPrefab);

        _players.Add(evt.data["id"].ToString(), player);
        Debug.Log("count: " + _players.Count);
    }

    private void OnMove(SocketIOEvent evt)
    {
        Debug.Log("player " + evt.data.GetField("id") + " is moving to x: " + evt.data.GetField("x") + " z: " + evt.data.GetField("z"));

        var player = _players[evt.data["id"].ToString()];

        var position = new Vector3(_GetFloatFromJson(evt.data, "x"), 0, _GetFloatFromJson(evt.data, "z"));
        var navigatePos = player.GetComponent<NavigatePosition>();
        navigatePos.NavigateTo(position);
    }

    private void OnDisconnect(SocketIOEvent evt)
    {
        var player = _players[evt.data["id"].ToString()];
        Destroy(player);
    }

    #endregion

    #region Private Helpers

    private float _GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    #endregion
}
