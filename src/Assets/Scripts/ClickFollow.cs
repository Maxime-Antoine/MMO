using System;
using UnityEngine;

public class ClickFollow : MonoBehaviour, IClickable
{
    public GameObject myPlayer;
    private NetworkEntity _networkEntity;
    private Targeter _myPlayerTargeter;

    void Start()
    {
        _networkEntity = GetComponent<NetworkEntity>();
        _myPlayerTargeter = myPlayer.GetComponent<Targeter>();
    }

    #region IClickable implementation

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("Following " + hit.collider.gameObject.name);

        Network.Follow(_networkEntity.id);

        _myPlayerTargeter.target = transform;
    }

    #endregion
}
