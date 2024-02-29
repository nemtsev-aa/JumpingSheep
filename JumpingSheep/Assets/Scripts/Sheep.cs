using UnityEngine;

public class Sheep : MonoBehaviour {
    private const string IsMove = "IsMove";
    private const string IsJump = "IsJump";
    private const string IsStrike = "IsStrike";

    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private float _slowMoveSpeed = 0.2f;
    [SerializeField] private float _friction;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private float _currentSpeed;
    private bool _isMoved = true;
    private bool _isTrigger = false;

    private bool _QTEActive = false;
    private bool _qTEResult = false;
    private QTESystem _qTESystem;

    public AnimatorEventsHandler EventsHandler { get; private set; }

    public void Init(QTESystem qTESystem) {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        EventsHandler = GetComponentInChildren<AnimatorEventsHandler>();

        _qTESystem = qTESystem;
        _qTESystem.Init();
        _qTESystem.Finished += OnQTESystemFinished;

        _animator.SetBool(IsMove, true);
        _currentSpeed = _moveSpeed;
    }


    private void Update() {
        if (_isTrigger == true && _QTEActive == false) {
            _qTESystem.StartFirstEvent();
            _QTEActive = true;
        }

        if (_isMoved == false && _qTESystem != null) {
            _animator.SetBool(IsMove, false);

            if (_qTEResult == true)
                _animator.SetBool(IsJump, true);
            else
                _animator.SetBool(IsStrike, true);

            _qTESystem.Reset();
            _qTESystem = null;
        }
    }

    private void FixedUpdate() {
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

    private void OnQTESystemFinished(bool qTEResult) {
        _qTEResult = qTEResult;
        _currentSpeed = _moveSpeed;
    }
}
