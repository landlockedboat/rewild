using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField] private Text _moneyText;

    void Start()
    {
        OnMoneyAmmountChanged();
        LevelConfiguration.Instance.RegisterCallback(NotificationType.OnMoneyAmmountChanged, OnMoneyAmmountChanged);
    }

    private void OnMoneyAmmountChanged()
    {
        _moneyText.text = LevelConfiguration.Instance.Money.ToString();
    }
}