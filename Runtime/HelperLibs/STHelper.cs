using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Physics and GameObject related helpers
/// </summary>
public static class STHelper
{
    public static List<Collider> GetCollidersInRangeByTag(Vector3 position, float radius, string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        colliders = colliders.Where(c => c.gameObject.CompareTag(tag)).ToArray();
        colliders = OrderCollidersByDistance(position, colliders);
        return colliders.ToList();
    }

    public static List<Collider> GetCollidersInRange(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        colliders = OrderCollidersByDistance(position, colliders);
        return colliders.ToList();
    }

    public static Collider[] OrderCollidersByDistance(Vector3 position, Collider[] colliders)
    {
        return colliders.OrderBy(col => Vector3.Distance(col.transform.position, position)).ToArray();
    }

    public static List<Collider2D> GetCollidersInRange2D(Vector3 position, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        colliders = OrderCollidersByDistance2D(position, colliders);
        return colliders.ToList();
    }

    public static Collider2D[] OrderCollidersByDistance2D(Vector3 position, Collider2D[] colliders)
    {
        return colliders.OrderBy(col => Vector3.Distance(col.transform.position, position)).ToArray();
    }

    public static Vector3 GetMouseWorldPosition(Camera cameraMain)
    {
        Vector3 vec = GetMouseWorldPositionWithZ(cameraMain);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetRelativeMousePosition2D(Camera cameraMain, Transform transform)
    {
        float distanceToTransform = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 cameraPoint = Input.mousePosition;
        cameraPoint.z = distanceToTransform;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);
        return worldPoint;
    }

    public static Vector3 GetDirectionVectorFromTo(Camera cameraMain, Transform transform, Vector3 Target)
    {
        var vec = (Target - transform.position).normalized;
        return vec;
    }

    public static GameObject GetObjectOnCursorPosition(Camera cameraMain) 
    {
        RaycastHit hit;
        var ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        else return null;
    }

    public static GameObject GetObjectOnCursorPosition2D(Camera cameraMain) 
    {
        RaycastHit2D hit = Physics2D.Raycast(cameraMain.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            return hit.transform.gameObject;
        }
        else return null;

    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera cam)
    {
        return GetWorldPositionWithZ(Input.mousePosition, cam);
    }

    public static Vector3 GetWorldPositionWithZ(Vector3 screenPosition, Camera camera)
    {
        return camera.ScreenToWorldPoint(screenPosition);
    }

