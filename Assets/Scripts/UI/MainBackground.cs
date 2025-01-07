using UnityEngine;

public class MainBackground : MonoBehaviour
{
    public RectTransform cloud1; // 첫 번째 구름 이미지
    public RectTransform cloud2; // 두 번째 구름 이미지
    public float scrollSpeed = 0.02f; // 구름 이동 속도 (픽셀/초)
    public float imageWidth = 3300f; // 구름 이미지의 너비 (이미지 크기에 맞게 설정)

    private Vector2 startPosition1; // 첫 번째 구름의 시작 위치
    private Vector2 startPosition2; // 두 번째 구름의 시작 위치

    void Start()
    {
        // RectTransform의 초기 위치를 저장
        startPosition1 = cloud1.anchoredPosition;
        startPosition2 = cloud2.anchoredPosition;
    }

    void Update()
    {
        // 구름들을 왼쪽으로 이동
        cloud1.anchoredPosition += Vector2.right * scrollSpeed * Time.deltaTime;
        cloud2.anchoredPosition += Vector2.right * scrollSpeed * Time.deltaTime;

        // 첫 번째 구름이 화면 밖으로 나가면 오른쪽으로 이동
        if (cloud1.anchoredPosition.x <= startPosition1.x - imageWidth)
        {
            cloud1.anchoredPosition = new Vector2(cloud2.anchoredPosition.x + imageWidth, startPosition1.y);
        }

        // 두 번째 구름이 화면 밖으로 나가면 오른쪽으로 이동
        if (cloud2.anchoredPosition.x <= startPosition2.x - imageWidth)
        {
            cloud2.anchoredPosition = new Vector2(cloud1.anchoredPosition.x + imageWidth, startPosition2.y);
        }
    }
}