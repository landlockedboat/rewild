using UnityEngine;
using UnityEngine.UI;

public class MoneyView : BitView
{
    [SerializeField] private Text _moneyText;

    private PlayerModel _playerModel;
    private NotificationManager _playerNotificationManager;

    private void Start()
    {
        _playerModel = PlayerModel.Instance;
        PlayerController.Instance.RegisterCallback(NotificationType.MoneyUpdated, OnPlayerMoneyUpdated);
        
        UpdateMoneyText();
    }

    private void OnPlayerMoneyUpdated()
    {
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        _moneyText.text = _playerModel.Money.ToString();
    }
}