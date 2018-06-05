using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private Animator _animator;
    private Text _text;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<Text>();
    }
    
    public void DisplayText(string text)
    {
        _text.enabled = true;
        _text.text = text;
        _animator.enabled = true;
        _animator.Play(0);
    }
    
    public void DisplayText(string text, Color color)
    {
        _text.color = color;
        DisplayText(text);
    }
}