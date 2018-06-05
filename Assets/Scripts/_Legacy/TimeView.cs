using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : BitView
{
    [SerializeField] private Text _timeText;

    private TimeModel _timeModel;

    private void Awake()
    {
        _timeModel = TimeModel.Instance;
    }

    private void LateUpdate()
    {
        _timeText.text = "Current time" + Environment.NewLine;
        _timeText.text += _timeModel.CurrentTime.ToString("dd/MM/yyyy HH:mm:ss") + Environment.NewLine;
    }
}