using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTileView : TileView
{
    [SerializeField]
    private Animator _animation;

    private bool _isAnimating = false;

    public override Coroutine DestroyCell()
    {
        return StartCoroutine(DestroyCellCoroutine());
    }

    public void OnAnimationFinished()
    {
        _isAnimating = false;
    }

    private IEnumerator DestroyCellCoroutine()
    {
        _isAnimating = true;
        yield return new WaitUntil(() => _animating == false);
    }

}
