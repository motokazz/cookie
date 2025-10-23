using UnityEngine;
using UnityEngine.AI; // NavMesh Agentを使うために必要

public class EnemyAI : MonoBehaviour
{
    // 公開変数
    [SerializeField] Animator animator;
    public Transform target; // 追跡するターゲット（プレイヤーなど）
    public float attackRange = 2f; // 攻撃できる距離
    public float attackInterval = 2f; // 攻撃の間隔
    [SerializeField] float attackBuffer = 0.5f;
    // 非公開変数
    private NavMeshAgent agent;
    private float timeSinceLastAttack = 0f;

    void Start()
    {
        // コンポーネントを取得
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(GameManager.Instance.enemyManager.currentEnemy != null)
        {
            target = GameManager.Instance.enemyManager.currentEnemy.transform;
        }

        // ターゲットが設定されていなければ何もしない
        if (target == null)
        {
            return;
        }

        // ターゲットとの距離を計算
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 目的地への移動処理
        if (distanceToTarget > attackRange)
        {
            // 攻撃範囲外なら追跡
            agent.SetDestination(target.position);
        }
        else
        {
            // 攻撃範囲内なら停止
            agent.SetDestination(transform.position);

            // 攻撃処理
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackInterval)
            {
                Attack();
                timeSinceLastAttack = 0f;

            }
        }

    }
    void AttackValue()
    {
        attackBuffer += GameManager.Instance.cookieManager.cookiesPerSecond * Time.deltaTime;

        if (attackBuffer >= 1f)
        {
            int add = Mathf.FloorToInt(attackBuffer);
            attackBuffer -= add;
            GameManager.Instance.enemyManager.TakeDamage(add); // ここで攻撃
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        int add = (int)GameManager.Instance.cookieManager.cookiesPerSecond;
        GameManager.Instance.enemyManager.TakeDamage(add);
        //Debug.Log("攻撃！");
        // 攻撃アニメーションの再生、ダメージ処理などをここに追加
    }
}
