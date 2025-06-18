using UnityEditor;
using UnityEngine;

public class LevelProgressEditor : EditorWindow
{
    [MenuItem("Tools/Reset Level Unlock Progress")]
    public static void ResetLevelProgress()
    {
        if (EditorUtility.DisplayDialog("Reset Progress",
            "Bạn có chắc chắn muốn xóa toàn bộ trạng thái mở khóa các màn chơi không?",
            "Có, reset", "Không"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Đã reset toàn bộ trạng thái mở khóa!");
        }
    }
}
