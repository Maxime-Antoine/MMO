using System;
using UnityEngine;

public class ClickFollow : MonoBehaviour, IClickable
{
    public Follower myPlayerFollower;
    private NetworkEntity _networkEntity;

    void Start()
    {
        _networkEntity = GetComponent<NetworkEntity>();
    }

    #region IClickable implementation

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("Following " + hit.collider.gameObject.name);

        GetComponent<NetworkFollow>().OnFollow(_networkEntity.id);

        myPlayerFollower.target = transform;
    }

    #endregion
}
