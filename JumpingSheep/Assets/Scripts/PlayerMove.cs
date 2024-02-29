using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public event Action FenceCollide;
    public event Action EndRoadCollide;

    private const string IsMove = "IsMove";
    private const string IsJump = "IsJump";
    private const string IsStrike = "IsStrike";

    [Tooltip("Физическое тело персонажа")]
    [SerializeField] private Rigidbody _rigidbody;
    [Tooltip("Тело персонажа")]
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _friction;

    private bool _isMoved = true;
    private bool _isTrigger = false;

    private bool _isJump;
    private bool _isStrike;

    private void Start() {
        _animator.SetBool(IsMove, true);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && _isTrigger == true) 
            _isJump = true;

        if (_isTrigger == false && _isMoved == false) {
            _animator.SetBool(IsMove, false);

            if (_isJump == true)
                _animator.SetBool(IsJump, true);
            else if (_isStrike == true)
                _animator.SetBool(IsStrike, true);
        }
    }

    private void FixedUpdate() {
        if (_isMoved == true) {
            _rigidbody.AddForce(_moveSpeed, 0, 0, ForceMode.VelocityChange);
            _rigidbody.AddForce(-_rigidbody.velocity.x * _friction, 0, 0, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("ClickTrigger")) {
            _canvas.enabled = true;
            _isTrigger = true;
            _isStrike = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("ClickTrigger")) {
            _canvas.enabled = false;
            _isTrigger = false;
            _isMoved = false;
        }
    }
}
