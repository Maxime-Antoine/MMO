using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

    public float health;
    public float respawnTime;
    public bool IsDead
    {
        get { return health <= 0; }
    }

    private Animator _animator;
    private Navigator _navigator;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();
        _navigator = GetComponent<Navigator>();
    }

    public void OnHit()
    {
        health -= 10;

        if (IsDead)
        {
            _animator.SetTrigger("Dead");
            Invoke("Spawn", respawnTime);
        }
    }

    private void Spawn()
    {
        Debug.Log("Spawning my player");
        transform.position = Vector3.zero;
        _navigator.ResetDestination();
        health = 100;
        _animator.SetTrigger("Spawn");
    }
}
