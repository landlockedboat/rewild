using UnityEngine;

public class _LegacyMainCameraController : BitController<_LegacyMainCameraController>
{
	private static Camera _camera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	public static Vector2 WorldCenterOfScreen()
	{
		var centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
		return _camera.ScreenToWorldPoint(centerOfScreen);
	}
}
