using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public void MoveToDirection(Rigidbody2D _rigidBody, Vector2 _direction, float _speed)
    {
        _rigidBody.velocity = _direction.normalized * _speed * Time.fixedDeltaTime;
    }
}
