using UnityEngine;
using System.Collections;
using SocketIO;
using System.Collections.Generic;
using System;

public class Network : MonoBehaviour {

    private static SocketIOComponent _socket;
    private Dictionary<string, GameObject> _players;
    
    public GameObject playerPrefab;
    public GameObject myPlayer;

    // Use this for initialization
    void Start ()
    {
        _players = new Dictionary<string, GameObject>();

        _socket = GetComponent<SocketIOComponent>();
        _socket.On("spawn", OnSpawn);
        _socket.On("move", OnMove);
        _socket.On("clientDisconnected", OnDisconnect);
        _socket.On("requestPosition", OnRequestPosition);
        _socket.On("updatePosition", OnUpdatePosition);
    }

    #region Event Handlers

    private void OnSpawn(SocketIOEvent evt)
    {
        var playerId = evt.data.GetField("id").ToString();
        Debug.Log("spawn - id: " + playerId);
        var player = Instantiate(playerPrefab);

        _players.Add(playerId, player);
        Debug.Log("count: " + _players.Count);
        
        //update player movement
		var targetPosition = new Vector3(_GetFloatFromJson(evt.data["targetPosition"], "x"), 
										 _GetFloatFromJson(evt.data["targetPosition"], "y"), 
										 _GetFloatFromJson(evt.data["targetPosition"], "z"));

		var navigatePos = player.GetComponent<NavigatePosition>();
		navigatePos.NavigateTo(targetPosition);
		Debug.Log("Player " + playerId + " moving to (x: " + targetPosition.x + ", y: " + targetPosition.y + ", z: " + targetPosition.z + ")");
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

    private void OnRequestPosition(SocketIOEvent evt)
    {
        Debug.Log("Server requesting position - transmitting");

        _socket.Emit("updatePosition", VectorToJson(myPlayer.transform.position));
    }

    private void OnUpdatePosition(SocketIOEvent evt)
    {
        Debug.Log("Updating position " + evt.data);

        var position = new Vector3(_GetFloatFromJson(evt.data, "x"), _GetFloatFromJson(evt.data, "y"), _GetFloatFromJson(evt.data, "z"));
        var player = _players[evt.data["id"].ToString()];

        player.transform.position = position;
    }

    #endregion

    #region Private Helpers

    private float _GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    #endregion

    #region Public Helpers

    public static JSONObject VectorToJson(Vector3 vector)
    {
        var jsonPos = new JSONObject();
        jsonPos.AddField("x", vector.x);
        jsonPos.AddField("y", vector.y);
        jsonPos.AddField("z", vector.z);

        return jsonPos;
    }

    #endregion
}
