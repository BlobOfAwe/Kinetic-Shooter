using UnityEngine;

[System.Serializable]
public class GameData
{
    public int totalKills;
    [Space]
    [Header("Base Enemy Kills")]
    public int normalKills;
    public int armourKills;
    public int speedKills;
    public int sniperKills;
    public int bladeKills;
    public int stickyKills;
    public int spawnerKills;
    public int trapletKills;
    public int minletKills;
    public int lightningKills;
    public int kamikazeKills;
    [Header("Leader Enemy Kills")]
    public int splitterKills;
    public int callerKills;
    public int armadilloKills;
    [Header("Boss Enemy Kills")]
    public int bouncerKills;
    public int necromancerKills;
    [Space]
    [Header("Loadout Kills")]
    public int standardLoadoutKills;
    public int heavyLoadoutKills;
    public int lightLoadoutKills;

    public GameData()
    {
        totalKills = 0;


        normalKills = 0;
        armourKills = 0;
        speedKills = 0;
        sniperKills = 0;
        bladeKills = 0;
        stickyKills = 0;
        spawnerKills = 0;
        trapletKills = 0;
        minletKills = 0;
        lightningKills = 0;
        kamikazeKills = 0;

        splitterKills = 0;
        callerKills = 0;
        armadilloKills = 0;

        bouncerKills = 0;
        necromancerKills = 0;


        standardLoadoutKills = 0;
        heavyLoadoutKills = 0;
        lightLoadoutKills = 0;
    }

    public void AddKills(string enemyName)
    {
        totalKills += 1;

        switch (enemyName)
        {
            case "Normal":
                normalKills += 1;
                break;
            case "Armoured":
                armourKills += 1;
                break;
            case "Speed Demon":
                speedKills += 1;
                break;
            case "Sniper":
                sniperKills += 1;
                break;
            case "Blade Swinger":
                bladeKills += 1;
                break;
            case "Sticky Sniper":
                stickyKills += 1;
                break;
            case "Spawner":
                spawnerKills += 1;
                break;
            case "Traplet":
                trapletKills += 1;
                break;
            case "Minlet":
                minletKills += 1;
                break;
            case "Lightning Twins":
                lightningKills += 1;
                break;
            case "Kamikaze":
                kamikazeKills += 1;
                break;
            case "Splitter":
                splitterKills += 1;
                break;
            case "War Caller":
                callerKills += 1;
                break;
            case "Armadillo":
                armadilloKills += 1;
                break;
            case "Bouncer":
                bouncerKills += 1;
                break;
            case "Necromancer":
                necromancerKills += 1;
                break;
        }

        switch (GameManager.playerLoadout.loadout)
        {
            case "Standard":
                standardLoadoutKills += 1;
                break;
            case "Heavy":
                heavyLoadoutKills += 1;
                break;
            case "Light":
                lightLoadoutKills += 1;
                break;
        }
    }
}
