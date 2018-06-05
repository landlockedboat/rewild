using UnityEngine.UI;

public class TranslateText : Translator
{
    private void Start()
    {
        var labelText = GetComponent<Text>();
        var originalText = labelText.text;
        BindToTranslator(originalText, res => { labelText.text = res; }, true);
    }
}