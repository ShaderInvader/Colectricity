using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    #region SerializedFields

    [Header("Movement")]
    [SerializeField] private float normalSpeed = 8f;
    [SerializeField] private float ghostSpeed = 3f;
    [Tooltip("Amount of smoothing")]
    [SerializeField] private float movementSmoothing = 0.08f;
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 2.5f;
    [SerializeField] private float dashTime = 0.2f;
    [Header("Bouncing")]
    [SerializeField] private List<string> knockbackTags;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float bounceTime = 0.2f;

    #endregion

    #region Private Fields

    // References
    private Rigidbody _rigidbody;
    private Camera _camera;
    private Electron _electron;
    private Energabler _energabler;
    private PlayerInput _playerInput;

    private float _cameraAngle;
    private Vector2 _inputVector;
    private Vector3 _movementVector;
    private float _currentSpeed;
    private bool _isDashing;
    private Vector3 _originalAppliedVector, _appliedVector = new(0, 0, 0);
    private float _originalDurationVector, _durationAppliedVector;

    #endregion

    public Vector3 MovementVector => _movementVector;
    public float NormalSpeed => normalSpeed;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _cameraAngle = _camera.transform.eulerAngles.y;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _electron = GetComponent<Electron>();
        _energabler = GetComponent<Energabler>();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {
        if (_isDashing)
        {
            StartCoroutine(Dash());
            return;
        }
        if (_durationAppliedVector > 0)
        {
            _durationAppliedVector -= Time.deltaTime;
            if (_durationAppliedVector < 0) _durationAppliedVector = 0;
            _appliedVector = Vector3.Lerp(Vector3.zero, _originalAppliedVector,
                _durationAppliedVector / _originalDurationVector);
        }

        _currentSpeed = _electron.isDead ? ghostSpeed : normalSpeed;
        
        UpdateMovementVector();
        Move(_movementVector);
        
        if (_playerInput.actions["Dash"].WasPerformedThisFrame() && !_isDashing)
        {
            _isDashing = true;
        }
    }
    
    private void UpdateMovementVector()
    {
        _inputVector = _playerInput.actions["Movement"].ReadValue<Vector2>();
        _rigidbody.velocity = Vector3.zero;
        var vel = new Vector3(_inputVector.x, 0, _inputVector.y) * _currentSpeed;
        var movement = Quaternion.Euler(0, _cameraAngle, 0) * vel;
        _movementVector = Vector3.Lerp(_movementVector, movement, movementSmoothing);
        _movementVector += _appliedVector;
    }

    private void Move(Vector3 vector)
    {
        _rigidbody.velocity = vector;
        _rigidbody.angularVelocity = new Vector3(vector.z, 0, -vector.x);
    }

    IEnumerator Dash()
    {
        var startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            Move(_movementVector.normalized * (_currentSpeed * dashSpeed));
            yield return null;
        }

        _isDashing = false;
    }

    private void ApplyKnockback(GameObject other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var currSpeed = _electron.isDead ? ghostSpeed : normalSpeed;
            var movementMagnitude = other.GetComponent<Movement>()._movementVector.magnitude;
            if (movementMagnitude < _movementVector.magnitude || movementMagnitude < currSpeed / 2) 
                return;
        }

        if (other.transform.CompareTag("SmallDoor"))
        {
            if (_energabler.energy_units == 0)
            {
                return;
            }
        }
        
        var dir = transform.position - other.transform.position;
        dir.y = 0;
        dir = dir.normalized;
        ApplyVector(dir * bounceForce, bounceTime);
    }
    
    private void ApplyVector(Vector3 appVect, float duration)
    {
        _originalAppliedVector = appVect;
        _durationAppliedVector = _originalDurationVector = duration;
    }

    #region Unity Events
    private void OnCollisionEnter(Collision collision)
    {
        if (!knockbackTags.Contains(collision.transform.tag)) return;
        ApplyKnockback(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!knockbackTags.Contains(other.transform.tag)) return;
        ApplyKnockback(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!knockbackTags.Contains(other.transform.tag)) return;
        ApplyKnockback(other.gameObject);
    }

    #endregion
}