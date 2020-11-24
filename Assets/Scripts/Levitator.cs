using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Levitator : MonoBehaviour
{
    public float levitationRange;
    public Vector2 levitationDuration;
    public Ease levitationEase;
    private void Start()
    {
        var dur = Random.Range(levitationDuration.x, levitationDuration.y);
        var pos = transform.position.y;
        transform.DOLocalMoveY(pos + levitationRange, dur).SetEase(levitationEase).OnComplete(() =>
        {
            transform.DOLocalMoveY(pos, dur);
            
        }).SetLoops(-1, LoopType.Yoyo);
    }
}
