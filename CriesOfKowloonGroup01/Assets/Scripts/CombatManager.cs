using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public bool CanReceiveInput;
    public bool InputGet;

    private void Awake()
    {
        instance = this;
    }

    void attack()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
        {
            if (CanReceiveInput)
            {
                InputGet = true;
                CanReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    void InputManager()
    {
        if(!CanReceiveInput)
        {
            CanReceiveInput = true;
        }
        else
        {
            CanReceiveInput = false;
        }
    }
}
