using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : Character, IDamageable{

    protected bool _isAttacking;
    [SerializeField]protected float _damageGrowFactor;
    [SerializeField]protected float _healthGrowFactor;
    [SerializeField]protected float _goldValueGrowFactor;
    private EnemySpawner _enemySpawner;
    private Image _healthBar;
    [SerializeField]private Sprite[] _healthBars;

    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }

    protected override void Awake()
    {
        _maxHealth = Mathf.RoundToInt(_maxHealth + Mathf.Pow(GameInformation.Wave, _healthGrowFactor)); //Scales max health with wave and growth factor
        _damage = _damage + Mathf.Pow(GameInformation.Wave, _damageGrowFactor); //Scales damage with wave and growth factor
        _enemySpawner = GameObject.FindGameObjectWithTag(Tags.ENEMYSPAWNER).GetComponent<EnemySpawner>();
        _healthBar = GetComponentInChildren<Image>();
        base.Awake();
    }

    void Update()
    {
        if(_rgb2d.velocity != Vector2.zero)
        {
            StartCoroutine(ResetVelocity());
        }
        if (_currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
        }
        UpdateHealthbar();
    }

    IEnumerator DeathRoutine()
    {
        _isDead = true;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        Destroy(collider);
        _animator.SetBool("IsDead", true);
        _enemySpawner.spawnedEnemies.Remove(this.gameObject);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    void UpdateHealthbar()
    {
        _healthBar.DOFillAmount(_currentHealth / _maxHealth, 0.3f);
        ChangeHealthColor();
    }

    void ChangeHealthColor()
    {
        float percentage = _currentHealth / _maxHealth * 100;
        if(percentage > 50)
        {
            _healthBar.sprite = _healthBars[0];
        }else if(percentage >= 25 && percentage <= 50)
        {
            _healthBar.sprite = _healthBars[1];
        }else
        {
            _healthBar.sprite = _healthBars[2];
        }
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.2f);
        _rgb2d.velocity = Vector2.zero;
    }

}
