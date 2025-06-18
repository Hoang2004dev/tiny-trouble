using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCornerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Container chứa 4 góc highlight")]
    public GameObject cornerContainer;

    private void Start()
    {
        if (cornerContainer != null)
            cornerContainer.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cornerContainer != null)
            cornerContainer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cornerContainer != null)
            cornerContainer.SetActive(false);
    }
}
