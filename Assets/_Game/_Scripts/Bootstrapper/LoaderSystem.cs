using System;
using UnityEngine;

public class LoaderSystem : MonoBehaviour
{
    private static LoaderSystem _instance;

    [SerializeField] private CanvasGroup _loaderVisual;
    [SerializeField] private Transform _spinnerImage;
    [SerializeField] private float _fadeSpeed = 2;

    private float _target;

    private void Awake()
    {
        _instance = this;
        ToggleLoader(false);
        _loaderVisual.alpha = 0;
    }

    private void ToggleLoader(bool on)
    {
        _loaderVisual.interactable = on;
        _loaderVisual.blocksRaycasts = on;
        _target = on ? 1 : 0;
    }

    private void Update()
    {
        _loaderVisual.alpha = Mathf.MoveTowards(_loaderVisual.alpha, _target, _fadeSpeed * Time.deltaTime);
        _spinnerImage.Rotate(new Vector3(0, 0, 20 * Time.deltaTime));
    }

    public class Load : IDisposable
    {
        public Load()
        {
            _instance.ToggleLoader(true);
        }

        public void Dispose()
        {
            _instance.ToggleLoader(false);
        }
    }
}