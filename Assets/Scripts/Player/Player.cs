using System;
using DG.Tweening;
using UnityEngine;

public class Player: BaseMonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;

    [SerializeField] 
    private float _speed;
    private Camera _camera;
    private IController _controller;
    private PlayerView _playerView;
    private Rigidbody _rigidbody;
    private Tween _movementTween;

    protected override void Start()
    {
        base.Start();
        _controller = new PlayerController(this);
        _rigidbody = GetComponent<Rigidbody>();
        _playerView = GetComponent<PlayerView>();
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        Vector3 inputDirection = _controller.Move();
        
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
        
        Vector3 forwardRelativeMovementVector = inputDirection.z * cameraForward;
        Vector3 rightRelativeMovementVector = inputDirection.x * cameraRight;
        
        Vector3 cameraRelativeMovement = forwardRelativeMovementVector + rightRelativeMovementVector;
        
        MovePlayer(cameraRelativeMovement);
        
        _playerView.MoveForward(inputDirection.z);
        _playerView.Rotation(inputDirection.x);
        
    }

    public override void OnLateUpdate()
    {
       /* Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraDirection = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        
        if (cameraDirection.magnitude > 0.1f)
            transform.forward = cameraDirection;*/
    }

    private void MovePlayer(Vector3 direction)
    {
        if (direction.magnitude > 0.1f) 
            _rigidbody.velocity = direction.normalized * _speed;
        else
            _rigidbody.velocity = Vector3.zero;
    }

    private void StopPlayer()
    {
        _rigidbody.velocity = Vector3.zero;
    }

}