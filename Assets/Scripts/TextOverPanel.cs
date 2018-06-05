using UnityEngine;

public class TextOverPanel<T> : UiPanel<T> where T : MonoBehaviour
{
    [SerializeField] private float _textAnimDuration = 40f;
    [SerializeField] private float _fadeOutDuration = 2f;
    [SerializeField] private float _minTimeInPanel = 5f;
    private bool _canLeave;

    private void Awake()
    {
        InputController.Instance.RegisterCallback(NotificationType.OnCursorClick, OnCursorClick);
    }

    private void OnCursorClick()
    {
        if (_canLeave)
        {
            BeginRestart();
        }
    }

    protected void StartTextOver()
    {
        ShowPanel();
        Invoke(nameof(SetCanLeave), _minTimeInPanel);
        Invoke(nameof(BeginRestart), _textAnimDuration);
    }

    private void SetCanLeave()
    {
        _canLeave = true;
    }

    private void BeginRestart()
    {
        FadeOut.Instance.ShowPanel();
        Invoke(nameof(FinishTextOver), _fadeOutDuration);
    }

    protected virtual void FinishTextOver()
    {
        LevelManager.Instance.RestartGame();
    }
}