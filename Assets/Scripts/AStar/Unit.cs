using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    private bool _isMoving = false;
    private Enemy _enemy;
    private Rigidbody2D _rgb2d;
    public Transform target;
    Vector3[] path;
    int targetIndex;
    private bool _isAttacking;
    private Quaternion _rotation;

    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _rgb2d = GetComponent<Rigidbody2D>();
        InvokeRepeating("FindPath", 0.01f, 0.25f);
    }

    void FindPath()
    {
        if (target != null && !_enemy.IsDead)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length >= 1)
        {
            _isMoving = true;
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                if (!_isAttacking)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Time.deltaTime * _enemy.MovementSpeed);
                    Vector3 direction = transform.position - currentWaypoint;
                    //_rgb2d.velocity = Vector2.zero;
                    RotateEnemy();
                }
                yield return null;
            }
        }
    }

    void RotateEnemy()
    {
        if (transform.position.x > target.position.x)
        {
            _rotation.y = 180;
        }
        else if (transform.position.x < target.position.x)
        {
            _rotation.y = 0;
        }
    }
}
