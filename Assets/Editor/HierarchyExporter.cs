using System.IO;
using UnityEditor;
using UnityEngine;

public class HierarchyExporter
{
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

        writer.WriteLine($"{indentString}{obj.name}");

        Component[] components = obj.GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (comp == null) continue;
            writer.WriteLine($"{indentString}    - {comp.GetType().Name}");
        }

        foreach (Transform child in obj.transform)
        {
            WriteHierarchy(writer, child.gameObject, indent + 1);
        }
    }
}
