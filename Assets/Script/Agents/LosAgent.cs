using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosAgent : MonoBehaviour
{
    [SerializeField] float _viewRadius = 5;
    [SerializeField] float _viewAngle = 90;
    [SerializeField] LayerMask _wallLayer;
    public LayerMask WallLayer
    { get { return _wallLayer; } }
    
    public bool InFieldOfView(Vector3 target)
    {
        Vector3 dist = target - transform.position;
        if (dist.magnitude > _viewRadius) return false;
        if (!InLineOfSight(target, dist, dist.magnitude)) return false;
        return Vector3.Angle(transform.forward, dist) <= _viewAngle / 2;
    }
    
    public bool InLineOfSight(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        return !Physics.Raycast(transform.position, dir, dir.magnitude, _wallLayer);
    }

    public bool InLineOfSight(Vector3 target, Vector3 dir, float dist)
    {
        return !Physics.Raycast(transform.position, dir, dist, _wallLayer);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + GetDirFromAngle(_viewAngle / 2).normalized * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + GetDirFromAngle(-_viewAngle / 2).normalized * _viewRadius);
        //Debug.DrawLine(transform.position, transform.position+Vector3.forward*_viewRadius,Color.red);
    }

    Vector3 GetDirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
