using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class FadeToBlackController : MonoBehaviour
{
    public static FadeToBlackController Instance;

    [SerializeField] private bool _fadeInOnAwake = false;
    [SerializeField] private float _fadeTime = 1.0f;
    [SerializeField] private float _fadeAlpha = 0.0f;
    [SerializeField][ColorUsage(false)] private Color _fadeColor = Color.black;

    private Image _image;
    private Tween _fadeTween;

    public event System.Action onFadeInStart;
    public event System.Action onFadeInEnd;
    public event System.Action onFadeOutStart;
    public event System.Action onFadeOutEnd;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"<color=red>Duplicate instance ({this.name}) of type FadeToBlackController detected! This is should never happen.</color>");
            Destroy(gameObject);
        }

        Instance = this;

        _image = GetComponent<Image>();
        _fadeColor.a = _fadeAlpha;
        _image.color = _fadeColor;

        if (_fadeInOnAwake)
        {
            StartCoroutine(FadeIn());
        }
    }

    private void Update()
    {
        _fadeColor.a = _fadeAlpha;
        _image.color = _fadeColor;
    }

    public IEnumerator FadeIn()
    {
        onFadeInStart?.Invoke();
        _fadeTween = DOTween.To(() => _fadeAlpha, x => _fadeAlpha = x, 0.0f, _fadeTime);
        yield return StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        yield return _fadeTween.WaitForKill();
        onFadeInEnd?.Invoke();
    }

    public IEnumerator FadeOut()
    {
        onFadeOutStart?.Invoke();
        _fadeTween = DOTween.To(() => _fadeAlpha, x => _fadeAlpha = x, 1.0f, _fadeTime);
        yield return StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return _fadeTween.WaitForKill();
        onFadeOutEnd?.Invoke();
    }
}
