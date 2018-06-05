using UnityEngine;

public class UiPanel<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField] protected GameObject UiPanelGameObject;
    public bool IsPanelVisible =>  UiPanelGameObject.activeInHierarchy && UiPanelGameObject.activeSelf;

    public virtual void ShowPanel()
    {
        UiPanelGameObject.SetActive(true);
    }

    public virtual void HidePanel()
    {
        UiPanelGameObject.SetActive(false);
    }
}