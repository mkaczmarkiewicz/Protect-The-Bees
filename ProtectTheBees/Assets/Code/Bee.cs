using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    Animator animator;

    const string ANIMATOR_STATUS = "status";
    const int HEALTHY = 0;
    const int ANGRY = 1;
    const int SICK = 2;

    void Start()
    {
        animator = GetComponent<Animator>();
    }   

    public void ChangeStatusToHealthy()
    {
        animator.SetInteger(ANIMATOR_STATUS, HEALTHY);
    }

    public void ChangeStatusToAngry()
    {
        animator.SetInteger(ANIMATOR_STATUS, ANGRY);
    }

    public void ChangeStatusToSick()
    {
        animator.SetInteger(ANIMATOR_STATUS, SICK);
    }   
}
