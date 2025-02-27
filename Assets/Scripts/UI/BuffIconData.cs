//Z.S
//This script stores and manages a collection of icons used for the buffs 
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffIconData", menuName = "ScriptableObjects/BuffIconData", order = 1)]
public class BuffIconData : ScriptableObject
{
    public List<BuffIcon> buffIcons;

    [System.Serializable]
    public class BuffIcon
    {
        public Buff.buffCategory buffType;
        public GenericBuffDebuff.buffType buffDebuff;
        public List<Sprite> icons; //Creates a list of sprites instead of individual ones to show the progression via the Icon itself
    }

    public Sprite GetIcon(Buff.buffCategory buffType, GenericBuffDebuff.buffType buffDebuff, int index = 0)
    {
        foreach (var buffIcon in buffIcons)
        {
            if (buffIcon.buffType == buffType && buffIcon.buffDebuff == buffDebuff)
            {
                return buffIcon.icons[index];
            }
        }
        return null;
    }
}