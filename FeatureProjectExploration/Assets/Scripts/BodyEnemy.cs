using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyEnemy : MonoBehaviour
{
    public PlayerMove pm;
    public JettAbilities ja;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KnifeProjectile")
        {
            ja.attackPoint1.SetActive(true);
            ja.attackPoint2.SetActive(true);
            ja.attackPoint3.SetActive(true);
            ja.attackPoint4.SetActive(true);
            ja.attackPoint5.SetActive(true);
            ja.MoreKnives();
        }
    }
}
