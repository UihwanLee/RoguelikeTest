using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPoolManager : MonoBehaviour
{
    private static FloatingTextPoolManager _instance;

    private void Awake()
    {
        if(_instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static FloatingTextPoolManager Instance
    {
        get
        {
            if( _instance == null )
            {
                return null;
            }
            return _instance;
        }
    }

    private FloatingTextPoolManager() { }

    private List<FloatingTextPool> floatingTexts = new List<FloatingTextPool>();

    public void Add(FloatingTextPool pool)
    {
        floatingTexts.Add(pool);
    }

    public void SpawnText(string key, string text, Transform target, Color? color = null)
    {
        // Key로 찾기
        foreach (var floatText in floatingTexts)
        {
            if (floatText.Key == key)
            {
                floatText.SpawnText(text, target, color);
                return;
            }
        }

        Debug.Log($"해당 {key} Pool을 찾을 수 없습니다!");
    }

    public void SpawnText(TextType type, string text, Transform target, Color? color = null)
    {
        // Type으로 찾기
        foreach (var floatText in floatingTexts)
        {
            if (floatText.Type == type)
            {
                floatText.SpawnText(text, target, color);
                return;
            }
        }

        Debug.Log($"해당 {type} Pool을 찾을 수 없습니다!");
    }

    public void Release(string key, GameObject obj)
    {
        foreach (var floatText in floatingTexts)
        {
            if (floatText.Key == key)
            {
                floatText.Release(obj);
                return;
            }
        }
    }
}
