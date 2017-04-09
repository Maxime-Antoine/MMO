using System.Collections;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour {

    private float _speed = 10f; //walk speed
    private float _degreesPerSecond = 600f; //rotation speed
    private float _updateNetworkEverySecs = 0.33f;
    private bool _walking = false;
    private IEnumerator _networkPositionUpdater;
    private Quaternion _targetRotation;
    private Transform _characterTransform;
    private Navigator _navigator;
    private Animator _animator;

	// Use this for initialization
	void Start () {
        _characterTransform = gameObject.transform;
        _navigator = GetComponent<Navigator>();
        _animator = GetComponent<Animator>();
        _networkPositionUpdater = SendPositionOnNetwork();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)
         || Input.GetKeyDown(KeyCode.DownArrow)
         || Input.GetKeyDown(KeyCode.LeftArrow)
         || Input.GetKeyDown(KeyCode.RightArrow))
            StartWalking();

        if (Input.GetKeyUp(KeyCode.UpArrow)
         || Input.GetKeyUp(KeyCode.DownArrow)
         || Input.GetKeyUp(KeyCode.LeftArrow)
         || Input.GetKeyUp(KeyCode.RightArrow))
            if (!Input.GetKey(KeyCode.UpArrow)
             && !Input.GetKey(KeyCode.DownArrow)
             && !Input.GetKey(KeyCode.LeftArrow)
             && !Input.GetKey(KeyCode.RightArrow))
                StopWalking();

        //walking
        Vector3 target = _characterTransform.position;
        if (Input.GetKey(KeyCode.UpArrow))
            target += new Vector3(0, 0, _speed / 100);

        if (Input.GetKey(KeyCode.DownArrow))
            target -= new Vector3(0, 0, _speed / 100);

        if (Input.GetKey(KeyCode.LeftArrow))
            target -= new Vector3(_speed / 100, 0, 0);

        if (Input.GetKey(KeyCode.RightArrow))
            target += new Vector3(_speed / 100, 0, 0);

        if (Input.GetKey(KeyCode.UpArrow)
         || Input.GetKey(KeyCode.DownArrow)
         || Input.GetKey(KeyCode.LeftArrow)
         || Input.GetKey(KeyCode.RightArrow))
        {
            _characterTransform.LookAt(target);
            _characterTransform.position = target;
        }
    }

    private void LookAtDirection(Vector3 moveDirection)
    {
        Vector3 xzDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

        if (xzDirection.magnitude > 0)
            _targetRotation = Quaternion.LookRotation(xzDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _degreesPerSecond * Time.deltaTime);
    }

    private void StartWalking()
    {
        if (!_walking)
        {
            _walking = true;
            _navigator.StopNavigate();
            _animator.SetFloat("Distance", 100); //dummy distance > threshold to trigger running anim
            StartCoroutine(_networkPositionUpdater);
        }
    }

    private void StopWalking()
    {
        if (_walking)
        {
            _walking = false;
            _animator.SetFloat("Distance", 0);  //dummy distance < threshold to trigger iddle anim
            StopCoroutine(_networkPositionUpdater);
        }
    }

    private IEnumerator SendPositionOnNetwork()
    {
        while(true)
        {
            yield return new WaitForSeconds(_updateNetworkEverySecs);
            Network.IddlePosition(_characterTransform.position);
        }
    }
}
