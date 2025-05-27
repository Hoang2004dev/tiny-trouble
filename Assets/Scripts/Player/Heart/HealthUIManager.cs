using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<HeartUI> hearts = new();

    public void SetupHearts(int maxHealth)
    {
        foreach (Transform child in heartContainer)
            Destroy(child.gameObject);
        hearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heartGO = Instantiate(heartPrefab, heartContainer);
            HeartUI heart = heartGO.GetComponent<HeartUI>();
            heart.SetFull();
            hearts.Add(heart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].SetFull();
            else
                hearts[i].SetEmpty();
        }
    }
}
