using UnityEngine;
using System.Collections;
using SocketIO;

public class NetworkMove : MonoBehaviour {

    public SocketIOComponent socket;

    public void OnMove(Vector3 position)
    {
        Debug.Log("sending position to server " + position);

        var jsonPos = new JSONObject();
        jsonPos.AddField("x", position.x);
        jsonPos.AddField("y", position.y);
        jsonPos.AddField("z", position.z);

        socket.Emit("move", jsonPos);
    }
}
