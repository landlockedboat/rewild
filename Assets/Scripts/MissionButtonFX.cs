using UnityEngine;

public class MissionButtonFX : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private MissionsManager _missionsManager;

    private void Start()
    {
        _missionsManager = MissionsManager.Instance;
        
        _missionsManager.RegisterCallback(NotificationType.OnMissionToStart,
            () => { _animator.SetBool("IsMissionToStart", true); });
        _missionsManager.RegisterCallback(NotificationType.OnMissionStarted,
            () => { _animator.SetBool("IsMissionToStart", false); });
        _missionsManager.RegisterCallback(NotificationType.OnMissionObjectivesReached,
            () =>
            {
                print("Mission objectives reached");
                _animator.SetBool("IsMissionCompleted", true);
            });
        _missionsManager.RegisterCallback(NotificationType.OnMissionCompleted,
            () => { _animator.SetBool("IsMissionCompleted", false); });
        
        _animator.SetBool("IsMissionToStart", true);
    }
}