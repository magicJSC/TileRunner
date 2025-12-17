using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static GameObject FindChild(GameObject go, string name = null)
    {
        if (go == null)
            return null;

        foreach (Transform component in go.GetComponentsInChildren<Transform>())
        {
            if (string.IsNullOrEmpty(name) | component.name == name)
                return component.gameObject;
        }

        return null;
    }

    public static T FindChild<T>(GameObject go, string name = null) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform transform = go.transform.GetChild(i);
            if (string.IsNullOrEmpty(name) || transform.name == name)
            {
                T component = transform.GetComponent<T>();
                if (component != null)
                    return component;
            }
        }
        foreach (T component in go.GetComponentsInChildren<T>())
        {
            if (string.IsNullOrEmpty(name) | component.name == name)
                return component;
        }

        return null;
    }

    public static T GetRandomToList<T>(List<T> list)
    {
        if (list.Count == 0)
            return default;

        return list[Random.Range(0, list.Count)];
    }
}
