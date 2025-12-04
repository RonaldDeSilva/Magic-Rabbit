using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
     public class Stats
     {

        public int Basedamage;
        public int health;
        public int cardshuffletime;
        public int luck;
        public int startingcardamount;
        public Stats(int Basedamage, int health, int cardshuffletime, int luck, int startingcardamount)
        {
            this.Basedamage = Basedamage;
            this.health = health;
            this.cardshuffletime = cardshuffletime;
            this.luck = luck;
            this.startingcardamount = startingcardamount;

        }
        void Start()
        {

        }
    }

   
}
