using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class MovePlayer : MonoBehaviour
{
    public float speed;
    public float minDistanceFront, minDistanceBack;
    public float lateralMovementDuration;
    public float jumpPower, jumpDuration;
    public Animator animator;
    private bool _inputOk;
    private float _tileSizeZ, _tileSizeX;
    private float _blend = 0f;
    private int _direction;
    private Coroutine _pauseCoroutine;

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleObstacleHit;
    }

    private void Start()
    {
        _tileSizeZ = Tile.tileZ;
        _tileSizeX = Tile.tileX;
        _inputOk = true;
    }
    
    void Update()
    {
        transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        animator.SetFloat("Blend", _blend);
        if (!_inputOk) return;
        
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StrafeLeft();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StrafeRight();
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (!Grounded()) return;
        _pauseCoroutine = StartCoroutine(MomentaryPause(jumpDuration));
        if (transform.position.x == 0 || transform.position.x == -1 || transform.position.x == 1)
        {
            var pos = new Vector3(
                transform.position.x,
                0,
                transform.position.z + (speed * jumpDuration)
            );
            bool jump2 = Random.Range(0f, 1f) > 0.5f;
            AudioManager.Instance.PlaySfx("Jump");
            transform.DOJump(pos, jumpPower, 1, jumpDuration).SetEase(Ease.Linear);
            animator.SetTrigger("Jump2");
        }
    }
    public void StrafeRight()
    {
        if (!Grounded()) return;
        _direction = 1;
        HandleStrafe();
    }
    public void StrafeLeft()
    {
        if (!Grounded()) return;
        _direction = -1;
        HandleStrafe();
    }
    private void HandleStrafe()
    {
        if (transform.position.x == 0 || transform.position.x == _tileSizeX * -_direction)
        {
            StartCoroutine(InputPause(lateralMovementDuration));
            transform.DOMoveX(transform.position.x + _tileSizeX * _direction,
                lateralMovementDuration);
            AudioManager.Instance.PlaySfx("Strafe");
            DOTween.To(() => _blend, x => _blend = x, _direction, lateralMovementDuration / 2).OnComplete(() =>
            {
                DOTween.To(() => _blend, x => _blend = x, 0f, lateralMovementDuration / 2);
            });
        }
    }

    private void HandleObstacleHit(Collider collider)
    {
        animator.SetBool("Grounded", Grounded());
        if (!Grounded())
        {
            animator.SetTrigger("Ouch");
            StopCoroutine(_pauseCoroutine);
            _pauseCoroutine = StartCoroutine(MomentaryPause(GetClipLength("Ouch"), true));
        }
        else
        {
            animator.SetTrigger("Ouch2");
            _pauseCoroutine = StartCoroutine(MomentaryPause(GetClipLength("Ouch2"), true));
        }
    }

    private float GetClipLength(string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        return 0f;
    }

    private IEnumerator MomentaryPause(float pauseDuration, bool stop = false)
    {
        _inputOk = false;
        var cacheSpeed = speed;
        if (stop)
        { 
            speed = 0;
        }
        AudioManager.Instance.ToggleFootSteps(false);
        yield return new WaitForSeconds(pauseDuration);
        AudioManager.Instance.ToggleFootSteps(true);
        _inputOk = true;
        if (stop)
        {
            speed = cacheSpeed;
        }
    }

    private IEnumerator InputPause(float pauseDuration)
    {
        _inputOk = false;
        yield return new WaitForSeconds(pauseDuration);
        _inputOk = true;
    }
    public bool Grounded()
    {
        return transform.position.y <= 0;
    }
}