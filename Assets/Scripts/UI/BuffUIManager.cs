//Z.S
//This script handles the creation and removal of the buffs using buffdata.
using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    public GameObject buffUIPrefab;
    public Transform buffUIContainer;
    private Dictionary<Buff, GameObject> activeBuffs = new Dictionary<Buff, GameObject>();

    public void AddBuff(Buff buff, GenericBuffDebuff.buffType buffDebuff, Sprite icon)
    {
        GameObject buffUI = Instantiate(buffUIPrefab, buffUIContainer);
        buffUI.GetComponent<BuffUI>().Initialize(buff, buffDebuff);
        activeBuffs.Add(buff, buffUI);
    }
    public void RemoveBuff(Buff buff)
    {
        if (activeBuffs.ContainsKey(buff))
        {
            Destroy(activeBuffs[buff]);
            activeBuffs.Remove(buff);
        }
    }
}