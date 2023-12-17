using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum StateEnum{
    Idle,
    Move
}

public class PetScript : MonoBehaviour
{
    // target to follow
    public Transform target;
    
    // AI fuction
    private NavMeshAgent _agent;

    // check the distance with the target
    public float _distance;
    
    // Variable for storing state
    private StateEnum _curState;

    //Component
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        
        _curState = StateEnum.Idle;
        _agent.enabled = true;
    }

    private void Update()
    {
        _distance = Vector3.Distance(transform.position, target.position);
        _anim.SetBool("isMove", _agent.enabled);
        switch (_curState)
        {
            case StateEnum.Idle:
                IdleState();
                break;
            case StateEnum.Move:
                MoveState();
                break;
        }
    }

    void IdleState()
    {
        if (_agent.enabled)
        {
            _agent.enabled = false;
        }
        if (_distance > 1 )
        {
            _curState = StateEnum.Move;
        }
    }

    void MoveState()
    {
        if (!_agent.enabled)
        {
            _agent.enabled = true;
        }

        if (_distance < 1)
        {
            _curState = StateEnum.Idle;
        }
        
        _agent.destination = target.position;
        
    }
}
