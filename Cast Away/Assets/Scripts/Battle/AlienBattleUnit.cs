using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienBattleUnit : MonoBehaviour
{
    public BaseAlienScript alien {get; set;}

    public void Setup() {
        alien = new CitizenAlienScript();
        GetComponent<Image>().sprite = alien.sprite;
    }
}