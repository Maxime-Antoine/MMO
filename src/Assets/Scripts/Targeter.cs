using UnityEngine;
using System.Collections;

public class Targeter : MonoBehaviour {

    public Transform target;

    public bool IsInRange(float stopFollowDistance)
    {
        var distance = Vector3.Distance(target.position, base.transform.position);
        return distance < stopFollowDistance;
    }
}
