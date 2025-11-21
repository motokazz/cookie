using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// エネミーの表示データ
/// </summary>
/// 

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public double currentHP;

    public TMP_Text hpText;
    public TMP_Text nameText;
    [SerializeField] Gauge hpGauge;
    [SerializeField] Gauge timeGauge;

    public float timeLimit = 1.0f;
    public float runInterval = 1.0f;

    public int waveCount=1;


    //
    private CancellationTokenSource cts = new CancellationTokenSource();



    private void Awake()
    {
        // Enemyコンポーネント取得
        var currentEnemy = GetComponent<Enemy>();

        if (currentEnemy == null)
        {
            currentEnemy = gameObject.AddComponent<Enemy>();
        }

        //タグ設定
        if (gameObject.tag != "Enemy")
        {
            gameObject.tag = "Enemy";
        }
    }


    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        cts?.Cancel();
    }

    private void Update()
    {
        if (hpGauge != null)
        {
            hpGauge.gaugeValue = (float)currentHP/(float)(data.maxHP*waveCount);
        }
        if (timeGauge != null)
        {
            timeGauge.gaugeValue = timeLimit / runInterval;
        }
    }

    // ===========================================
    // Enemyの初期化 渡したエネミーデータで上書き
    // ===========================================
    public void Initialize()
    {
        if (!cts.Token.IsCancellationRequested)
        {
            cts.Cancel();
        }
        currentHP = data.maxHP * waveCount;

        //UI
        if (hpText != null) hpText.text = $"HP: {currentHP}";
        if (nameText != null) nameText.text = data.enemyName;

        //Run
        RunProcess(runInterval).Forget();
    }


    // ===========================================
    // スポーン処理
    // ===========================================

    // 逃走処理
    public async UniTask RunProcess(float runInterval)
    {
        cts = new CancellationTokenSource();
        await ShowWaitTime(runInterval,cts.Token);
        if (!cts.Token.IsCancellationRequested)
        {
            Run();
        }
    }
    // ウェイト処理
    private async UniTask ShowWaitTime(float seconds,CancellationToken token)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            GameManager.Instance.enemyManager.timeLimit = remaining;
            await UniTask.Yield(token); // 次のフレームまで待つ
            remaining -= Time.deltaTime;
            timeLimit = remaining;
        }
    }

    // ===========================================
    // エネミー挙動
    // ===========================================
    // ダメージ処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP > 0)
        {
            if (hpText != null) hpText.text = $"HP: {currentHP}";
        }
        else
        {
            Die();
        }
    }

    // 逃走
    public void Run()
    {
        cts?.Cancel();

        GameManager.Instance.enemyManager.currentEnemyCount --;

        var pooledObject = GetComponent<PooledObjectAddressable>();
        if (pooledObject != null)
        {
            pooledObject.Release();
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("run");
        
    }

    // 死亡
    public void Die()
    {
        cts?.Cancel();

        //勝利ボーナス
        GameManager.Instance.cookieManager.cookies += data.rewardCookies;
        GameManager.Instance.enemyManager.waveCount++;
        GameManager.Instance.enemyManager.currentEnemyCount --;

        //Spawn
        var pooledObject = GetComponent<PooledObjectAddressable>();
        if (pooledObject != null)
        {
            pooledObject.Release();
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("dead");
    }
}
