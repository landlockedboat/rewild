public class LanguagePicker : UiPanel<LanguagePicker> {

	public void PickSpanish()
	{
		GameApplication.Instance.CurrentLanguage = Language.Spanish;
		HidePanel();
	}

	public void PickEnglish()
	{
		GameApplication.Instance.CurrentLanguage = Language.English;
		HidePanel();
	}
}
