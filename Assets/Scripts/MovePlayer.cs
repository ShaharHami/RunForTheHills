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
        
        
        if (Grounded() && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = -1;
            HandleStrafe();
        }

        else if (Grounded() && Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = 1;
            HandleStrafe();
        }
        

        if (Grounded() && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(InputPause(jumpDuration));
            if (transform.position.x == 0 || transform.position.x == -1 || transform.position.x == 1)
            {
                var pos = new Vector3(
                    transform.position.x,
                    0,
                    transform.position.z + (speed * jumpDuration)
                );
                bool jump2 = Random.Range(0f, 1f) > 0.5f;
                transform.DOJump(pos, jump2 ? jumpPower+0.5f : jumpPower, 1, jumpDuration).SetEase(Ease.Linear);
                animator.SetTrigger(jump2 ? "Jump" : "Jump2");
            }
        }
    }

    private void HandleStrafe()
    {
        if (transform.position.x == 0 || transform.position.x == _tileSizeX * -_direction)
        {
            StartCoroutine(InputPause(lateralMovementDuration));
            transform.DOMoveX(transform.position.x + _tileSizeX * _direction,
                lateralMovementDuration);
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
            StartCoroutine(MomentaryPause(GetClipLength("Ouch")));
        }
        else
        {
            animator.SetTrigger("Ouch2");
            StartCoroutine(MomentaryPause(GetClipLength("Ouch2")));
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

    private IEnumerator MomentaryPause(float pauseDuration)
    {
        _inputOk = false;
        var cacheSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(pauseDuration);
        speed = cacheSpeed;
        _inputOk = true;
    }

    private IEnumerator InputPause(float pauseDuration)
    {
        _inputOk = false;
        yield return new WaitForSeconds(pauseDuration);
        _inputOk = true;
    }
    public bool Grounded()
    {
        return transform.position.y == 0;
    }
}