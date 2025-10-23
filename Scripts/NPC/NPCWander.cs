using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    [SerializeField] float wanderRadius = 10f; // 移動範囲
    [SerializeField] float waitTime = 2f; // 次の目的地へ移動するまでの待機時間
    [SerializeField] float attackBuffer = 0.5f;

    [SerializeField] private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewDestination();
    }

    void Update()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                SetNewDestination();
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            agent.SetDestination(other.transform.position);
            attackBuffer += GameManager.Instance.cookieManager.cookiesPerSecond * Time.deltaTime;

            if (attackBuffer >= 1f)
            {
                int add = Mathf.FloorToInt(attackBuffer);
                attackBuffer -= add;
                GameManager.Instance.enemyManager.TakeDamage(add); // ここで攻撃
            }
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}