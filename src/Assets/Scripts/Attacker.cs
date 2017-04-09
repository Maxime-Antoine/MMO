using UnityEngine;

public class Attacker : MonoBehaviour {

    public float attackDistance;
    public float attackRate;

    private float _lastAttackTime = 0;

    private Targeter _targeter;

	// Use this for initialization
	void Start ()
    {
        _targeter = GetComponent<Targeter>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isReadyToAttack())
            return;

        if (isTargetDead())
        {
            _targeter.ResetTarget();
            return;
        }

	    if (_targeter.IsInRange(attackDistance))
        {
            Debug.Log("Attacking " + _targeter.target.name);

            var targetId = _targeter.target.GetComponent<NetworkEntity>().id;

            Network.Attack(targetId);
            _lastAttackTime = Time.time;
        }
	}

    bool isReadyToAttack()
    {
        return Time.time - _lastAttackTime > attackRate && _targeter.target;
    }

    bool isTargetDead()
    {
        return _targeter.target.GetComponent<Hittable>().IsDead;
    }
}
