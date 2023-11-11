using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI; : 스코어가 바뀔 때마다 UI적 요소의 변화를 주기 위해 using을 해줘야된다.

public class Score : MonoBehaviour
{
    public static int score = 0;
    Text text = null;
    //static을 붙여줌으로써 다른 클래스에서도 score를 조절가능하다.

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = score.ToString();
        //GetComponent: Component를 들고와줌.
        //ToString : int형을 string으로 변환
    }
}
