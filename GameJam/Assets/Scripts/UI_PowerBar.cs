using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerBar : MonoBehaviour {


    public Image PowerChargeImage;
    public Text ChargeText;

    void Update()
    {
        ChargeText.text = Mathf.RoundToInt(GameController.powerCharge).ToString() + "%";
    }
}
