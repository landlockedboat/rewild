using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logger : UiPanel<Logger>
{
    private TextMeshProUGUI _text;
    [SerializeField] private int _maxMessages = 50;
    [SerializeField] private float _alphaToSubstract = .2f;

    private List<Tuple<Color, string>> _loggedMessages;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _loggedMessages = new List<Tuple<Color, string>>();
    }

    public void LogWarning(string message)
    {
        LogMessage(Color.yellow, message);
    }

    public void LogEmergency(string message)
    {
        LogMessage(Color.red, message);
    }

    public void LogInfo(string message)
    {
        LogMessage(Color.white, message);
    }

    public void LogAdvantage(string message)
    {
        LogMessage(Color.green, message);
    }

    private void LogMessage(Color color, string message)
    {
        if (!LocalisationManager.Instance.AreTranslationsLoaded)
        {
            StartCoroutine(LogMessageAsync(color, message));
            return;
        }

        message = LocalisationManager.Instance.GetTranslation(message);

        _loggedMessages.Add(new Tuple<Color, string>(color, message));

        if (_loggedMessages.Count > _maxMessages)
        {
            _loggedMessages.RemoveAt(0);
        }

        RelogMessages();
    }

    private IEnumerator LogMessageAsync(Color color, string message)
    {
        while (!LocalisationManager.Instance.AreTranslationsLoaded)
        {
            yield return null;
        }

        LogMessage(color, message);
    }

    private void RelogMessages()
    {
        if (!IsPanelVisible)
        {
            return;
        }
        
        for (var i = 0; i < _loggedMessages.Count; i++)
        {
            var log = _loggedMessages[i];
            var newCol = log.Item1;
            if (i < _loggedMessages.Count - 1)
            {
                newCol.a -= _alphaToSubstract;
            }

            _loggedMessages[i] = new Tuple<Color, string>(newCol, log.Item2);
        }

        _text.text = "";

        foreach (var log in _loggedMessages)
        {
            _text.text += GetColoredText(log.Item1, log.Item2);
        }
    }

    private string GetColoredText(Color color, string text)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" +
               text +
               "</color>" +
               Environment.NewLine;
    }
}