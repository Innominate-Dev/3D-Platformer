using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI hiScoreText;
    public TextMeshProUGUI scoreText;

    public float scoreCount;
    public float hiScoreCount;

    public float pointsPerSecond;

    public bool ScoreIncreasing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreCount += pointsPerSecond * Time.deltaTime;

        if(scoreCount > hiScoreCount)
        {
            hiScoreCount = scoreCount;
        }

        scoreText.text = "Time: " + Mathf.Round(scoreCount);
        hiScoreText.text = "Best Time: " + Mathf.Round(hiScoreCount);
    }
}
