using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUI_LabelSolo : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI sumText;
    private TextMeshProUGUI maxText;
    private TextMeshProUGUI scoreLevel;
    private void Awake()
    {
        levelText = GameObject.Find("levelText").GetComponent<TextMeshProUGUI>();
        sumText = GameObject.Find("sumText").GetComponent<TextMeshProUGUI>();
        maxText = GameObject.Find("maxText").GetComponent<TextMeshProUGUI>();
        scoreLevel = GameObject.Find("scoreLevel").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        int level = PlayerPrefs.GetInt("level");
        levelText.text= "LEVEL "+level.ToString();
        sumText.text = PlayerPrefs.GetInt("sum").ToString();
        maxText.text = PlayerPrefs.GetInt("max").ToString();
        scoreLevel.text = PlayerPrefs.GetInt("level " + level).ToString();
    }
}
