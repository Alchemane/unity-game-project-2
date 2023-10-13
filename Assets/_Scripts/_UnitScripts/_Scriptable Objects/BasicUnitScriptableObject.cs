using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Units.BasicUnit.UnitScriptableObject
{
    public class BasicUnitScriptableObject : ScriptableObject
    {
        public string unitType;
        public string description;

        public int attackDamage;
        public int movementSpeed;

        public int health;
        public int armor;
        public int shield;
        public int barrier;
        public int forceField;


        public float fireRate;
        public float attackRange;

        public bool hasArmor;
        public bool hasShield;
        public bool hasBarrier;
        public bool hasForceField;
    }
}