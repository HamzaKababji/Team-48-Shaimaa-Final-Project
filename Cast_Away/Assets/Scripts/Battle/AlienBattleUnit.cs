using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienBattleUnit : MonoBehaviour
{
    public BaseAlienScript alien {get; set;}

    public void Setup() {
        alien = GameManager.Instance.alienToFight;
        // alien = new DoctorAlienScript();
        GetComponent<Image>().sprite = alien.sprite;
    }
}
