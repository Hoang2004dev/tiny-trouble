using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetFull()
    {
        image.sprite = fullHeart;
    }

    public void SetEmpty()
    {
        image.sprite = emptyHeart;
    }
}
