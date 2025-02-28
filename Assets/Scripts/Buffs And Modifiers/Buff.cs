// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : ScriptableObject
{
    public enum buffCategory { ATTACK_BUFF, SPEED_BUFF, DEFENSE_BUFF, HP_BUFF, RECOVER_BUFF }
    public enum modificationType { Additive, Multiplicative } // Is the buff a flat addition to the stat, or a multiplier? Multipliers are applied last
    public modificationType modification;
    public buffCategory buffType;
    public float value;
    public float duration = 0; // A Duration of 0 means the buff never expires
    private Entity entity;
}
