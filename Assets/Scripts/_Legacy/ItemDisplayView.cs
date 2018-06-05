using UnityEngine;

public class ItemDisplayView : BitView
{
    [SerializeField] protected GameObject ItemPrefab;

    [Header("Internal References")] [SerializeField]
    protected RectTransform ContentTransform;

    [SerializeField] protected float ItemPrefabHeight = 250;
    [SerializeField] protected float SpaceBetweenItems = 50;
    
    protected Item[] ItemsToDisplay;

    protected void DisplayItems()
    {
        var n = ContentTransform.transform.childCount;

        for (var i = 0; i < n; i++)
        {
            Destroy(ContentTransform.transform.GetChild(i).gameObject);
        }

        var offset = -ItemPrefabHeight / 2;

        foreach (var item in ItemsToDisplay)
        {
            var position = new Vector2(0, offset);

            var go = Instantiate(ItemPrefab, ContentTransform);

            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;

            var itemDetailView = go.GetComponent<ItemDetailView>();
            itemDetailView.Item = item;
            offset -= SpaceBetweenItems + ItemPrefabHeight;
        }

        var contentSize = ContentTransform.sizeDelta;
        ContentTransform.sizeDelta = new Vector2(contentSize.x, -offset);
    }
}

public class ItemDetailView : BitView
{
    public Item Item;
}