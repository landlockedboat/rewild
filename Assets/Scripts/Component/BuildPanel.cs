using TMPro;
using UnityEngine;

public class BuildPanel : UiPanel<BuildPanel>
{
    [SerializeField] private TextMeshProUGUI _moneyAmmountText;

    private void Start()
    {
        _moneyAmmountText.text = LevelConfiguration.Instance.Money.ToString();
        LevelConfiguration.Instance.RegisterCallback(NotificationType.OnMoneyAmmountChanged, OnMoneyAmmountChanged);
    }

    private void OnMoneyAmmountChanged()
    {
        _moneyAmmountText.text = LevelConfiguration.Instance.Money.ToString();
    }

    public override void ShowPanel()
    {
        MainHUD.Instance.HidePanel();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        MainHUD.Instance.ShowPanel();
        base.HidePanel();
    }
}