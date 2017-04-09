using UnityEngine;

public class CameraMove : MonoBehaviour {

    public GameObject player;

    private GameObject _mainCamera;
    private Vector3 _offset;

	// Use this for initialization
	void Start () {
        _mainCamera = gameObject;

        var playerPos = player.transform.position;
        var cameraPos = _mainCamera.transform.position;

        _offset = new Vector3(cameraPos.x - playerPos.x,
                              cameraPos.y - playerPos.y,
                              cameraPos.z - playerPos.z);
	}
	
	// Update is called once per frame
	void Update () {
        _mainCamera.transform.position = new Vector3(player.transform.position.x + _offset.x,
                                                     player.transform.position.y + _offset.y,
                                                     player.transform.position.z + _offset.z);
	}
}
