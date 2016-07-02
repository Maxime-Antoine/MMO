using SocketIO;
using UnityEngine;

public class NetworkFollow : MonoBehaviour
{
    public SocketIOComponent socket;

    public void OnFollow(string id)
    {
        var jsonId = Network.PlayerIdToJson(id);

        Debug.Log("sending follow player id to server " + jsonId);

        socket.Emit("follow", jsonId);
    }
}
