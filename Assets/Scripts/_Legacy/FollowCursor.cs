using UnityEngine;

public class FollowCursor : MonoBehaviour
{
	private InputModel _inputModel;
	
	void Start ()
	{
		_inputModel = InputModel.Instance;
	}

	void Update ()
	{
		transform.position = _inputModel.WorldCursorPos;
	}
}
