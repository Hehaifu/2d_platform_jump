using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    public void DeductLife(int damage);

    public void RecoverSpeedInSecounds(float secounds);

    public int GetDamage();
}
