using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;
    private static bool shouldFadeOut = true;

    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }

    void OnEnable()
    {
        if (shouldFadeOut)
        {
            StartCoroutine(Fade(FadeDirection.Out));
        }
        shouldFadeOut = true; // 다음 번에는 페이드 아웃 실행
    }

    public static void DisableNextFadeOut()
    {
        shouldFadeOut = false;
    }

    // 페이드 인/아웃 코루틴
    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }

    // UI 이미지의 알파 값을 설정
    public void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }

    // 다른 씬으로 페이드 아웃하며 전환
    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad)
    {
        yield return Fade(fadeDirection);
        SceneManager.LoadScene(sceneToLoad);
    }
}