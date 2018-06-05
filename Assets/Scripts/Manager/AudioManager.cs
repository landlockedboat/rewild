public class AudioManager : BitGameManager<AudioManager>
{
	public bool CanPlayMusic = true;
	public bool CanPlaySound = true;
	
	public void ToggleSound(bool value)
	{
		TriggerCallback(value ? NotificationType.OnSoundToggledOn : NotificationType.OnSoundToggledOff);
	}
	
	public void ToggleMusic(bool value)
	{
		TriggerCallback(value ? NotificationType.OnMusicToggledOn : NotificationType.OnMusicToggledOff);
	}
}