    public static TextMesh createWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 355, Color color = default(Color), TextAnchor anchor = TextAnchor.MiddleCenter, TextAlignment alignment = TextAlignment.Center, int sortingOrder = 0)
    {
        GameObject gameObject = new GameObject("world_text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.anchor = anchor;
        textMesh.fontSize = fontSize;
        textMesh.characterSize = 0.01f;
        textMesh.color = color;
        textMesh.alignment = alignment;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }


    public static GameObject CreateCircleHighlighter(GameObject objWithLineRenderer, Transform targetParent, float radius, Color color1, Color color2, float alphaMultiplier = 1.0f, int edgeCount = 50)
    {
        GameObject obj = GameObject.Instantiate(objWithLineRenderer, targetParent.transform.position, Quaternion.identity, targetParent);
        LineRenderer renderer = obj.GetComponent<LineRenderer>();
        renderer.positionCount = edgeCount;
        renderer.useWorldSpace = false;
        renderer.loop = true;
        renderer.material.SetFloat("_AlphaMultiplier", alphaMultiplier);

        float x, y;
        float angle = 20f;
        for (int i = 0; i < (edgeCount); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            renderer.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / edgeCount);
        }

        var colorKey = new GradientColorKey[2];
        colorKey[0].color = color1;
        colorKey[0].time = 0.0f;
        colorKey[1].color = color2;
        colorKey[1].time = 1.0f;
        var alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;
        renderer.startColor = color1;
        renderer.endColor = color2;
        renderer.colorGradient.SetKeys(colorKey, alphaKey);

        return obj;
    }


    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static RaycastHit LayeredRayCast(Vector3 castPos, Vector3 castDir, out bool castHit, params string[] layerNames)
    {
        int layer_mask = LayerMask.GetMask(layerNames);
        RaycastHit hitInfo;
        if (Physics.Raycast(castPos, castDir, out hitInfo, 20, layer_mask))
        {
            castHit = true;
        }
        else
            castHit = false;
        return hitInfo;
    }

    public static bool LerpWithCurveOffset(GameObject gameObj,
        AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ,
        float curvePosition, float timeStep, float offsetAmplitude,
        Vector3 startPosition, Vector3 targetPosition, bool forceTargetPosition) 
    {

        //Example Usage: STHelper.LerpWithCurveOffset(gameObject, curveX, curveY, curveZ, curvePosition += timeStep * Time.deltaTime, timeStep * Time.deltaTime, 1f, startPosition, targetPosition, true);

        float curveMin = Mathf.Min(curveX.keys.FirstOrDefault().time, curveY.keys.FirstOrDefault().time, curveZ.keys.FirstOrDefault().time);
        float curveMax = Mathf.Max(curveX.keys.LastOrDefault().time, curveY.keys.LastOrDefault().time, curveZ.keys.LastOrDefault().time);

        if (curvePosition >= curveMax) 
        {
            if(forceTargetPosition)
                gameObj.transform.position = targetPosition;
            return true;
        }

        float clampedStep = Mathf.Clamp(curvePosition + timeStep, curveMin, curveMax);
        Vector3 evaluatedOffset = new Vector3(curveX.Evaluate(clampedStep),
                                          curveY.Evaluate(clampedStep),
                                          curveZ.Evaluate(clampedStep));

        gameObj.transform.position = Vector3.Lerp(startPosition, targetPosition, curvePosition) + (evaluatedOffset * offsetAmplitude);

        if (timeStep >= curveMax)
        {
            if (forceTargetPosition)
                gameObj.transform.position = targetPosition;
            return (true);
        }
        else
            return false;
         
    }

    public static bool RotateByCurve(GameObject gameObj,
    AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, float curvePosition, float timeStep)
    {
        //STHelper.RotateByCurve(gameObject, crvx, crvy, crvz, cpos, timeStep * Time.deltaTime);
        float curveMin = Mathf.Min(curveX.keys.FirstOrDefault().time, curveY.keys.FirstOrDefault().time, curveZ.keys.FirstOrDefault().time);
        float curveMax = Mathf.Max(curveX.keys.LastOrDefault().time, curveY.keys.LastOrDefault().time, curveZ.keys.LastOrDefault().time);

        if (curvePosition >= curveMax)
            return true;

        float clampedStep = Mathf.Clamp(curvePosition + timeStep, curveMin, curveMax);
        Vector3 evaluatedOffset = new Vector3(curveX.Evaluate(clampedStep),
                                          curveY.Evaluate(clampedStep),
                                          curveZ.Evaluate(clampedStep));

        gameObj.transform.Rotate(evaluatedOffset.normalized * curvePosition / (curveMax - curveMin));

        if (timeStep >= curveMax)
            return (true);
        else
            return false;

    }

    public static float DotIdentity(Vector3 a, Vector3 b) 
    {
        return a.magnitude * b.magnitude * Mathf.Cos(Vector3.Angle(a,b) * Mathf.Deg2Rad);
    }

    public static float DotLength(Vector3 a)
    {
        return Mathf.Sqrt(Vector3.Dot(a, a));
    }

    public static float DotAngle(Vector3 a, Vector3 b)
    {
        return Mathf.Acos(Vector3.Dot(a,b) / (a.magnitude*b.magnitude));
    }

    public static float DotSqr(Vector3 a)
    {
        return Vector3.Dot(a, a);
    }

    public static void RotateToCursor2D(Transform transform, Camera camera, float rotationSpeed)
    {
        Vector2 direction = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

    public static void RotateToTarget2D(Transform transform, Transform targetTransform, Camera camera, float rotationSpeed)
    {
        Vector2 direction = targetTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }



}
