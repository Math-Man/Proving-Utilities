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
        Quaternion look = Quaternion.LookRotation(targetLookPosition - Center);
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

    public static Vector3[] UniformPointsOnSphere(int numberOfPoints)
    {
        List<Vector3> upts = new List<Vector3>();
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0f / numberOfPoints;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        float phi = 0;
       
        for (var k = 0; k < numberOfPoints; k++){
            y = k * off - 1 + (off /2);
            r = Mathf.Sqrt(1 - y * y);
            phi = k * inc;
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;
           
            upts.Add(new Vector3(x, y, z));
        }
        Vector3[] pts = upts.ToArray();
        return pts;
    }
    
    public static List<RaycastHit> CircularRaycastOnXY(Vector3 origin, int rayCount, float maxDistance, int layerMask)
    {
        List<RaycastHit> contacts = new List<RaycastHit>();
            
        float angle = 0;
        for (int i=0; i<rayCount; i++) 
        {
            float x = Mathf.Sin (angle);
            float y = Mathf.Cos (angle);
            float z = origin.z;
            angle += 2 * Mathf.PI / rayCount;
 
            Vector3 dir = new Vector3 (origin.x + x, origin.y + y, z);
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, maxDistance, layerMask))
            {
                contacts.Add(hit);
            }
        }

        return contacts;
    }



}
