using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floatingText;

    private Coroutine _fadeCoroutine;
    private WaitForSeconds waitForSeconds;
    private float _fadeDuration;

    private FloatingTextPoolManager floatingTextPoolManager;

    private void Awake()
    {
        floatingText = GetComponent<TextMeshProUGUI>();

        waitForSeconds = new WaitForSeconds(0.1f);

        Initialize();
    }

    private void Start()
    {
        floatingTextPoolManager = FloatingTextPoolManager.Instance;
    }

    public void Initialize()
    {
        // 색상 초기화
        Color color = floatingText.color;
        color.a = 1.0f;
        floatingText.color = color;

        // Text 초기화
        floatingText.text = string.Empty;
    }

    public void SetText(string text)
    {
        this.floatingText.text = text;
    }

    public void SetColor(Color color)
    {
        this.floatingText.color = color;
    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void SetDuration(float duration)
    {
        _fadeDuration = duration;
        waitForSeconds = new WaitForSeconds(_fadeDuration);
    }

    public void StartFadeCoroutine(FloatingTextSO data)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeCoroutine(data));
    }

    public IEnumerator FadeCoroutine(FloatingTextSO data)
    {
        float fadeTime = _fadeDuration;
        float curTime = 0.0f;
        Color tempColor = floatingText.color;
        Vector3 targetPosition = Vector3.zero;
        float alpha = 0;

        while (curTime < fadeTime)
        {
            curTime += Time.deltaTime;

            float t = (curTime / fadeTime);

            // alpha 값 수정
            alpha = 1f - t;
            tempColor.a = alpha;
            floatingText.color = tempColor;

            // 위치 값 수정
            targetPosition = new Vector3(transform.position.x, transform.position.y + data.floatingDist, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            yield return null;
        }

        tempColor.a = 0;
        floatingText.color = tempColor;
        transform.position = targetPosition;

        // 반환
        floatingTextPoolManager.Release(data.textName, this.gameObject);
    }
}
