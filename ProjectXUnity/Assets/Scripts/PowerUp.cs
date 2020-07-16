﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Runner.Player;

public class PowerUp : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] int price;
    [SerializeField] PowerUpTypeEnum powerUpType;
    [Header("Invincibility")]
    [SerializeField] int invincibilityDuration;
    [Header("Meteorite")]
    [SerializeField] int diceFaces;
    [SerializeField] int difficultyToHit;

    bool available = false;

    public enum PowerUpTypeEnum { METEOR, INVINCIBILITY}

	public void OnPointerClick(PointerEventData eventData)
	{
        if (available)
		{
            switch (powerUpType)
            {
                case PowerUpTypeEnum.METEOR:
                    MeteorPower();
                    break;
                case PowerUpTypeEnum.INVINCIBILITY:
                    Invincibility();
                    break;
                default: break;
            }
        }
		
	}

	void Start()
    {
        TMP_Text txtPrice = gameObject.GetComponentInChildren<TMP_Text>();
        if (txtPrice != null)
		{
            txtPrice.text = price.ToString();
		}
    }

    void Update()
    {
        if (price <= ScoreManager.instance.GetScore())
		{
            gameObject.GetComponent<Image>().color = Color.white;
            available = true;
		}
		else
		{
            //le ponemos el sombreado o lo q sea
            gameObject.GetComponent<Image>().color = Color.black;
            available = false;
		}

    }

    void MeteorPower(){
        ScoreManager.instance.PayScore(price);
        int dinosNumber = FindObjectsOfType<AIDino>().Length;
        AIDino[] dinosArray = FindObjectsOfType<AIDino>();

        for(int i = 0; i<dinosNumber; i++) {
            bool used = false;
            if (dinosArray[i].IsAlive()) {
                used = true;
                dinosArray[i].Attacked(diceFaces, difficultyToHit);
                print(dinosArray[i].name + " attacked");
            }
            if (used) {
                return;
            }
        }
	}

    void Invincibility() {
        ScoreManager.instance.PayScore(price);
        StartCoroutine(FindObjectOfType<PlayerController>().Invincible(invincibilityDuration));
    }
}
