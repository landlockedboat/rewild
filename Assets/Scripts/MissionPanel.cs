using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionPanel : UiPanel<MissionPanel>
{
    [SerializeField] private Text _missionText;
    [SerializeField] private Text _missionTitle;
    [SerializeField] private Text _missionProgress;

    [SerializeField] private Button _missionButton;

    private bool _missionsLoaded;

    private void Awake()
    {
        MissionsManager.Instance.RegisterCallback(NotificationType.OnMissionsLoaded, OnMissionsLoaded);
    }

    private void OnMissionsLoaded()
    {
        _missionsLoaded = true;
        if (IsPanelVisible)
        {
            UpdatePanel();
        }
    }

    public override void ShowPanel()
    {
        if (!_missionsLoaded)
        {
            return;
        }

        UpdatePanel();
        base.ShowPanel();
    }

    private void UpdatePanel()
    {
        if (!MissionsManager.Instance.HasMissions)
        {
            return;
        }

        var currentMission = MissionsManager.Instance.GetCurrentMission();
        _missionTitle.text = currentMission.Title[GameApplication.Instance.CurrentLanguage];
        _missionText.text = currentMission.Text[GameApplication.Instance.CurrentLanguage];
        _missionProgress.text = $"{currentMission.CurrentAmmount}/{currentMission.ObjectiveAmmount}";

        string buttonText;
        UnityAction buttonCallback = () => { };
        var buttonColor = Color.white;
        var isButtonActive = true;

        if (!currentMission.HasStarted)
        {
            buttonText = "start_label";
            buttonCallback = () =>
            {
                MissionsManager.Instance.StartCurrentMission();
                UpdatePanel();
            };
        }
        else if (!currentMission.IsCompleted)
        {
            buttonText = "in_progress_label";
            isButtonActive = false;
        }
        else //Mission is completed and, obviously, has started
        {
            buttonText = "next_mission_label";
            buttonCallback = () =>
            {
                MissionsManager.Instance.CompleteCurrentMission();
                UpdatePanel();
            };
            buttonColor = Color.green;
        }

        _missionButton.GetComponentInChildren<Text>().text = LocalisationManager.Instance.GetTranslation(buttonText);
        _missionButton.onClick.RemoveAllListeners();
        _missionButton.onClick.AddListener(buttonCallback);
        _missionButton.image.color = buttonColor;
        _missionButton.interactable = isButtonActive;
    }
}