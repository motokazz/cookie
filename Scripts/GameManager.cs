using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CookieManager cookieManager { get; private set; }
    public UpgradeManager upgradeManager { get; private set; }
    public UpgradeUIManager upgradeUIManager { get; private set; }
    public EnemyManager enemyManager { get; private set; }
    public DataManager dataManager { get; private set; }

    [SerializeField] CookieManager cookieManagerX;
    [SerializeField] UpgradeManager upgradeManagerX;
    [SerializeField] UpgradeUIManager upgradeUIManagerX;
    [SerializeField] EnemyManager enemyManagerX;
    [SerializeField] DataManager dataManagerX;

    private void Awake()
    {
        // インスタンスがまだ存在しない場合、このインスタンスを保持する
        if (Instance == null)
        {
            Instance = this;
            // シーンを切り替えても破棄されないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既に存在する場合は破棄する
            Destroy(gameObject);
            return;
        }

        // 初期化
        Init();
    }

    private async void Start()
    {
        dataManager.Init();

        //upgradeManager.Init();

        dataManager.Load();

        upgradeManager.SpawnNPCs();
        await enemyManager.SpawnProcess();
    }

    // ===========================================
    // 初期化
    // ===========================================
    public void Init()
    {
        cookieManager = cookieManagerX;
        upgradeManager = upgradeManagerX;
        upgradeUIManager = upgradeUIManagerX;
        enemyManager = enemyManagerX;
        dataManager = dataManagerX;

        // CookieManager初期化
        cookieManager.Init();

        // UpgaradeManager初期化
        upgradeManager.Init();

        // UpgradeManager初期化
        upgradeUIManager.Init();

        // EnemyManager初期化
        enemyManager.Init();

    }

    // ===========================================
    // リセット
    // ===========================================
    public async void Reset()
    {
        // UpgaradeManager初期化
        upgradeManager.Reset();
        upgradeManager.Init();
        upgradeManager.SpawnNPCs();

        // CookieManager初期化
        cookieManager.Init();

        // UpgradeManager初期化
        upgradeUIManager.Init();

        // EnemyManager初期化
        enemyManager.Init();
        await enemyManager.SpawnProcess();

        dataManager.Init();
        dataManager.Save();
    }

    // ===========================================
    // ゲーム終了時に保存
    // ===========================================
    void OnDestroy()
    {
        dataManager.Save();
    }





    public async void Checker()
    {
        Debug.Log(enemyManager.currentEnemy);
        await enemyManager.SpawnProcess();
    }
}
