//( ͡° ͜ʖ ͡°)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAttack : MonoBehaviour {

    Enemy _enemy;
    EnemyTargetting _enemyTargetting;
    private bool _readyToAttack = true;

	void Awake () {
        _enemy = GetComponent<Enemy>();
        _enemyTargetting = GetComponentInChildren<EnemyTargetting>();
	}
	
	void Update () {
        if (!_enemy.IsDead)
        {
            Attack();
        }
	}

    void Attack()
    {
        if(_enemyTargetting.Target != null)
        {
            float distance = Vector2.Distance(transform.position, _enemyTargetting.Target.transform.position);
            if (distance <= _enemy.AttackRange)
            {
                _enemy.IsAttacking = true;
                if (_readyToAttack)
                {
                    _readyToAttack = false;
                    StartCoroutine(AttackCooldown());  
                }
            }
            else
            {
                _enemy.IsAttacking = false;
            }
        }

    }

    IEnumerator AttackCooldown()
    {
        _enemy.CharacterAnimator.SetInteger("AnimationState", 1);
        yield return new WaitForSeconds(_enemy.AttackSpeed);
        _enemy.CharacterAnimator.SetInteger("AnimationState", 0);
        _readyToAttack = true;
    }
}
