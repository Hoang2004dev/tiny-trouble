using UnityEngine;

public class MenuPanelAnimator : MonoBehaviour
{
    public GameObject panel;
    public float duration = 0.3f;
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = panel.transform.localScale;
    }

    public void Open()
    {
        panel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Scale(panel.transform, Vector3.zero, originalScale, duration));
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleAndDisable(panel.transform, panel.transform.localScale, Vector3.zero, duration));
    }

    private System.Collections.IEnumerator Scale(Transform target, Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0;
        target.localScale = from;
        while (elapsed < time)
        {
            target.localScale = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = to;
    }

    private System.Collections.IEnumerator ScaleAndDisable(Transform target, Vector3 from, Vector3 to, float time)
    {
        float elapsed = 0;
        target.localScale = from;
        while (elapsed < time)
        {
            target.localScale = Vector3.Lerp(from, to, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = to;
        panel.SetActive(false);
    }
}
