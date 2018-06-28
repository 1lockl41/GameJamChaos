using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Timer : MonoBehaviour {

    public float Timer;
    public float Min;
    public float Sec;
    public Text Timer_Text;
	// Use this for initialization
	void Start ()
    {

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
