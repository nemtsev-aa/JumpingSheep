using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public event Action FenceCollide;
    public event Action EndRoadCollide;

    [Tooltip("���������� ���� ���������")]
    [SerializeField] private Rigidbody _rigidbody;
    [Tooltip("���� ���������")]
    [SerializeField] private Transform _transform;
    [Tooltip("�������� �����������")]
    [SerializeField] private float _moveSpeed;
    [Tooltip("������������ �������� �����������")]
    [SerializeField] private float _maxSpeed;
    [Tooltip("�������� ������")]
    [SerializeField] private float _jumpSpeed;
    [Tooltip("������������� ��������������� ��������")]
    [SerializeField] private float _friction;
    [Tooltip("������ ���������� �� �����")]
    [SerializeField] private bool _grounded;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_grounded)
                _rigidbody.AddForce(0, _jumpSpeed, 0, ForceMode.VelocityChange);
        }      
    }

    private void FixedUpdate() {
        float speedMultiplier = 1f;
        if (_grounded == false) {
            speedMultiplier = 0.1f;
            // ������������ ����������� ������ � �����
            // ������
            if (_rigidbody.velocity.x > _maxSpeed && Input.GetAxis("Horizontal") > 0)
                speedMultiplier = 0f;
           
            // �����
            if (_rigidbody.velocity.x < -_maxSpeed && Input.GetAxis("Horizontal") < 0)
                speedMultiplier = 0f;
        }

        // ���������� ������ ��� ������� �� �������/������ A,D
        _rigidbody.AddForce(Input.GetAxis("Horizontal") * _moveSpeed * speedMultiplier, 0, 0, ForceMode.VelocityChange);
        
        if (_grounded == true) {
            // ������������ ����������� ������ � ������� �������������
            _rigidbody.AddForce(-_rigidbody.velocity.x * _friction, 0, 0, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.rigidbody.TryGetComponent(out Fence fence)) {
            FenceCollide?.Invoke();
        }

        if (collision.rigidbody.TryGetComponent(out EndRoad endRoad)) {
            EndRoadCollide?.Invoke();
        }

        for (int i = 0; i < collision.contactCount; i++) {
            float angle = Vector3.Angle(collision.contacts[i].normal, Vector3.up);
            
            if (angle < 45f) 
                _grounded = true;
        } 
    }

    private void OnCollisionExit(Collision collision) {
        _grounded = false;
    }

}
