using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

    private static SocketIOComponent _socket;

    public GameObject playerPrefab;
    public GameObject myPlayer;
    public Spawner spawner;


    // Use this for initialization
    void Start ()
    {
        _socket = GetComponent<SocketIOComponent>();
        _socket.On("register", OnRegister);
        _socket.On("spawn", OnSpawn);
        _socket.On("move", OnMove);
        _socket.On("follow", OnFollow);
        _socket.On("clientDisconnected", OnDisconnect);
        _socket.On("requestPosition", OnRequestPosition);
        _socket.On("updatePosition", OnUpdatePosition);
    }

    #region Event Handlers

    private void OnRegister(SocketIOEvent evt)
    {
        Debug.Log("Successfully registered with id: " + evt.data);

        spawner.AddPlayer(evt.data["id"].str, myPlayer);
    }

    private void OnSpawn(SocketIOEvent evt)
    {
        var playerId = evt.data.GetField("id").str;
        Debug.Log("spawn - id: " + playerId);
        var player = spawner.SpawnPlayer(playerId);
        
        //update player movement
		var targetPosition = new Vector3(_GetFloatFromJson(evt.data["targetPosition"], "x"), 
										 _GetFloatFromJson(evt.data["targetPosition"], "y"), 
										 _GetFloatFromJson(evt.data["targetPosition"], "z"));

		var navigatePos = player.GetComponent<Navigator>();
		navigatePos.NavigateTo(targetPosition);
		Debug.Log("Player " + playerId + " moving to (x: " + targetPosition.x + ", y: " + targetPosition.y + ", z: " + targetPosition.z + ")");
    }

    private void OnMove(SocketIOEvent evt)
    {
        Debug.Log("player " + evt.data.GetField("id") + " is moving to x: " + evt.data.GetField("x") + " z: " + evt.data.GetField("z"));

        var player = spawner.FindPlayer(evt.data["id"].str);

        var position = new Vector3(_GetFloatFromJson(evt.data, "x"), 0, _GetFloatFromJson(evt.data, "z"));
        var navigatePos = player.GetComponent<Navigator>();
        navigatePos.NavigateTo(position);
    }

    private void OnFollow(SocketIOEvent evt)
    {
        Debug.Log("Follow request " + evt.data);

        var player = spawner.FindPlayer(evt.data["id"].str);
        var target = spawner.FindPlayer(evt.data["targetId"].str);
        var follower = player.GetComponent<Follower>();
        follower.target = target.transform;
    }

    private void OnDisconnect(SocketIOEvent evt)
    {
        var playerId = evt.data["id"].str;
        var player = spawner.FindPlayer(playerId);

        Destroy(player);
        spawner.RemovePlayer(playerId);
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
        var player = spawner.FindPlayer(evt.data["id"].str);

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
        var json = new JSONObject();
        json.AddField("x", vector.x);
        json.AddField("y", vector.y);
        json.AddField("z", vector.z);

        return json;
    }

    public static JSONObject PlayerIdToJson(string id)
    {
        var json = new JSONObject();
        json.AddField("targetId", id);

        return json;
    }

    #endregion
}
