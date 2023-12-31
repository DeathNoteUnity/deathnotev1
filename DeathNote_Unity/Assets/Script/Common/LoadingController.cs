using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    //함수와 변수를 스태틱으로 선언하면 로딩신으로 넘어오지 않아서 로딩컨트롤러가 부착된 오브젝트가 생성되지 않아도 로딩씬 컨트롤로러의 함수명을 통해 호출해 사용한다.
    static string nextScene;

    [SerializeField] Image progressBar;

    public static void LoadScene(string sceneName)
    {
        //로딩씬 불러온다.
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0;

        yield return new WaitForSeconds(2.0f);

        //비동기 방식이라서 씬을 불러오는 도중에 다른 작업이 가능하다
        AsyncOperation op  = SceneManager.LoadSceneAsync(nextScene);
        //씬 로딩을 잠깐 멈춰서 로딩씬이 너무 빨리 넘어가지 않게 한다(페이크 로딩)
        //씬을 90%까지만 로딩한다.
        op.allowSceneActivation = false;


        //로딩 상황 표시해주는 코드
        float timer = 0f;
        bool firstVal = false;
        while(!op.isDone) //씬 로딩이 끝나지 않은 상태라면 계속 반복한다.
        {
            //유니티 엔진에 제어권을 넘긴다. 
            //반복이 돌 때마다 제어권을 넘기지 않으면 반복문이 끝나기 전에는 화면이 갱신되지 않아서 진행바가 차오르지 않을 것
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if(progressBar.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
