using System;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private int Health = 10;

    private void OnCollisionEnter(Collision collision)
    {
            ICharacter iCharecter = 
                collision.gameObject.GetComponent<ICharacter>();
            
            Health -= iCharecter.damage;           //-2
            
            if (Health <= 0)
                Destroy(gameObject);
    }
}