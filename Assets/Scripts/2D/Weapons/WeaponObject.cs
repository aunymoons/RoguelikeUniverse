﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponObject : NetworkedGameObject {

    //References
    public Weapon weaponReference;
    public Text nameLabel;

    //Components
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
        if(!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        if (!nameLabel) nameLabel = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateValues(Sprite targetSprite, string targetName, Weapon targetWeapon)
    {
        UpdateSprite(targetSprite);
        UpdateName(targetName);
        UpdateWeaponReference(targetWeapon);
    }

    public void UpdateSprite(Sprite targetSprite)
    {
        //Changes the sprite
        spriteRenderer.sprite = targetSprite;
    }

    public void UpdateName(string targetName)
    {
        //Change the name
        nameLabel.text = targetName;
    }
	
    public void UpdateWeaponReference(Weapon targetWeapon)
    {
    	weaponReference = targetWeapon;
    }
}
