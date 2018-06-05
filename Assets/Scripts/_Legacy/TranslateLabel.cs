using UnityEngine.UI;

public class TranslateLabel : Translator
{
    private Text _labelText;
    private string _originalLabelText;

    private void Awake()
    {
        _labelText = GetComponent<Text>();
    }

    private void Start()
    {
        _originalLabelText = _labelText.text;
        BindToTranslator(_originalLabelText, res => { _labelText.text = res; });
    }
}