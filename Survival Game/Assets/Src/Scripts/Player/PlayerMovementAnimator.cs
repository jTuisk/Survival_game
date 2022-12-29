using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimator : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private PlayerAnimationStates _previousState;
    [ReadOnly]
    [SerializeField]
    private PlayerAnimationStates _currentState;

    [SerializeField]
    private float _crossFadeTime = 0.2f;
    private Animator _animator;
    [SerializeField]
    private AnimationClip[] _animationClips;

    private void Awake()
    {
        _animator = GetComponent<Animator>(); 
        _animationClips = _animator.runtimeAnimatorController.animationClips;
    }

    private void Start()
    {
        ChangeAnimatinState(PlayerAnimationStates.Idle);
    }

    private void ChangeState(PlayerAnimationStates newState)
    {
        _previousState = _currentState;
        _currentState = newState;
    }

    public PlayerAnimationStates GetCurrentState()
    {
        return _currentState;
    }

    public PlayerAnimationStates GetPreviousState()
    {
        return _previousState;
    }

    public void ChangeAnimatinState(PlayerAnimationStates newState)
    {
        if (_animator == null)
            return;

        if (newState == _currentState)
            return;

        if (!AnimatorHasAnimation(newState.ToString()))
            return;
        
        _animator.speed = 1f;
        _animator.CrossFade(newState.ToString(), _crossFadeTime);
        ChangeState(newState);
    }

    public void ChangeAnimationSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public float GetCurrentAnimationLength()
    {
        return _animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public bool IsAnimationPlaying(string animation)
    {
        AnimationClip aClip = GetAnimationClipByName(animation);

        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animation)
                && isAnimationPlaying();
    }

    public bool isAnimationPlaying()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length > _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void PlayAnimation(string animation)
    {
        AnimationClip aClip = GetAnimationClipByName(animation);

        if (aClip == null)
            return;

        _animator.Play(animation);
    }

    private AnimationClip GetAnimationClipByName(string animation)
    {
        foreach (AnimationClip animClip in _animationClips)
        {
            if (animClip.name == animation)
                return animClip;
        }
        return null;
    }

    public float GetAnimationLength(string animation)
    {
        AnimationClip aClip = GetAnimationClipByName(animation);

        if (aClip == null)
            return -1f;

        return aClip.length;
    }

    private bool AnimatorHasAnimation(string animation)
    {
        AnimationClip aClip = GetAnimationClipByName(animation);
        return aClip != null;
    }
}
