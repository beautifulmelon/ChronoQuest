using UnityEngine;

public class Player_Past : MonoBehaviour
{
    public Transform Player;

    private Vector3[] pos = new Vector3[720]; // �� ������ ��ġ�� �������� �ʹٸ� �̰� ����
    private int currentIndex = 0;
    public int framesToGoBack; // �ڷ� �̵��� ������ ��
    int previousIndex = 0;
    float recordInterval = 0;
    private void Start()
    {
        // �ʱ� ��ġ ���
        for (int j = 0; j < 720; j++)
        {
            pos[j] = Player.position;
        }
    }

    private void Update()
    {
        recordInterval = recordInterval + Time.deltaTime;
        if(recordInterval > 0.01f)   
        {
            recordInterval = 0;
            currentIndex = (currentIndex + 1) % pos.Length;
            pos[currentIndex] = Player.position;
            previousIndex = (currentIndex - framesToGoBack + pos.Length) % pos.Length;
            transform.position = pos[previousIndex];
        }
        Debug.Log(Time.deltaTime);  
        // ���� ��ġ ���


        // A Ű�� ������ framesToGoBack ��ŭ ������ ��ġ�� �̵�
        if (Input.GetKeyDown(KeyCode.A))
        {
            // ���� �ε������� framesToGoBack ��ŭ �ڷ� �̵��Ͽ� ���� ��ġ�� ������
            
            Player.position = pos[previousIndex];
        }
    }
}