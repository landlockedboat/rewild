using UnityEngine;
using UnityEngine.PostProcessing;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] private AnimationCurve _red;
    [SerializeField] private AnimationCurve _green;
    [SerializeField] private AnimationCurve _exposure;
    [SerializeField] private AnimationCurve _saturation;

    private PostProcessingProfile _profile;
    private ColorGradingModel.Settings _settings;
    private float _secondsInADay;
    private float _currentSeconds;
    private float _nightAmmount;
    private bool _isDarkening = true;

    private void Awake()
    {
        _profile = GetComponent<PostProcessingBehaviour>().profile;
        _settings = _profile.colorGrading.settings;
    }

    private void Start()
    {
        _secondsInADay = LevelConfiguration.Instance.SecondsInADay;
    }

    private void Update()
    {
        _nightAmmount = _currentSeconds / _secondsInADay;
        ProcessDay();
        if (_isDarkening)
        {
            _currentSeconds += Time.deltaTime;
            if (!(_currentSeconds >= _secondsInADay))
            {
                return;
            }

            _currentSeconds = _secondsInADay;
            _isDarkening = false;
        }
        else
        {
            _currentSeconds -= Time.deltaTime;
            if (!(_currentSeconds <= 0))
            {
                return;
            }

            _currentSeconds = 0;
            _isDarkening = true;
        }
    }

    private void ProcessDay()
    {
        _settings.basic.postExposure = _exposure.Evaluate(_nightAmmount);
        _settings.basic.saturation = _saturation.Evaluate(_nightAmmount);
        _settings.channelMixer.red = Vector3.right * _red.Evaluate(_nightAmmount);
        _settings.channelMixer.green = Vector3.up * _green.Evaluate(_nightAmmount);

        _profile.colorGrading.settings = _settings;
    }
}