using System.IO;
using UnityEditor;
using UnityEngine;

public class HierarchyExporter
{
    [MenuItem("Tools/Export Hierarchy to Console")]
    static void ExportHierarchy()
    {
        GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            PrintHierarchy(root, 0);
        }
    }

    static void PrintHierarchy(GameObject obj, int indent)
    {
        string indentString = new string(' ', indent * 4);  // 4 spaces per indent
        string type = obj.GetComponent<Component>()?.GetType().Name ?? "GameObject";

        Debug.Log($"{indentString}{obj.name} [{type}]");

        foreach (Transform child in obj.transform)
        {
            PrintHierarchy(child.gameObject, indent + 1);
        }
    }

    [MenuItem("Tools/Export Hierarchy to File")]
    static void ExportHierarchyToFile()
    {
        string path = "Assets/hierarchy.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject root in roots)
            {
                WriteHierarchy(writer, root, 0);
            }
        }
        Debug.Log("Exported to Assets/hierarchy.txt");
    }

    static void WriteHierarchy(StreamWriter writer, GameObject obj, int indent)
    {
        string indentString = new string(' ', indent * 4);
        string type = obj.GetComponent<Component>()?.GetType().Name ?? "GameObject";

        writer.WriteLine($"{indentString}{obj.name} [{type}]");

        foreach (Transform child in obj.transform)
        {
            WriteHierarchy(writer, child.gameObject, indent + 1);
        }
    }
}
