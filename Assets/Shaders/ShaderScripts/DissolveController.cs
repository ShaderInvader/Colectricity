using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Renderer[] renderersToDissolve;
    public float dissolveTime;
    public float startValue;
    public float endValue;

    private float _dissolveValue;
    private bool _isDissolving = false;

    private static int _dissolvePropertyId = Shader.PropertyToID("_Alpha_Clip");

    private void Start()
    {
        _dissolveValue = startValue;
        foreach (var renderer in renderersToDissolve)
        {
            renderer.material.SetFloat(_dissolvePropertyId, _dissolveValue);
        }
    }

    private void Update()
    {
        if (_isDissolving)
        {
            foreach (var renderer in renderersToDissolve)
            {
                renderer.material.SetFloat(_dissolvePropertyId, _dissolveValue);
            }
        }
    }

    public IEnumerator Dissolve()
    {
        _isDissolving = true;
        Tween _dissolveTween = DOTween.To(() => _dissolveValue, x => _dissolveValue = x, endValue, dissolveTime);
        yield return _dissolveTween.WaitForKill();
        _isDissolving = false;
    }
}
