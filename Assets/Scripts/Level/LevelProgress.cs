using UnityEngine;

public static class LevelProgress
{
    public static void UnlockLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName + "_Unlocked", 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(string levelName)
    {
        // Mặc định Level1 được mở sẵn
        return PlayerPrefs.GetInt(levelName + "_Unlocked", levelName == "Lv1" ? 1 : 0) == 1;
    }
}
