
using System.Numerics;

[System.Serializable]
public class SaveData
{
    public double cookies = 0;
    public float cookiesPerSecond = 0f;
    public float cookiesPerClick = 1f;
    public int[] upgradeLevels;
    public int waveCount = 0;
    public double enemyCurrentHP = 0;
}
