﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public GameObject PickupEffect;

    [HideInInspector]
    public string Name;
    
    public delegate void GotDelegate (string name, int time);
    public static event GotDelegate Got;

    public int Time = 3;
 
    private void Awake()
    {
        Name = name.Replace("(Clone)", "");

        ShipBuffs shipBuffs = FindObjectOfType<ShipBuffs>();
        if (shipBuffs.Contains(Name))
            Destroy(gameObject);

        int level = Upgrades.Get(Name);
        
        Time += (level * 2);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" && c.gameObject.name != ("Shield(Clone)"))
        {
            Destroy(gameObject);

            Instantiate(PickupEffect, transform.position, transform.rotation);

            // Error check delegate subscriptions
            if (Got != null)
                Got(Name, Time);
        }
    }
}