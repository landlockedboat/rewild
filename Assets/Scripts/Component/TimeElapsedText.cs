using UnityEngine;
using UnityEngine.UI;

public class TimeElapsedText : MonoBehaviour
{
	private Text _text;

	private void Awake()
	{
		_text = GetComponent<Text>();
	}

	private void Update ()
	{
		_text.text = "Time elapsed: " + Time.time.ToString("0.00");
	}
}
