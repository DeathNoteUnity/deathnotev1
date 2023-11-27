using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class TalkData
{
    //id 0�̸� ����, 1�̸� ���� ���, 2�̸� ��� ���, 3�̸� ����ε� ������, 4�̸� ���� ���
    public int id;
    public string content;
    public string background;
    public int opp;
    public float delay;

    public TalkData(int id, string content)
    {
        this.id = id;
        this.content = content;
        this.background = null;
    }
    public TalkData(int id, string content, string background)
    {
        this.id = id;
        this.content = content;
        this.background = background;
    }
    public TalkData(int id, string content, string background, float delay)
    {
        this.id = id;
        this.content = content;
        this.background = background;
        this.delay = delay;
    }
}

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;
    [SerializeField] public RectTransform[] rects;
    [SerializeField] public Image[] images;
    [SerializeField] public Sprite[] sprites;
    [SerializeField] Text sayname;
    [SerializeField] Text content;
    [SerializeField] Animator[] textAnimation;
    [SerializeField] Button button;
    [SerializeField] Animator scriptBox;
    [SerializeField] Image background;
    AudioSource audioSource;

    Dictionary<int, List<TalkData>> scriptList;
    //�����׽��� 0�� ���� ���� 7�� 1~6���� ����
    public int storyId;


    void Awake()
    {
        //�ʱ�ȭ
        instance = this;
        //int�� ���丮�� ��ȣ
        scriptList = new Dictionary<int, List<TalkData>>();
        GenerateData();
        audioSource = GetComponent<AudioSource>();  
    }

    void GenerateData()
    {
        // ������ ������
        List<TalkData> openingData = new List<TalkData> {
            new TalkData(1, "�������� �̰� ������", "landscape"),
            new TalkData(2, "����! ����Ʈ�ȴ١�", "6"),
            new TalkData(1, "����Ʈ�� �� �������ֳס�","landscape"),
            new TalkData(1, "���Ǻ��� �ָ��� ���� �����Ʊ��������� ��"),
            new TalkData(1, "���������� ���ֳ� �غ������?��"),
            new TalkData(1, "���Ƴ�, �ƾ���� ���� �Ǳ��� �������ߡ�������"),
            new TalkData(4, "[[ �ۢ� - ! ]]          "),
            new TalkData(1, "����, ���� �̰ǡ�����!?��", "landscape"),
            new TalkData(1, "���񡤡��������ٷ�!��"),
            new TalkData(1, "���������� �Ǻ��� �����϶�� �ǰ�?��"),
            new TalkData(1, "���� �� ����, �׷� �ѹ� �����غ��������"),
        };
        scriptList.Add(0, openingData);

        List<TalkData> opening2Data = new List<TalkData> {
            new TalkData(1, "���������������̶� ���� ���� ������.��", "landscape"),
            new TalkData(1, "����, ���� �����ϱ� ����� �ٷ� �ʷ� �����Ӿ�?��"),
            new TalkData(2, "���ʡ�����! ��� ������ �ҷ��� ����?��"),
            new TalkData(1, "��������!  �ʡ����� �� ����!��"),
            new TalkData(2, "�������� �پ ������ �־�߸� ��Ÿ���µ������� ��������"),
            new TalkData(2, "���� �Ǻ��� �����ϴ� ��(��)���ߡ�"),
            new TalkData(2, "��������, �Ǻ��� �� �������� ���ɵ��� �Ի��� ������� ���Ҿ������"),
            new TalkData(2, "���Ǻ��� ã�� ������ ����~!��"),
            new TalkData(2, "���Ϳ��� ���ɵ��� ���� �� �����ž�!��"),
            new TalkData(2, "������ �Ǻ��� ã�� ������ ������!��")
        };
        scriptList.Add(-1, opening2Data);

        List<TalkData> soulGetData = new List<TalkData> {
            new TalkData(0, "��0����� ������ �����!��", "landscape"),
            new TalkData(0, "��1����硻"),
            new TalkData(0, "��2����硻"),
            new TalkData(0, "��3����硻"),
            new TalkData(0, "��4����硻"),
        };
        scriptList.Add(2, soulGetData);
        //�̷��� �� ���丮 ������ ��� �ְ� storyId�� ����ȣ �־ ����ϸ� ��.
        //����1������ ���丮 4�� ���ߵǴϱ� ��� ������ �־���� �������� ����ϰ��ұ�?
        //����°���

        List<TalkData> endingData = new List<TalkData>
        {
            new TalkData(2, "������ ��� �Ǻ��� ã�ұ���!��"),
            new TalkData(1, "��������  ������  !��", "landscape"),
            new TalkData(1, "���� �뷡������ ��� �ͼ��ѵ�?��"),
            new TalkData(1, "������ � �� ���� ��� �뷡�ݾ�?!��"),
            new TalkData(2, "������ ����� ���ö���?��"),
            new TalkData(2, "���� ������ �����. ��, ����� ���̾ߡ�"),
            new TalkData(2, "���� �и� ���� �뷡�� ���� �� �����ž�...��"),
            new TalkData(2, "�������� ������...��"),
            new TalkData(2, "���׷� �̸���������"),
            new TalkData(1, "���ȳ硤������"),
        };
        scriptList.Add(7, endingData);
    }


    public void SetBackground(string bg)
    {
        background.sprite = Resources.Load<Sprite>("Image/Background/" + bg);
    }

    public int getStoryId()
    {
        return storyId;
    }

    public void BoxAppear(bool param)
    {
        //��� �ڽ� ��Ÿ����.
        scriptBox.SetBool("isShow", param);
    }

    public void click()
    {
        //��� �����ϸ� ��ư ��Ȱ��ȭ
        button.interactable = false;
        audioSource.Play();
    }

    public TalkData getTalk(int id, int idx)
    {
        if (idx == scriptList[id].Count) 
            return null; //��ȭ ������ null
        List<TalkData> data = scriptList[id];
        return data[idx];
    }

    public bool ChangeUI(int id, int idx)
    {
        TalkData data = getTalk(id, idx);
        if (data == null) return true;

        if(data.id == 1)
        {
            images[0].color = new Color(255, 255, 255, 1);
            rects[0].SetAsLastSibling();
            images[1].color = new Color(255, 255, 255, 0.5f);
            rects[1].SetAsFirstSibling();
            //sayname.text = UserManager.instance.userData.nickname;
            sayname.text = "������Ʈ�÷��̾�";
            sayname.alignment = TextAnchor.MiddleLeft;
            content.alignment = TextAnchor.UpperLeft;

            StartCoroutine(Speaking(textAnimation[0]));

        }

        else
        {
            images[1].sprite = sprites[data.id - 1];
            images[1].color = new Color(255, 255, 255, 1);
            rects[1].SetAsLastSibling();
            for (int i = 1; i <= 3; i++)
            {
                if (i == data.id - 1)
                {
                    if (i == 1) sayname.text = "�Ǹ�";
                    else if (i == 2) sayname.text = "???";
                    else if (i == 3) sayname.text = "��¦��";
                }
            }
            images[0].color = new Color(255, 255, 255, 0.5f);
            rects[0].SetAsFirstSibling();

            
            sayname.alignment = TextAnchor.MiddleRight;
            content.alignment = TextAnchor.UpperRight;

            StartCoroutine(Speaking(textAnimation[1]));
            

        }
        if(data.background != null)
        {
            background.sprite = Resources.Load<Sprite>("Image/Background/" + data.background);
        }

        StartCoroutine(Typing(data.content,data.delay));
        
        return false;
    }

    IEnumerator Speaking(Animator animator)
    {
        ;
        yield return new WaitForSeconds(1);
        animator.SetBool("turn", false);
    }

    IEnumerator Typing(string str, float delay)
    {
        yield return new WaitForSeconds(delay);
        content.text = null;
        for (int i = 0; i < str.Length; i++)
        {
            content.text += str[i];
            yield return new WaitForSeconds(0.06f);
        }
        //��� ������ �ٽ� ��ư Ȱ��ȭ
        button.interactable = true;
    }

}
