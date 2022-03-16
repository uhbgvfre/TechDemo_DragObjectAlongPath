using UnityEngine;
using PathCreation;
using TouchScript.Gestures.TransformGestures;

public class DragOnPath : MonoBehaviour
{
    [Header("Config")]
    [Range(0, 1f)]
    public float smoothness;
    public float moveThreshold = .8f;

    [Header("Require")]
    public TransformGesture transformGesture;
    public PathCreator pathCreator;
    public Transform follower;

    private float deltaPosLength;

    private void FixedUpdate()
    {
        if (transformGesture.NumPointers > 0)
        {
            var hit = transformGesture.GetScreenPositionHitData();

            // 快取最近路徑上資訊
            var distOnPath = pathCreator.path.GetClosestDistanceAlongPath(hit.Point);
            var posOnPath = pathCreator.path.GetPointAtDistance(distOnPath);
            var rotOnPath = pathCreator.path.GetRotationAtDistance(distOnPath);

            // 檢驗步長閾值
            deltaPosLength = (follower.position - posOnPath).magnitude;
            if (deltaPosLength > moveThreshold) return;

            // 位移及導向
            var t = 1f - smoothness;
            posOnPath = Vector3.Lerp(follower.position, posOnPath, t);
            rotOnPath = Quaternion.Lerp(follower.rotation, rotOnPath, t);
            follower.SetPositionAndRotation(posOnPath, rotOnPath);
        }
    }
}