using TMPro;
using UnityEngine;

public class LoadSceneTestButtons : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI numberText;

    private GameData gameData;

    private void Awake()
    {
        gameData = DataManager.Instance.gameData;
    }

    public void ChangeValue(int value)
    {
        gameData.totalKills += value;
        numberText.text = gameData.totalKills.ToString();
    }

    public void Save()
    {
        DataManager.Instance.SaveGame();
    }

    public void Load()
    {
        if (SaveSystem.LoadData() != null)
        {
            DataManager.Instance.LoadGame();
            gameData = DataManager.Instance.gameData;
            numberText.text = gameData.totalKills.ToString();
        }
    }

    public void Delete()
    {
        DataManager.Instance.DeleteGame();
        gameData = DataManager.Instance.gameData;
        numberText.text = gameData.totalKills.ToString();
    }
}
