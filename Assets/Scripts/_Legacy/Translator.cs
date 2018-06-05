using System;
using System.Collections;
using UnityEngine;

public class Translator : MonoBehaviour
{
    private Action<string> _onTranslationFound;
    private string _originalText;
    private bool _isText;

    protected void BindToTranslator(string originalText, Action<string> onTranslationFound, bool isText = false)
    {
        _originalText = originalText;
        _onTranslationFound = onTranslationFound;
        _isText = isText;

        StartCoroutine(!_isText ? TranslateLabel() : TranslateText());
    }

    private void Awake()
    {
        GameApplication.Instance.RegisterCallback(NotificationType.OnLanguageChanged, OnLanguageChanged);
    }

    private void OnLanguageChanged()
    {
        StopAllCoroutines();
        StartCoroutine(!_isText ? TranslateLabel() : TranslateText());
    }
    
    private IEnumerator TranslateText()
    {
        while (!LocalisationManager.Instance.AreTranslationsLoaded)
        {
            yield return null;
        }

        var res =
            LocalisationManager.Instance.GetTextTranslation(_originalText);
        _onTranslationFound(res);
    }

    private IEnumerator TranslateLabel()
    {
        while (!LocalisationManager.Instance.AreTranslationsLoaded)
        {
            yield return null;
        }

        var res =
            LocalisationManager.Instance.GetTranslation(_originalText);
        _onTranslationFound(res);
    }
}