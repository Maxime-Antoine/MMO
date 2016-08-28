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
        _socket.On("attack", OnAttack);
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
        var player = spawner.SpawnPlayer(playerId, Vector3.zero); //todo: update with current position

        //update player movement
        var targetPosition = _GetVectorFromJson(evt.data);

        var navigatePos = player.GetComponent<Navigator>();
        navigatePos.NavigateTo(targetPosition);
        Debug.Log("Player " + playerId + " moving to (x: " + targetPosition.x + ", y: " + targetPosition.y + ", z: " + targetPosition.z + ")");
    }

    private void OnMove(SocketIOEvent evt)
    {
        var player = spawner.FindPlayer(evt.data["id"].str);

        var position = _GetVectorFromJson(evt.data["targetPosition"]);
        var navigatePos = player.GetComponent<Navigator>();
        navigatePos.NavigateTo(position);

        Debug.Log("player " + evt.data["id"].str + " is moving to x: " + position.x + " y: " + position.y + " z: " + position.z);
    }

    private void OnFollow(SocketIOEvent evt)
    {
        Debug.Log("Follow request " + evt.data);

        var player = spawner.FindPlayer(evt.data["id"].str);
        var target = spawner.FindPlayer(evt.data["targetId"].str);
        var follower = player.GetComponent<Targeter>();
        follower.target = target.transform;
    }

    private void OnAttack(SocketIOEvent evt)
    {
        Debug.Log("Received attack " + evt.data);

        var targetPlayer = spawner.FindPlayer(evt.data["targetId"].str);
        targetPlayer.GetComponent<Hittable>().OnHit();

        var attackingPlayer = spawner.FindPlayer(evt.data["id"].str);
        attackingPlayer.GetComponent<Animator>().SetTrigger("Attack");
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

        var position = _GetVectorFromJson(evt.data);
        var player = spawner.FindPlayer(evt.data["id"].str);

        player.transform.position = position;
    }

    #endregion

    #region Private Helpers

    private static Vector3 _GetVectorFromJson(JSONObject json)
    {
        return new Vector3(json["x"].n,
                           json["y"].n,
                           json["z"].n);
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

    #region Public Static Methods

    public static void Move(Vector3 position)
    {
        Debug.Log("sending position to server " + position);

        var jsonPos = Network.VectorToJson(position);

        _socket.Emit("move", jsonPos);
    }

    public static void Follow(string id)
    {
        var jsonId = Network.PlayerIdToJson(id);

        Debug.Log("sending follow player id to server " + jsonId);

        _socket.Emit("follow", jsonId);
    }

    public static void Attack(string targetId)
    {
        var jsonId = PlayerIdToJson(targetId);
        Debug.Log("Attacking player " + jsonId);
        _socket.Emit("attack", jsonId);
    }

    #endregion
}
