using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {

    }

    public void OnClick (Vector3 position)
    {
        var navpos = player.GetComponent<NavigatePosition>();
        var netMove = player.GetComponent<NetworkMove>();
        navpos.NavigateTo(position);

        netMove.OnMove(position);
	}
}
