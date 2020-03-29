using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private int damage;

    public int GetDamage() => damage;
    public void SetDamage(int damage) => this.damage = damage;

    public void Hit()
    {
        Destroy(gameObject);
    }
}
