using UnityEngine;

public class Need
{
    private readonly float _increasePerSecond;
    private readonly float _normalTreshold;
    private readonly float _criticalTreshold;
    private float _ammount;

    private bool _turnedNeeded;
    private bool _turnedCritical;

    public Need(
        float secondsToFill = 4f,
        float normalTreshold = 0.6f,
        float criticalTreshold = 0.8f)
    {
        _increasePerSecond = 1 / secondsToFill;
        _normalTreshold = normalTreshold;
        _criticalTreshold = criticalTreshold;
    }

    public void Increase()
    {
        _ammount += _increasePerSecond * Time.deltaTime;
        if (_ammount >= 1)
        {
            _ammount = 1;
        }
    }

    public void Restore(float ammount)
    {
        _ammount -= ammount;
        if (_ammount <= 0)
        {
            _ammount = 0;
        }

        if (!IsCritical())
        {
            _turnedCritical = false;
        }
        if (!IsNeeded())
        {
            _turnedNeeded = false;
        }
    }

    public bool IsRestored()
    {
        return _ammount <= 0;
    }

    public bool IsNeeded()
    {
        return _ammount > _normalTreshold;
    }

    public bool IsCritical()
    {
        return _ammount > _criticalTreshold;
    }

    public bool HasJustTurnedCritical()
    {
        if (!IsCritical() || _turnedCritical)
        {
            return false;
        }

        _turnedCritical = true;
        return true;
    }

    public bool HasJustTurnedNeeded()
    {
        if (!IsNeeded() || _turnedNeeded)
        {
            return false;
        }

        _turnedNeeded = true;
        return true;
    }
}