using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    protected Vector3 _velocity;
    public float MaxLife;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _damageTime;
    [SerializeField] protected float _maxSpeed = 5;
    [SerializeField] protected float _maxForce = 5;
    [SerializeField] protected float _timeToRecover;
    protected int _index;

    public float _viewRadiusAlignmentCohesion = 5;
    protected float _life;
    public float Life { get => _life; set { _life = value; } }
    public int Index { get => _index;}
    public float MaxSpeed { get => _maxSpeed; }
    [SerializeField] protected float _viewRadiusSeparation = 5;
    [SerializeField] protected LayerMask _obstacleMask = 1 << 6;

    public Team _team;
    public bool MoveQ = false;
    public DecisionNode DecisionT;
    protected Visual _visual;

    public Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = targetPos - transform.position;
        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }

    public Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, _maxSpeed);
    }

    public Vector3 Arrive(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist > _viewRadiusAlignmentCohesion) return Seek(targetPos);

        return Seek(targetPos, (_maxSpeed * (dist / _viewRadiusAlignmentCohesion)));
    }

    public Vector3 ObstacleAvoidance()
    {
        if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, _viewRadiusAlignmentCohesion, _obstacleMask))
        {
            Vector3 desired = -transform.right * _maxSpeed;

            return CalculateSteering(desired);
        }
        else if (Physics.Raycast(transform.position - transform.up * 0.5f, transform.forward, _viewRadiusAlignmentCohesion, _obstacleMask))
        {
            Vector3 desired = transform.right * _maxSpeed;

            return CalculateSteering(desired);
        }
        else return default;
    }

    #region Flocking

    public Vector3 Cohesion(List<SteeringAgent> agents)
    {
        //Promedio de posiciones de agentes de locales.
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.forward;
            if (dist.sqrMagnitude > _viewRadiusAlignmentCohesion)
                continue;

            desired += item.transform.position;
            count++;
        }
        if (count == 0) return Vector3.zero;

        //Promedio = Suma / Cant.
        desired /= count;
        return Seek(desired);
    }

    public Vector3 Separation(List<SteeringAgent> agents)
    {
        Vector3 desired = Vector3.zero;
        foreach (var item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > _viewRadiusSeparation * _viewRadiusSeparation) continue;
            desired += dist;
        }
        if (desired == Vector3.zero) return desired;
        desired *= -1;
        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    public Vector3 Alignment(List<SteeringAgent> agents)
    {
        //Promedio de posiciones de agentes de locales.
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > _viewRadiusAlignmentCohesion) continue;

            desired += item._velocity;
            count++;
        }
        if (count == 0) return Vector3.zero;
        //Promedio = Suma / Cant.
        desired /= count;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    #endregion

    public void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    public void Move()
    {
        //movimiento
        _velocity = new Vector3(_velocity.x, 0, _velocity.z);
        transform.position += _velocity * Time.deltaTime;
        
        //rotacion
        if (_velocity != Vector3.zero) transform.forward = _velocity;
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }

    public void RestartVelocity()
    {
        _velocity = Vector3.zero;
    }

    public void RestartPosition()
    {
        transform.position = Vector3.zero;
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, _viewRadiusAlignmentCohesion);
        Vector3 originA = transform.position + transform.up * 0.5f;
        Vector3 originB = transform.position - transform.up * 0.5f;

        Gizmos.DrawLine(originA, originA + transform.forward * _viewRadiusAlignmentCohesion);
        Gizmos.DrawLine(originB, originB + transform.forward * _viewRadiusAlignmentCohesion);

    }
}