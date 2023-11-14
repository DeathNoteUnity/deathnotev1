using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveChar : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector2 direction;
    private float changeDirectionTime = 2.0f;
    public Transform boundaryTransform;
    private Vector2 boundarySize;
    private bool isMoving = true;
    private MoveChar selectedCharacter;
    public Button playButton;
    private bool isDragging = false;
    private Coroutine pulseCoroutine; // Pulse �ִϸ��̼��� ���� �ڷ�ƾ ����

    void Start()
    {
        if (boundaryTransform.GetComponent<SpriteRenderer>() != null)
        {
            boundarySize = boundaryTransform.GetComponent<SpriteRenderer>().bounds.size;
        }

        ChooseDirection();
        InvokeRepeating("ChooseDirection", changeDirectionTime, changeDirectionTime);
    }

    IEnumerator PulseButtonAnimation(Button button, bool isOverButton)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector3 originalScale = rectTransform.localScale;
        Vector3 minScale = new Vector3(1f, 1f, 1f);
        Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);

        while (true)
        {
            // ��ư ���� ���� ���� ũ�⸦ �����մϴ�.
            if (isOverButton)
            {
                rectTransform.localScale = maxScale;
            }
            else
            {
                // ��ư���� ����� ���� �޽� �ִϸ��̼��� �����մϴ�.
                float scale = Mathf.PingPong(Time.time, maxScale.x - minScale.x) + minScale.x;
                rectTransform.localScale = new Vector3(scale, scale, 1f);
            }
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0f;

            Collider2D collider = Physics2D.OverlapPoint(new Vector2(touchPosition.x, touchPosition.y));
            if (collider != null && collider.GetComponent<MoveChar>() == this)
            {
                isDragging = true;
                isMoving = false;
                selectedCharacter = this;
                playButton.gameObject.SetActive(true); // ��ư Ȱ��ȭ
                pulseCoroutine = StartCoroutine(PulseButtonAnimation(playButton));
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            DragCharacter();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                isMoving = true;

                // ��ũ�� ��ǥ�� ����
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(selectedCharacter.transform.position);

                if (IsOverButton(screenPoint, playButton))
                {
                    SceneManager.LoadScene("PlayScene");
                }
                else
                {
                    // PlayBtn �ִϸ��̼��� �����մϴ�.
                    if (pulseCoroutine != null)
                    {
                        StopCoroutine(pulseCoroutine);
                        pulseCoroutine = null;
                    }
                    playButton.GetComponent<RectTransform>().localScale = Vector3.one; // ��ư ������ �ʱ�ȭ
                    playButton.gameObject.SetActive(false); // ��ư�� ����ϴ�.
                }

                selectedCharacter = null;  // ���� �����մϴ�.
            }
        }

        if (isMoving)
        {
            MoveCharacter();
        }
    }


    void DragCharacter()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0f;
        transform.position = touchPosition;
    }

    bool IsOverButton(Vector2 screenPosition, Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Camera camera = null;

        // Canvas�� Render Mode�� Screen Space - Overlay���� Screen Space - Camera������ ���� 
        // ������ ī�޶� ã�Ƽ� �Ѱ���� �մϴ�.
        Canvas canvas = button.GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            camera = null; // Overlay ��忡���� ī�޶� ������� �ʽ��ϴ�.
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            camera = canvas.worldCamera; // Camera ��忡���� �ش� Canvas�� ī�޶� ����մϴ�.
        }

        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, camera);
    }

    IEnumerator PulseButtonAnimation(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector3 minScale = new Vector3(1f, 1f, 1f);
        Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);
        Vector3 fixedScale = new Vector3(1.2f, 1.2f, 1f); // ���� ũ��

        while (true)
        {
            // �巡�� ���̰� ��ư ���� ���� ���� ���� �ִϸ��̼� ����
            if (isDragging && !IsOverButton(Camera.main.WorldToScreenPoint(selectedCharacter.transform.position), playButton))
            {
                float scale = Mathf.PingPong(Time.time, maxScale.x - minScale.x) + minScale.x;
                rectTransform.localScale = new Vector3(scale, scale, 1f);
            }
            else
            {
                rectTransform.localScale = fixedScale; // �巡�� ���̰� ��ư ���� ���� ���� ���� ũ��
            }

            yield return null;
        }
    }


    void MoveCharacter()
    {
        Vector3 moveAmount = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        Vector3 newPos = transform.position + moveAmount;
        newPos.x = Mathf.Clamp(newPos.x, -boundarySize.x / 2, boundarySize.x / 2);
        newPos.y = Mathf.Clamp(newPos.y, -boundarySize.y / 2, boundarySize.y / 2);
        transform.position = newPos;
    }

    void ChooseDirection()
    {
        float h = Random.Range(-1f, 1f);
        float v = Random.Range(-1f, 1f);
        direction = new Vector2(h, v).normalized;
    }
}