using TMPro;
using UnityEngine;

public class PlayerInfoItem : MonoBehaviour
{
    public TMP_Text InfoText;
    public string Name { get; set; }

    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (InfoText != null)
                InfoText.text = $"{Name} : {score}";
        }
    }
}
