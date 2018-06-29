using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Timer : MonoBehaviour {

    public float Timer;
    public float Min;
    public float Sec;
    public Text Timer_Text;

    public Text Player1txt;
    public Text Player2txt;
    public Text Player3txt;
    public Text Player4txt;

    public Platformer2DUserControl Player1;
    public Platformer2DUserControl Player2;
    public Platformer2DUserControl Player3;
    public Platformer2DUserControl Player4;

    public GameObject endScreen;

    // Use this for initialization
    void Start ()
    {
        endScreen.SetActive(false);

        Timer = 180f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        Timer -= Time.deltaTime;
        UpdateTimer(Timer);
        //Min = Mathf.Floor(Timer / 60f);
        //Sec = Mathf.RoundToInt(Timer % 60f);
    
    Timer_Text.text = Min + ":" + Sec;

        if(Timer <= 0)
        {
            Timer = 0;
            Sec = 0;
            Min = 0;
            endScreen.SetActive(true);

            Player1txt.text = Player1.score.ToString();
            Player2txt.text = Player2.score.ToString();
            Player3txt.text = Player3.score.ToString();
            Player4txt.text = Player4.score.ToString();

            //run the end game stuff here
            Debug.Log("EndGame");
        }
	}

    void UpdateTimer(float TotalSeconds)
    {
        Min = Mathf.FloorToInt(TotalSeconds / 60f);
        Sec = Mathf.RoundToInt(TotalSeconds % 60f);

        string FormattedSeconds = Sec.ToString();

        if(Sec == 60)
        {
            Sec = 0;
            Min += 1;
        }

        Timer_Text.text = Min.ToString("00") + ":" + Sec.ToString("00");
    }
}
