using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public struct RayBundle_Ray{ // Whitspace Linting is Fun :)
    public RaycastHit hitInfo;
    public bool isHit;
}*/

public class Sensors : MonoBehaviour
{
    [HideInInspector]
    public enum Type
    {
        Line,
        RayBundle,
        SphereCast,
        BoxCast
    }

    [Header("General Variables")]
    public Type sensorType = Type.Line;
    public float sensorDistance = 1.0f;
    [SerializeField]
    public bool Hit { get; private set; }

    private RaycastHit castInfo = new RaycastHit();

    [Header("SphereCast Settings")]
    public float sphereRadius = 1.0f;

    [Header("BoxCast Settings")]
    public Vector2 boxExtents = new Vector2(1.0f, 1.0f);

    [Header("Raybundle Settings")]
    [Range(1, 100)]
    public int rayAmount = 1;

    [Range(0, 360)]
    public int fov;

    //private RayBundle_Ray[] raybundleHits;// To be Used in Updated Raybundle

    #region Unity Functions
    void Start()
    {

    }

    #endregion

    #region Main Functions
    public bool Scan(LayerMask _maskToCheck)
    {
        Hit = false;

        Vector3 dir = transform.forward;
        switch (sensorType)
        {
            case Type.Line:
                if (Physics.Linecast(transform.position, transform.position + dir * sensorDistance, out castInfo, _maskToCheck, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;

            case Type.RayBundle:
                float angleIncrement = (-(fov / 2) / (rayAmount / 2));

                for (int i = 1; i < rayAmount + 1; i++)
                {
                    Vector3 rayDirection = Quaternion.Euler(0f, (fov / 2) + (angleIncrement * i), 0f) * transform.forward;

                    if (Physics.Raycast(transform.position, rayDirection, out castInfo, sensorDistance, _maskToCheck, QueryTriggerInteraction.Ignore))
                    {
                        Hit = true;
                    }
                }
                if (Hit) return true;
                break;

            case Type.SphereCast:
                if (Physics.SphereCast(new Ray(transform.position, dir), sphereRadius, out castInfo, sensorDistance, _maskToCheck, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;

            case Type.BoxCast:
                if (Physics.CheckBox(transform.position, new Vector3(boxExtents.x, boxExtents.y, sensorDistance) / 2.0f, this.transform.rotation, _maskToCheck, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;

            default:
                break;
        }

        return false;
    }
    #endregion

    #region Debug Functions
    private void DebugHit() => Debug.Log("Hit Object");
    private void DebugRay() => Debug.DrawRay(transform.position, transform.forward * sensorDistance, Hit ? Color.red : Color.green);

    public void OnDrawGizmos()
    {
        Hit = Scan(LayerMask.GetMask("Player"));

        Gizmos.color = Color.green;
        if (Hit) Gizmos.color = Color.red;

        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        float length = sensorDistance;
        if (Hit)
        {
            length = Vector3.Distance(transform.position, castInfo.point);
        }

        switch (sensorType)
        {
            case Type.Line:
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.05f, 0.05f, 0.05f));
                break;

            case Type.RayBundle:
                if (rayAmount > 1)
                {
                    float angleIncrement = (-(fov / 2) / (rayAmount / 2));

                    for (int i = 1; i < rayAmount + 1; i++)
                    {
                        Vector3 rayDirection = Quaternion.Euler(0f, (fov / 2) + (angleIncrement * i), 0f) * Vector3.forward;

                        Gizmos.DrawRay(Vector3.zero, rayDirection * length);
                        Gizmos.DrawCube(rayDirection * length, new Vector3(0.05f, 0.05f, 0.05f));
                    }
                }
                break;

            case Type.SphereCast:
                Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
                if (Hit)
                {
                    Vector3 ballCenter = castInfo.point + castInfo.normal * sphereRadius;
                    length = Vector3.Distance(transform.position, ballCenter);
                }
                Gizmos.DrawWireSphere(Vector3.zero + Vector3.forward * length, sphereRadius);
                Gizmos.DrawLine(Vector3.up * sphereRadius, (Vector3.up * sphereRadius) + (Vector3.forward * length)); // Top Line To Sphere
                Gizmos.DrawLine(-Vector3.up * sphereRadius, (-Vector3.up * sphereRadius) + (Vector3.forward * length)); // Bottom Line To Sphere
                Gizmos.DrawLine(Vector3.right * sphereRadius, (Vector3.right * sphereRadius) + (Vector3.forward * length)); // Right Line To Sphere
                Gizmos.DrawLine(-Vector3.right * sphereRadius, (-Vector3.right * sphereRadius) + (Vector3.forward * length)); // Left Line To Sphere
                break;

            case Type.BoxCast:
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxExtents.x, boxExtents.y, length));
                break;

            default:
                break;
        }
    }
    #endregion
}
