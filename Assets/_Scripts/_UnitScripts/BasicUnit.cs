using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameRoot.InGame.Units.BasicUnit.UnitScriptableObject;
using GameRoot.InGame.Navigation.SelectionSystem.UnitManager;

namespace GameRoot.InGame.Units.BasicUnit
{
    public class BasicUnit : MonoBehaviour
    {
        public BasicUnitScriptableObject basicUnitScriptableObject;

        private string unitType;
        private string description;

        private int attackDamage;
        private int movementSpeed;

        private int health;
        private int armor;
        private int shield;
        private int barrier;
        private int forceField;

        private float fireRate;
        private float attackRange;

        private bool hasArmor;
        private bool hasShield;
        private bool hasBarrier;
        private bool hasForceField;

        private void Awake()
        {
            unitType = basicUnitScriptableObject.unitType;
            attackDamage = basicUnitScriptableObject.attackDamage;
            movementSpeed = basicUnitScriptableObject.movementSpeed;
            health = basicUnitScriptableObject.health;
            armor = basicUnitScriptableObject.armor;
            shield = basicUnitScriptableObject.shield;
            barrier = basicUnitScriptableObject.barrier;
            forceField = basicUnitScriptableObject.forceField;
            hasArmor = basicUnitScriptableObject.hasArmor;
            hasShield = basicUnitScriptableObject.hasShield;
            hasBarrier = basicUnitScriptableObject.hasBarrier;
            hasForceField = basicUnitScriptableObject.hasForceField;
        }

        // Start is called before the first frame update
        void Start()
        {
            UnitSelectionManager.Instance.unitList.Add(gameObject);
        }

        // getters
        public string GetUnitType()
        {
            return unitType;
        }
        public int GetAttackDamage()
        {
            return attackDamage;
        }
        public int GetMovementSpeed()
        {
            return movementSpeed;
        }
        public int GetHealth()
        {
            return health;
        }
        public int GetArmor()
        {
            return armor;
        }
        public int GetShield()
        {
            return shield;
        }
        public int GetBarrier()
        {
            return barrier;
        }
        public int GetForceField()
        {
            return forceField;
        }
        public float GetFireRate()
        {
            return fireRate;
        }
        public float GetAttackRange()
        {
            return attackRange;
        }
        public bool GetHasArmor()
        {
            return hasArmor;
        }
        public bool GetHasShield()
        {
            return hasShield;
        }
        public bool GetHasBarrier()
        {
            return hasBarrier;
        }
        public bool GetHasForceField()
        {
            return hasForceField;
        }
    }
}