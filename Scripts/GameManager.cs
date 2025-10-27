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

    private void Start()
    {
        dataManager.Init();
        enemyManager.SpawnNextEnemy();
    }

    // ゲーム終了時に保存
    void OnDestroy()
    {
        dataManager.Save();
    }

    public void Init()
    {
        cookieManager = cookieManagerX;
        upgradeManager = upgradeManagerX;
        upgradeUIManager = upgradeUIManagerX;
        enemyManager = enemyManagerX;
        dataManager = dataManagerX;

        cookieManager.Init();

        upgradeManager.Init();

        upgradeUIManager.RessetButtons();

        enemyManager.waveCount = 1;

    }





    public void Checker()
    {
        Debug.Log(enemyManager.currentEnemy);
        enemyManager.SpawnNextEnemy();
    }
}
