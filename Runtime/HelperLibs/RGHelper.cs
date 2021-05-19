using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Random Number and numerical Helpers
/// </summary>
public static class RGHelper
{

    public static Vector3 MidPoint(Vector3 v1, Vector3 v2) 
    {
        return Vector3.Lerp(v1, v2, 0.5f);
    }

    public static Vector3 RandomPositionInSphere(Vector3 position, float radius, float min) 
    {
        Vector3 pos = new Vector3();
        pos = position + (Random.insideUnitSphere * radius) + (Random.onUnitSphere * radius) * min;

        return pos;
    }

    public static Vector3 RandomPositionOnSphere(Vector3 position, float radius)
    {
        Vector3 pos = new Vector3();
        pos = position + (Random.onUnitSphere * radius) + (Random.onUnitSphere * radius);

        return pos;
    }

    public static Vector3 RandomPositionOnHemiSphere(Quaternion targetDirection, float angle, float radius, Vector3 offset)
    {
        var angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        var PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        var V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return (targetDirection * V * radius) + offset;

    }

    public static Vector3 RandomPositionAroundHemiCircle(Vector3 Center, Vector3 targetLookPosition, float angle, float radius, float z = 0)
    {
        Quaternion look = Quaternion.LookRotation(Center - targetLookPosition);
        var rp = RGHelper.RandomPositionOnHemiSphere(look, angle, radius, targetLookPosition);
        rp.z = z;
        return (rp - Center).normalized * radius;
    }

    public static Vector3 RandomPositionAroundHemiCircle(Quaternion lookRotation, Vector3 Center, Vector3 targetLookPosition, float angle, float radius, float z = 0)
    {
        var rp = RGHelper.RandomPositionOnHemiSphere(lookRotation, angle, radius, targetLookPosition);
        rp.z = z;
        return (rp - Center).normalized * radius;
    }





}
