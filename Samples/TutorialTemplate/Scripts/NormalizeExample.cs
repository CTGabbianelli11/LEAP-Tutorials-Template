using UnityEngine;

public class NormalizeExample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 startPosition;
    public Vector3 endPosition;
    [Range(0,1)]
    public float normalizeValue;
    public AnimationCurve curve;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        normalizeValue = Mathf.Sin(Time.time);
        transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(normalizeValue));
    }
}
