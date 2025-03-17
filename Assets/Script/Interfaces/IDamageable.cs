using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void ModifyLife(float damage);
    public Vector3 MyPosition();
    public Visual MyVisual();
    public Team MyTeam();
    public float MyLife();
}
