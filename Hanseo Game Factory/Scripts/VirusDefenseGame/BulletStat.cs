using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VO: Value Object, 자바빈즈와 동일
public class BulletStat
{
    public float speed { get; set; }
    public int damage { get; set; }

    public BulletStat(float speed, int damage) //생성자
    {
        this.speed = speed;
        this.damage = damage;
    }
   
}
