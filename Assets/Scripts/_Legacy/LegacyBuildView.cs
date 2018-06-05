using UnityEngine;

public class LegacyBuildView : BitView
{
    [SerializeField] private BuildPointer _buildPointer;

    private void OnEnable()
    {
        _buildPointer.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _buildPointer.gameObject.SetActive(false);
    }
}