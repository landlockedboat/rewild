using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
	private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		AudioManager.Instance.RegisterCallback(NotificationType.OnMusicToggledOn,
			OnMusicToggledOn);
		AudioManager.Instance.RegisterCallback(NotificationType.OnMusicToggledOff,
			OnMusicToggledOff);
	}

	private void Start () 
	{
		if (AudioManager.Instance.CanPlayMusic)
		{
			_audioSource.Play();
		}
	}

	private void OnDisable()
	{
		AudioManager.Instance.UnregisterCallback(NotificationType.OnMusicToggledOn,
			OnMusicToggledOn);
		AudioManager.Instance.UnregisterCallback(NotificationType.OnMusicToggledOff,
			OnMusicToggledOff);
	}

	private void OnMusicToggledOff()
	{
		_audioSource.Pause();
	}

	private void OnMusicToggledOn()
	{
		_audioSource.Play();
	}
}
