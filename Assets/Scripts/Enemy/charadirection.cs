/*
using UnityEngine;

public class SetRotation : MonoBehaviour
{
    public Transform parentObject;  // 親オブジェクト

    void Update()
    {
        float parentYRotation = parentObject.eulerAngles.y;

        // 親オブジェクトのY軸が180度の場合
        if (Mathf.Approximately(parentYRotation, 180f))
        {
            // 子オブジェクトのX軸を20、Y軸を180、Z軸を0にセット
            transform.rotation = Quaternion.Euler(20f, 0f, 0f);
            Debug.Log("Parent Y axis is 180. Child rotation set to (20, 180, 0).");
        }
        // 親オブジェクトのY軸が-90度の場合（Unityでは270度として扱われる）
        else if (Mathf.Approximately(parentYRotation, 270f))
        {
            // 子オブジェクトのX軸を20、Y軸を-270、Z軸を0にセット
            transform.rotation = Quaternion.Euler(20f, -360f, 0f);
            Debug.Log("Parent Y axis is -90 (270 in Unity). Child rotation set to (20, -270, 0).");
        }
        // 親オブジェクトのY軸が90度の場合
        else if (Mathf.Approximately(parentYRotation, 90f))
        {
            // 子オブジェクトのX軸を-20、Y軸を90、Z軸を0にセット
            transform.rotation = Quaternion.Euler(-20f, 180f, 0f);
            Debug.Log("Parent Y axis is 90. Child rotation set to (-20, 90, 0).");
        }
        // 親オブジェクトのY軸が0度の場合
        else if (Mathf.Approximately(parentYRotation, 0f))
        {
            // 子オブジェクトのX軸を20、Y軸を0、Z軸を0にセット
            transform.rotation = Quaternion.Euler(-20f, 180f, 0f);
            Debug.Log("Parent Y axis is 0. Child rotation set to (20, 0, 0).");
        }
        else
        {
            //Debug.Log("Parent Y axis is at an unsupported angle: " + parentYRotation);
        }
    }
}
*/
using UnityEngine;

public class FlipSpriteBasedOnParentDirection : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform parentTransform;
    private Quaternion initialRotation; // 初期回転を保持

    void Start()
    {
        // 子オブジェクトのSpriteRendererを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 親オブジェクトのTransformを取得
        parentTransform = transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError("Parent object is not assigned.");
            return;
        }

        // 子オブジェクトの初期回転を保存
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // 初期の回転を維持
        transform.rotation = initialRotation;

        // 親オブジェクトのローカル回転角度を取得
        float parentYRotation = parentTransform.localEulerAngles.y;

        // 親オブジェクトの向きによってスプライトを反転
        if (IsFacingRight(parentYRotation) || IsFacingDown(parentYRotation))
        {
            spriteRenderer.flipX = true; // 右向きまたは下向き：反転
        }
        else if (IsFacingUp(parentYRotation) || IsFacingLeft(parentYRotation))
        {
            spriteRenderer.flipX = false; // 上向きまたは左向き：反転なし
        }
    }

    // 右向き判定 (90度 ± 10度)
    private bool IsFacingRight(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 90f)) <= 10f;
    }

    // 左向き判定 (-90度 ± 10度)
    private bool IsFacingLeft(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, -90f)) <= 10f;
    }

    // 上向き判定 (0度 ± 10度)
    private bool IsFacingUp(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 0f)) <= 10f;
    }

    // 下向き判定 (180度 ± 10度)
    private bool IsFacingDown(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 180f)) <= 10f;
    }
}
