using UnityEngine;

public class FillContent : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;

    [SerializeField] protected float ItemPrefabHeight = 250;
    [SerializeField] protected float SpaceBetweenItems = 50;

    private RectTransform _rectTransform;
    private float _offset;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        DestroyContents();
        CreateMenuFromAllowedBuildings();
    }

    private void DestroyContents()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void CreateMenuFromAllowedBuildings()
    {
        var availableBuildingTypes = LevelConfiguration.Instance.AvailableBuildings;
        _offset = -ItemPrefabHeight / 2;

        foreach (var type in availableBuildingTypes)
        {
            var position = new Vector2(0, _offset);

            var go = Instantiate(_buttonPrefab, _rectTransform);
            
            go.GetComponent<BuildItem>().Init(type, position);

            _offset -= SpaceBetweenItems + ItemPrefabHeight;
        }

        var contentSize = _rectTransform.sizeDelta;
        _rectTransform.sizeDelta = new Vector2(contentSize.x, -_offset);
    }

    
}