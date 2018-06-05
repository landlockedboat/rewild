using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModePointer : UiPanel<BuildModePointer>
{
    public Vector2Int BuildingDimensions;
    [SerializeField] private GameObject _buildModePointerViewPrefab;
    private List<GameObject> _buildModePointerViews;

    [HideInInspector] public bool CanBuild;

    [SerializeField] private Transform _leftButtonTransform;
    [SerializeField] private Transform _rightButtonTransform;
    [SerializeField] private Transform _upButtonTransform;
    [SerializeField] private Transform _downButtonTransform;

    public Vector2Int BuildingPos => BitMath.RoundToInt((Vector2) transform.position);
    public Vector2 BuildingCenter => BuildingPos + (Vector2) BuildingDimensions / 2;

    private void Awake()
    {
        _buildModePointerViews = new List<GameObject>();
    }

    public override void ShowPanel()
    {
        foreach (var view in _buildModePointerViews)
        {
            Destroy(view);
        }

        _buildModePointerViews = new List<GameObject>();

        var offset = new Vector2(.5f, .5f);
        for (var i = 0; i < BuildingDimensions.x; i++)
        {
            for (var j = 0; j < BuildingDimensions.y; j++)
            {
                var pos = offset + new Vector2(i, j);

                var go =
                    Instantiate(_buildModePointerViewPrefab, UiPanelGameObject.transform, false);
                go.transform.localPosition = pos;
                _buildModePointerViews.Add(go);
            }
        }

        _rightButtonTransform.localPosition = new Vector2(BuildingDimensions.x, BuildingDimensions.y / 2f);
        _leftButtonTransform.localPosition = new Vector2(0, BuildingDimensions.y / 2f);
        _upButtonTransform.localPosition = new Vector2(BuildingDimensions.x / 2f, BuildingDimensions.y);
        _downButtonTransform.localPosition = new Vector2(BuildingDimensions.x / 2f, 0);

        CheckValidBuildingPos();

        base.ShowPanel();
    }

    private void CheckValidBuildingPos()
    {
        CanBuild = CheckCanBuild();
        ColorPointers(CanBuild ? Color.green : Color.red);
    }

    private void ColorPointers(Color color)
    {
        foreach (var pointer in _buildModePointerViews)
        {
            var image = pointer.GetComponent<Image>();
            image.color = color;
        }
    }

    public void MoveUp()
    {
        MovePointer(new Vector2(transform.position.x, transform.position.y + 1));
    }

    public void MoveDown()
    {
        MovePointer(new Vector2(transform.position.x, transform.position.y - 1));
    }

    public void MoveLeft()
    {
        MovePointer(new Vector2(transform.position.x - 1, transform.position.y));
    }

    public void MoveRight()
    {
        MovePointer(new Vector2(transform.position.x + 1, transform.position.y));
    }

    public void MovePointer(Vector2 position)
    {
        var pos = BitMath.RoundToInt(position);
        transform.position = (Vector2) pos;
        CheckValidBuildingPos();
    }

    private bool CheckCanBuild()
    {
        for (var i = 0; i < BuildingDimensions.x; i++)
        {
            for (var j = 0; j < BuildingDimensions.y; j++)
            {
                var checkPos = new Vector2Int(BuildingPos.x + i, BuildingPos.y + j);
                if (!CellMapController.Instance.IsWalkable(checkPos))
                {
                    return false;
                }
            }
        }

        return true;
    }
}