using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public float detectionRadius = 3f;    // 부채꼴의 반경
    public float detectionAngle = 45f;     // 부채꼴의 각도
    public LayerMask enemyLayer;           // 적 레이어 마스크

    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        // 플레이어의 위치를 기준으로 detectionRadius 반경 내에 있는 모든 적 찾기
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        foreach (var enemy in enemiesInRange)
        {
            // 플레이어의 전방과 적 사이의 방향 벡터 계산
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // 플레이어 전방(현재 방향)과 적 방향 벡터 간의 각도 계산
            float angleBetweenPlayerAndEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            // 부채꼴 각도 범위 내에 있는지 확인
            if (angleBetweenPlayerAndEnemy <= detectionAngle / 2)
            {
                // 적이 부채꼴 안에 있음
                Debug.Log("Enemy detected: " + enemy.name);
                // 여기에서 원하는 로직을 추가할 수 있습니다 (예: 공격, 타겟팅 등)
            }
        }
    }

    // 부채꼴 시각화를 위해 기즈모를 사용하여 에디터에서 확인 가능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
