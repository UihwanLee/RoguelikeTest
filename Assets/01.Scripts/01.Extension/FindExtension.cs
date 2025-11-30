using UnityEngine;

public static class FindExtension
{
    /// <summary>
    /// T 자식 컴포넌트 반환하는 확장 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T FindChild<T>(this Transform transform, string name) where T : Component
    {
        T[] children = transform.GetComponentsInChildren<T>();
        foreach(T child in children)
        {
            if(child.name == name)
            {
                return child;
            }
        }

        return null;
    }
}
