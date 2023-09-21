using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaStatus : MonoBehaviour {


    public 

    void Start() {
        
    }
    
    void Init() {
        
    }
    void Update()
    {
        
    }

    float CharaCounterCheck(CharaAttackType getAttackType) {
        switch(getAttackType) {
            case CharaAttackType.Mystery:
            break;
            case CharaAttackType.Explosion:
            break;
            case CharaAttackType.Penetrate:
            break;
        }
        float getDamage = 1.0f;
        return getDamage;
    }

    public void CharaHit(int enemyAtk, CharaAttackType enemyAttackType) {
        float getDamage = enemyAtk * CharaCounterCheck(enemyAttackType);
    }
}
