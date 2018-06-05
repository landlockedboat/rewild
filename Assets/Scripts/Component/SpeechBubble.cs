using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private Sprite _hungerSprite;
    [SerializeField] private Sprite _sleepinessSprite;
    [SerializeField] private Sprite _fearSprite;

    [SerializeField] private float _needChangeTime;


    private Dictionary<string, Sprite> _needSprites;
    private Dictionary<string, Need> _needs;

    [SerializeField] private SpriteRenderer _needsSpriteRenderer;
    [SerializeField] private SpriteRenderer _bubbleSpriteRenderer;

    private VillagerModel _villagerModel;

    private void Awake()
    {
        _villagerModel = transform.parent.GetComponent<VillagerModel>();
    }

    private void Start()
    {
        _needs = new Dictionary<string, Need>
        {
            {"Hunger", _villagerModel.Hunger},
            {"Sleepiness", _villagerModel.Sleepiness},
            {"Fear", _villagerModel.Fear}
        };
        
        _needSprites = new Dictionary<string, Sprite>
        {
            {"Hunger", _hungerSprite},
            {"Sleepiness", _sleepinessSprite},
            {"Fear", _fearSprite}
        };
        _bubbleSpriteRenderer.enabled = false;
        _needsSpriteRenderer.enabled = false;
        
        StartCoroutine(ChangeNeedSprite());
    }

    private IEnumerator ChangeNeedSprite()
    {
        while (true)
        {
            var needy = false;
            for (var i = 0; i < _needs.Count; i++)
            {
                var pair = _needs.ElementAt(i);
                var need = pair.Value;

                if (!need.IsNeeded())
                {
                    continue;
                }
                
                _bubbleSpriteRenderer.enabled = true;
                _needsSpriteRenderer.enabled = true;
                _needsSpriteRenderer.sprite = _needSprites[pair.Key];
                needy = true;
                yield return new WaitForSeconds(_needChangeTime);
            }

            if (!needy)
            {
                _bubbleSpriteRenderer.enabled = false;
                _needsSpriteRenderer.enabled = false;
            }
                
            yield return null;
        }
    }
}