using UnityEngine;

public class CropController : BuildingController
{
    [SerializeField] private Sprite[] _growSprites;
    [SerializeField] private float _timeToGrow = 3;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _timeSincePlanted;
    private int _currentSpriteIndex = -1;
    private bool _grown;

    private void Update()
    {
        if (_grown) return;

        _timeSincePlanted += Time.deltaTime;
        var newSpriteIndex = Mathf.FloorToInt((_timeSincePlanted / _timeToGrow) * _growSprites.Length - 1);
        
        if (_currentSpriteIndex != newSpriteIndex)
        {
            _currentSpriteIndex = newSpriteIndex;
            _spriteRenderer.sprite = _growSprites[_currentSpriteIndex];
        }

        if (_timeSincePlanted < _timeToGrow) return;
        
        // Debug.Log("Plant " + GetHashCode() + " grown");
        _grown = true;
        _spriteRenderer.sprite = _growSprites[_growSprites.Length - 1];
        
        TownController.Instance.PushNewOrder(OrderType.HarvestPlant, BitMath.RoundToInt((Vector2)transform.position));
    }
}