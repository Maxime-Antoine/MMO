using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;

    public float scanFrequency = 0.5f;
    public float stopFollowDistance = 1;

    private float _lastScanTime = 0;
    private Navigator _navigator;

    // Use this for initialization
    void Start()
    {
        _navigator = GetComponent<Navigator>();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (IsReadyToScan() && IsNotInRange())
        {
            Debug.Log("Scanning nav path");
            _navigator.NavigateTo(target.position);
        }
    }

    #region Private Methods

    private bool IsReadyToScan()
    {
        return Time.time - _lastScanTime > scanFrequency && target;
    }

    private bool IsNotInRange()
    {
        return Vector3.Distance(target.position, transform.position) > stopFollowDistance;
    }

    #endregion
}
