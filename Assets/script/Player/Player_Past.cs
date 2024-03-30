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

    private List<PositionSnapshot> previousPositions = new List<PositionSnapshot>(); // 이전 위치를 기록하기 위한 리스트
    private float maxRecordDuration = 3f; // 최대 기록 기간(초)

    void Update()
    {
        // 매 프레임마다 현재 위치 업데이트
        RecordCurrentPosition();
    }

    void RecordCurrentPosition()
    {
        // 현재 위치와 타임스탬프를 기록
        PositionSnapshot snapshot = new PositionSnapshot();
        snapshot.position = transform.position;
        snapshot.timestamp = Time.time;
        previousPositions.Add(snapshot);

        // 이전 위치 중 일정 시간 이전의 위치만 남기기
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
        // 현재 시간을 기준으로 일정 시간 이전의 위치를 찾아 반환
        float currentTime = Time.time;
        foreach (var snapshot in previousPositions)
        {
            if (currentTime - snapshot.timestamp <= maxRecordDuration)
            {
                return snapshot.position;
            }
        }
        // 이전 위치가 없는 경우 현재 위치를 반환
        return transform.position;
    }
}

public class Example : MonoBehaviour
{
    public GameObject targetObject; // 다른 오브젝트의 참조
    
    void Update()
    {
        targetObject = GameObject.Find("Player");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 다른 오브젝트의 3초 전 위치를 얻어옴
            Vector3 previousPosition = GetPreviousPosition(targetObject);
            Debug.Log("3초 전 위치: " + previousPosition);
        }
    }

    Vector3 GetPreviousPosition(GameObject obj)
    {
        // 다른 오브젝트의 위치를 기록하는 컴포넌트를 가져옴
        Player_Past recorder = obj.GetComponent<Player_Past>();
        if (recorder != null)
        {
            // 다른 오브젝트의 3초 전 위치를 반환
            return recorder.GetPreviousPosition();
        }
        else
        {
            Debug.LogWarning("PositionRecorder 컴포넌트를 찾을 수 없습니다.");
            return Vector3.zero;
        }
    }
}
