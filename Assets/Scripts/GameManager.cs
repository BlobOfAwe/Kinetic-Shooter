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

    public static bool debugEnabled;
    public static LoadoutManager.Loadout playerLoadout;
    public static bool paused;
}
