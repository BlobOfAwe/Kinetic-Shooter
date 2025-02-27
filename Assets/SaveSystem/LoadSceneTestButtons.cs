using TMPro;
using UnityEngine;

public class LoadSceneTestButtons : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI numberText;

    private int number = 0;

    public void ChangeValue(int value)
    {
        number += value;
        numberText.text = number.ToString();
    }

    public void Save()
    {
        SaveSystem.SaveData(number);
    }

    public void Load()
    {
        if (SaveSystem.LoadData() != null)
        {
            number = SaveSystem.LoadData().testInt;
            numberText.text = number.ToString();
        }
    }

    public void Delete()
    {
        SaveSystem.DeleteData();
    }
}
