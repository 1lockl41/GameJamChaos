using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerBar : MonoBehaviour {

    public float MaxCharge = 100;
    public float CurrCharge;
    public Image PowerChargeImage;
    public Text ChargeText;

	// Use this for initialization
	void Start ()
    {
        CurrCharge = 0;
        InvokeRepeating("IncreasePower", 0f, 1f);
	}
	
	void IncreasePower()
    {
        CurrCharge += 1f;
        float Calc_Charge = CurrCharge / MaxCharge;
        SetCharge(Calc_Charge);
    }

    void SetCharge(float Charge)
    {
        PowerChargeImage.fillAmount = Charge;
        
    }

   void Update()
    {
        ChargeText.text = CurrCharge + "%";
    }
}
