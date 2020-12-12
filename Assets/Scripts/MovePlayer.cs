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
    public CollisionHandler collisionHandler;
    private Collider _playerCollider;
    private bool _inputOk, _gameStarted;
    private float _tileSizeZ, _tileSizeX;
    private float _blend = 0f;
    private int _direction;
    private Coroutine _pauseCoroutine;

    private void OnEnable()
    {
        CollisionHandler.ObstacleHit += HandleObstacleHit;
    }
    private void OnDisable()
    {
        CollisionHandler.ObstacleHit -= HandleObstacleHit;
    }

    private void Start()
    {
        animator.StopPlayback();
        _gameStarted = false;
        _tileSizeZ = Tile.tileZ;
        _tileSizeX = Tile.tileX;
        Health.dead = false;
        _playerCollider = collisionHandler.GetComponent<Collider>();
    }

    public void StartMoving()
    {
        _gameStarted = true;
        _inputOk = true;
        animator.SetBool("GameStarted", true);
        AudioManager.Instance.StartFootsteps();
    }
    
    void Update()
    {
        if (!_gameStarted) return;
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
        if (!Grounded() || !_inputOk) return;
        _inputOk = false;
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
        if (!Grounded() || !_inputOk) return;
        _direction = 1;
        HandleStrafe();
    }
    public void StrafeLeft()
    {
        if (!Grounded() || !_inputOk) return;
        _direction = -1;
        HandleStrafe();
    }
    private void HandleStrafe()
    {
        if (transform.position.x == 0 || transform.position.x == _tileSizeX * -_direction)
        {
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
        _inputOk = false;
        speed = 0;
        animator.SetBool("Grounded", Grounded());
        if (!Grounded())
        {
            animator.SetTrigger("Ouch");
            if (_pauseCoroutine != null)
            {
                StopCoroutine(_pauseCoroutine);
            }
            _inputOk = false;
            _pauseCoroutine = StartCoroutine(MomentaryPause(GetClipLength("Ouch"), true));
        }
        else
        {
            animator.SetTrigger("Ouch2");
            if (_pauseCoroutine != null)
            {
                StopCoroutine(_pauseCoroutine);
            }
            _inputOk = false;
            _pauseCoroutine = StartCoroutine(MomentaryPause(GetClipLength("Ouch2"), true));
        }
    }

    public void HandleDeath()
    {
        animator.SetBool("Dead", Health.dead);
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
        if (Health.dead)
        {
            yield break;
        }
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

    public void ToggleMovement(bool move)
    {
        _inputOk = move;
    }
}