using System;
using UnityEngine;

public class Sheep : MonoBehaviour, IPause, IDisposable {
    private const string IsMove = "IsMove";
    private const string IsJump = "IsJump";
    private const string IsStrike = "IsStrike";

    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private float _slowMoveSpeed = 0.2f;
    [SerializeField] private float _friction;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private SheepSFXManager _sheepSFXManager;
    private QTESystem _qTESystem;

    private float _currentSpeed;
    private bool _isMoved = true;
    private bool _isTrigger = false;

    private bool _QTEActive = false;
    private bool _qTEResult = false;
    private bool _isPaused = false;

    public AnimatorEventsHandler EventsHandler { get; private set; }

    public void Init(QTESystem qTESystem, PauseHandler pauseHandler, SheepSFXManager sheepSFXManager) {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        EventsHandler = GetComponentInChildren<AnimatorEventsHandler>();

        _sheepSFXManager = sheepSFXManager;
        _sheepSFXManager.Init(this);

        _qTESystem = qTESystem;
        _qTESystem.AllEventsCompleted += OnQTESystemAllEventsCompleted;

        pauseHandler.Add(this);

        _animator.SetBool(IsMove, true);
        _currentSpeed = _moveSpeed;
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    private void Update() {
        if (_isPaused)
            return;

        if (_isTrigger == true && _QTEActive == false) {
            _qTESystem.StartEvents();
            _QTEActive = true;
        }

        if (_isMoved == false && _qTESystem != null) {
            _animator.SetBool(IsMove, false);

            if (_qTEResult == true)
                _animator.SetBool(IsJump, true);
            else
                _animator.SetBool(IsStrike, true);

            _qTESystem = null;
        }
    }

    private void FixedUpdate() {
        if (_isPaused)
            return;

        if (_isMoved == true) {
            _rigidbody.AddForce(_currentSpeed, 0, 0, ForceMode.VelocityChange);
            _rigidbody.AddForce(-_rigidbody.velocity.x * _friction, 0, 0, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("ClickTrigger")) {
            _isTrigger = true;
            _currentSpeed = _slowMoveSpeed;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("ClickTrigger")) {
            _isTrigger = false;
            _isMoved = false;
        }
    }

    private void OnQTESystemAllEventsCompleted(bool qTEResult) {
        _qTEResult = qTEResult;
        _currentSpeed = _moveSpeed;
    }

    public void Dispose() {     
        _qTESystem.AllEventsCompleted -= OnQTESystemAllEventsCompleted;
        _qTESystem = null;
    }
}
