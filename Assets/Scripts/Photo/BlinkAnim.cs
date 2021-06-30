using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState {
    FadeIn = 0, FadeOut, FadeInOut
}

public class BlinkAnim : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image;
    private FadeState fadeState;


    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Fade(1, 0));
        yield return StartCoroutine(Fade(0, 1));
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while(percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }
}
