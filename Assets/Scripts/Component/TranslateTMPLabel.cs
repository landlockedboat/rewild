using TMPro;

public class TranslateTMPLabel : Translator {

	private void Start()
	{
		var labelText = GetComponent<TextMeshProUGUI>();
		var originalText = labelText.text;
		BindToTranslator(originalText, res => { labelText.text = res; });
	}
}
