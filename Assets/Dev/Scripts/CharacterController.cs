using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum TransitionParameters
{
    Idle,Walk,Run
}

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    private Joystick _joystick; 
    private Animator _animator;
    private void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveCharacter();
    }
    

    public void MoveCharacter()
    {
        float horizontalInput = _joystick.Horizontal;
        float verticalInput = _joystick.Vertical;

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.DORotateQuaternion(toRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 targetPosition = transform.position + moveDirection * moveSpeed;
        transform.DOMove(targetPosition, 0.1f).SetEase(Ease.Linear);

        
        if (moveDirection != Vector3.zero)
        {
            PlayAnim(TransitionParameters.Run.ToString(),0.1f);
        }
        else
        {
            PlayAnim(TransitionParameters.Idle.ToString(),0.1f);
        }
    }

    public void PlayAnim(string targetAnimation,float animationTransitionSpeed)
    {
        if(_animator.IsInTransition(0)) {return;}
        _animator.CrossFade(targetAnimation, animationTransitionSpeed);
    }
}