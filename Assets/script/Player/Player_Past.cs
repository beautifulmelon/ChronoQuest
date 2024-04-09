using UnityEngine;

public class Player_Past : MonoBehaviour
{
    public Transform Player;

    private Vector3[] pos = new Vector3[720]; // �� ������ ��ġ�� �������� �ʹٸ� �̰� ����
    private int currentIndex = 0;
    public int framesToGoBack; // �ڷ� �̵��� ������ ��
    int previousIndex = 0;
    float recordInterval = 0;
    bool OnPast = true;
    public float PastCoolTime = 5f;
    private void Start()
    {
        // �ʱ� ��ġ ���
        for (int j = 0; j < 720; j++)
        {
            pos[j] = Player.position;//�ʱ� �� ����
        }
    }

    private void Update()
    {
        recordInterval = recordInterval + Time.deltaTime;
        if(recordInterval > 0.01f)   //0.01�ʸ��� ����
        {
            recordInterval = 0;
            currentIndex = (currentIndex + 1) % pos.Length;
            pos[currentIndex] = Player.position;
            previousIndex = (currentIndex - framesToGoBack + pos.Length) % pos.Length;
            transform.position = pos[previousIndex];
        }
        // ���� ��ġ ���
        if(OnPast == false)
        {
            PastCoolTime = PastCoolTime - Time.deltaTime;
            if(PastCoolTime <= 0)
            {
                PastCoolTime = 5f;
                OnPast = true;
            }
        }

        // A Ű�� ������ framesToGoBack ��ŭ ������ ��ġ�� �̵�
        if (Input.GetKeyDown(KeyCode.A) && OnPast == true)
        {
            // ���� �ε������� framesToGoBack ��ŭ �ڷ� �̵��Ͽ� ���� ��ġ�� ������
            
            Player.position = pos[previousIndex];
            OnPast = false;
            for (int j = 0; j < 720; j++)
            {
                pos[j] = Player.position;
            }
        }
    }
}