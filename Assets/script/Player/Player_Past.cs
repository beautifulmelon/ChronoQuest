using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Past : MonoBehaviour
{
    private struct PositionSnapshot
    {
        public Vector3 position;
        public float timestamp;
    }

    private List<PositionSnapshot> previousPositions = new List<PositionSnapshot>(); // ���� ��ġ�� ����ϱ� ���� ����Ʈ
    private float maxRecordDuration = 3f; // �ִ� ��� �Ⱓ(��)

    void Update()
    {
        // �� �����Ӹ��� ���� ��ġ ������Ʈ
        RecordCurrentPosition();
    }

    void RecordCurrentPosition()
    {
        // ���� ��ġ�� Ÿ�ӽ������� ���
        PositionSnapshot snapshot = new PositionSnapshot();
        snapshot.position = transform.position;
        snapshot.timestamp = Time.time;
        previousPositions.Add(snapshot);

        // ���� ��ġ �� ���� �ð� ������ ��ġ�� �����
        float currentTime = Time.time;
        for (int i = previousPositions.Count - 1; i >= 0; i--)
        {
            if (currentTime - previousPositions[i].timestamp > maxRecordDuration)
            {
                previousPositions.RemoveAt(i);
            }
            else
            {
                break;
            }
        }
    }

    public Vector3 GetPreviousPosition()
    {
        // ���� �ð��� �������� ���� �ð� ������ ��ġ�� ã�� ��ȯ
        float currentTime = Time.time;
        foreach (var snapshot in previousPositions)
        {
            if (currentTime - snapshot.timestamp <= maxRecordDuration)
            {
                return snapshot.position;
            }
        }
        // ���� ��ġ�� ���� ��� ���� ��ġ�� ��ȯ
        return transform.position;
    }
}

public class Example : MonoBehaviour
{
    public GameObject targetObject; // �ٸ� ������Ʈ�� ����
    
    void Update()
    {
        targetObject = GameObject.Find("Player");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �ٸ� ������Ʈ�� 3�� �� ��ġ�� ����
            Vector3 previousPosition = GetPreviousPosition(targetObject);
            Debug.Log("3�� �� ��ġ: " + previousPosition);
        }
    }

    Vector3 GetPreviousPosition(GameObject obj)
    {
        // �ٸ� ������Ʈ�� ��ġ�� ����ϴ� ������Ʈ�� ������
        Player_Past recorder = obj.GetComponent<Player_Past>();
        if (recorder != null)
        {
            // �ٸ� ������Ʈ�� 3�� �� ��ġ�� ��ȯ
            return recorder.GetPreviousPosition();
        }
        else
        {
            Debug.LogWarning("PositionRecorder ������Ʈ�� ã�� �� �����ϴ�.");
            return Vector3.zero;
        }
    }
}
