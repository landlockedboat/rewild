 using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private float _cameraHeight = -10;

    private void Awake()
    {
        _playerTransform = PlayerController.Instance.transform;
    }

    private void Update()
    {
        transform.position = _playerTransform.position +
                             (new Vector3(.5f, .5f, 0)) +
                             Vector3.forward * _cameraHeight;
    }
}