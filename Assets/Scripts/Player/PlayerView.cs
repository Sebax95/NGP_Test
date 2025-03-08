using UnityEngine;

public class PlayerView: BaseMonoBehaviour
{
    private Animator _animator;
    private void Awake() => _animator = GetComponentInChildren<Animator>();
    public void MoveForward(float value) => _animator.SetFloat("ForwardValue", value);
    
    public void Rotation(float value) => _animator.SetFloat("RotationValue", value);
    
    public void PickUp(bool value) => _animator.SetBool("PickUp", value);
    
}