using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : CharacterInfo
{
    public UserInfo(int id) : base(id)
    {
    }

    public bool IsDead()
    {
        return false;
    }
}
