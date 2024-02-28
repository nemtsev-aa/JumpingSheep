using System;
using UnityEngine;

public class Sheep : MonoBehaviour {
    public event Action FenceCollide;
    public event Action EndRoadCollide;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _friction;
    [SerializeField] private float _jumpSpeed;

    private Rigidbody _rigidbody;
    private bool _isMove = false;
    

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        StartMove();
    }

    [ContextMenu(nameof(StartMove))]
    public void StartMove() {
        _isMove = true;
    }

    [ContextMenu(nameof(StopMove))]
    public void StopMove() {
        _isMove = false;
    }

    private void FixedUpdate() {
        if (_isMove) {
            _rigidbody.AddForce(Input.GetAxis("Horizontal") * _moveSpeed, 0, 0, ForceMode.Acceleration);
            _rigidbody.AddForce(-_rigidbody.velocity.x * _friction, 0, 0, ForceMode.VelocityChange);

            if (Input.GetKeyDown(KeyCode.Space)) {
                _rigidbody.AddForce(0, _jumpSpeed, 0, ForceMode.VelocityChange);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.rigidbody.TryGetComponent(out Fence fence)) {
            FenceCollide?.Invoke();
        }

        if (collision.rigidbody.TryGetComponent(out EndRoad endRoad)) {
            EndRoadCollide?.Invoke();
        }
    }
}
