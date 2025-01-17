﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuffs : MonoBehaviour
{
    public GameObject shield;
    [HideInInspector]
    public List<Buff> Active = new List<Buff>();
    public bool Contains(string item)
    {
        foreach (Buff buff in Active)
        {
            if (buff.GetName() == item)
                return true;
        }

        return false;
    }

    private void OnEnable()
    {
        ClearBuffs();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Pickup")
        {
            Pickup pickup = c.GetComponent<Pickup>();
            AddBuff(pickup.Name, pickup.Time);
        }
    }

    public void AddBuff(string buff, int time)
    {
        Active.Add(new Buff(buff, time));

        if (buff == "Shield" && !transform.Find("Shield(Clone)"))
            Instantiate(shield, transform);
    }

    void ClearBuffs()
    {
        Active.Clear();

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Shield") || child.name.Contains("Laser"))
                Destroy(child.gameObject);
        } 
    }

    private void Update()
    {
        foreach (Buff buff in Active.ToList())
        {
            if (!Contains("Laser") || buff.GetName() == "Laser")
            {
                buff.Tick(Time.deltaTime);

                if (buff.IsFinished)
                {
                    Active.Remove(buff);
                    RemoveBuff(buff.GetName());
                }
            }
        }      
    }

    void RemoveBuff(string buff)
    {
        if (buff == "Laser" || buff == "Shield")
        {
            // Check either a laser or shield exists, and there isn't another instance of this buff active
            if (transform.Find(buff + "(Clone)") != null && !Contains(buff)) 
                Destroy(transform.Find(buff + "(Clone)").gameObject);
        }
    }
}

public class Buff
{
    public string GetName() { return name; }
    public bool IsFinished { get { return timeRemaining <= 0; } }

    private string name;
    private float duration;
    private float timeRemaining;

    private BuffRadialSlider slider;

    public Buff(string _name, float _duration)
    {
        slider = UIControl.instance.PickupTimer();

        name = slider.buff = _name;
        duration = timeRemaining = _duration;
    }

    public void Tick(float Delta)
    {
        timeRemaining -= Delta;
        slider.UpdateAngle(timeRemaining / duration);
    }
}

