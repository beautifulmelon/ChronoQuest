using UnityEngine;

public class Player_Past : MonoBehaviour
{
    public Transform Player;

    private Vector3[] pos = new Vector3[720]; // 더 나중의 위치를 가져오고 싶다면 이걸 높게
    private int currentIndex = 0;
    public int framesToGoBack; // 뒤로 이동할 프레임 수
    int previousIndex = 0;
    float recordInterval = 0;
    bool OnPast = true;
    public float PastCoolTime = 5f;
    private void Start()
    {
        // 초기 위치 기록
        for (int j = 0; j < 720; j++)
        {
            pos[j] = Player.position;//초기 값 설정
        }
    }

    private void Update()
    {
        recordInterval = recordInterval + Time.deltaTime;
        if(recordInterval > 0.01f)   //0.01초마다 실행
        {
            recordInterval = 0;
            currentIndex = (currentIndex + 1) % pos.Length;
            pos[currentIndex] = Player.position;
            previousIndex = (currentIndex - framesToGoBack + pos.Length) % pos.Length;
            transform.position = pos[previousIndex];
        }
        // 현재 위치 기록
        if(OnPast == false)
        {
            PastCoolTime = PastCoolTime - Time.deltaTime;
            if(PastCoolTime <= 0)
            {
                PastCoolTime = 5f;
                OnPast = true;
            }
        }

        // A 키를 누르면 framesToGoBack 만큼 이전의 위치로 이동
        if (Input.GetKeyDown(KeyCode.A) && OnPast == true)
        {
            // 현재 인덱스에서 framesToGoBack 만큼 뒤로 이동하여 이전 위치를 가져옴
            
            Player.position = pos[previousIndex];
            OnPast = false;
            for (int j = 0; j < 720; j++)
            {
                pos[j] = Player.position;
            }
        }
    }
}