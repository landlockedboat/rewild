using System;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour {
	private void Update () {
		if(Input.GetKeyDown(KeyCode.S))
		{
			var fileName = $"Screenshot_{DateTime.Now.Ticks}.png";
			var path = Path.Combine("Press", fileName);
			ScreenCapture.CaptureScreenshot(path);
			print($"Screenshot taken and saved as {path}" );
		}
	}
}
