// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    /// <summary>
    /// The Difficulty Coefficient tracks the game's difficulty across levels.
    /// References to the coefficient should look as follows:
    ///     _value = _baseValue * ( 1 + (difficultyCoefficient * _modifier))
    /// As an example:
    ///     attackStat = 10 * ( 1 + (difficultyCoefficient * 0.1))
    ///     This increases the attack stat by 10% of its base value for every increment of 1 of the coefficient
    /// The difficulty Coefficient increases by 1 every level.
    /// </summary>
    public static float difficultyCoefficient = 0;
    public static float score = 0;
    public static int currentStage = 0;

    /// <summary>
    /// This is a list of the scene index that should be loaded for each level.
    /// Each scene contains one environment. See the Unity Build Settings for which index belongs
    /// to which environment. The indexes can be changed, or more can be added to increase the game length.
    /// This is retrieved by Beacon.cs to load levels after completion.
    /// </summary>
    public static int[] sceneIndexForLevel = {1,1};
    
    /// <summary>
    /// Which level the player is on. The first level is level 0. This is compared to the index of sceneIndexForLevel
    /// to see which level should be loaded.
    /// This is retrieved by Beacon.cs.
    /// </summary>
    public static int currentLevel = 0;

    public static bool debugEnabled;
    public static LoadoutManager.Loadout playerLoadout;
    public static bool paused;
}
