using UnityEngine;
using UnityEngine.UI;

public class DaysText : MonoBehaviour
{
	private Text _text;
	private string _dayString;

	private void Awake()
	{
		_text = GetComponent<Text>();
		if (!LocalisationManager.Instance.AreTranslationsLoaded)
		{
			LocalisationManager.Instance.RegisterCallback(NotificationType.OnFinishedTranslating, OnFinishedTranslating);
		}
		else
		{
			OnFinishedTranslating();
		}
		TimeController.Instance.RegisterCallback(NotificationType.OnDayPassed, OnDayPassed);
	}

	private void OnFinishedTranslating()
	{
		_dayString = LocalisationManager.Instance.GetTranslation("day");
		OnDayPassed();
	}

	private void OnDayPassed()
	{
		_text.text = _dayString + " ";
		_text.text += TimeController.Instance.CurrentDay.ToString();
	}
}
