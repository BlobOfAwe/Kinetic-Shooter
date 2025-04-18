//ZS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using Unity.VisualScripting;
using System;

public class LoadoutManager : MonoBehaviour
{
    public Image currentLoadoutImage;
    public TextMeshProUGUI loadoutInfoText;
    public TextMeshProUGUI loadoutTitleText;
    private int currentIndex = 0;
    public Loadout[] loadouts;
    [SerializeField] private GameObject playerPrefab;
    [System.Serializable]
    //class stores the information for each loadout
    public class Loadout
    {
        public string loadout;
        public string weapon;
        public string primary;
        public string secondary;
        public string ability;
        public string description;
        public Sprite loadoutImage;
        public Ability primaryAbility;
        public Ability secondaryAbility;
        public Ability utilityAbility;

        [Header("Stats")]
        public float attackStat;
        public float defenseStat;
        public float speedStat;
        public float hpStat;
        public float recoverStat;

    }
    private void Start()
    {
        UpdateLoadoutUI();
    }
    public void NextLoadout()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSelect, this.transform.position);
        currentIndex++;
        if (currentIndex >= loadouts.Length) currentIndex = 0;
        UpdateLoadoutUI();
        
    }

    public void PreviousLoadout()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSelect, this.transform.position);
        currentIndex--;
        if (currentIndex < 0) currentIndex = loadouts.Length - 1;
        UpdateLoadoutUI();
      
    }
    private void UpdateLoadoutUI()
    {

        Debug.Log($"Current Loadout Index: {currentIndex}");
        Loadout currentLoadout = loadouts[currentIndex];
        currentLoadoutImage.sprite = currentLoadout.loadoutImage;
        loadoutTitleText.text = $"<b>{currentLoadout.loadout}</b>";
        loadoutInfoText.text = $"<b>Weapon:</b> {currentLoadout.weapon}\n" +
                               $"<b>Primary:</b> {currentLoadout.primary}\n" +
                               $"<b>Secondary:</b> {currentLoadout.secondary}\n" +
                               $"<b>Ability:</b> {currentLoadout.ability}\n" +
                               $"<b>Description:</b> {currentLoadout.description}\n";
        PlayerPrefs.SetInt("SelectedLoadoutIndex", currentIndex);
    }

    public void AssignLoadoutToPlayer()
    {
        GameManager.playerLoadout = loadouts[currentIndex];
    }
}