using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskのためのusing
using DG.Tweening; // DoTweenのためのusing

public class StageSelectManger : MonoBehaviour
{
    public int nextstageid;
    public static StageSelectManger Instance { get; private set; }

    public RectTransform uiElement;                   // 動かすUI要素
    public List<RectTransform> targetRectTransforms;  // UIの目標位置のリスト
    public float moveSpeed = 2f;                      // 移動速度
    public float waitTime = 1f;                       // 移動後の待機時間
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1f); // 移動前のスケール
    public float scaleDuration = 0.5f;                // スケールのアニメーション時間

    private Vector2 initialAnchoredPosition;          // UIの初期位置
    private Vector3 initialScale;                     // UIの初期スケール

    public List<selectupdown> selectupdowns;
    public GameObject UIBlock;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (uiElement != null)
        {
            initialAnchoredPosition = uiElement.anchoredPosition;
            initialScale = uiElement.localScale;
            await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime / 2));

            if (DataManager.Instance.currentstage == 0 && DataManager.Instance.remaincount == 0)
            {
                StartInitialMovement().Forget();
            }
            else
            {
                HandleInitialState();
            }
        }
        else
        {
            Debug.LogError("No UI element specified for movement!");
        }
    }

    private async UniTask StartInitialMovement()
    {
        UIBlock.SetActive(true);
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime));

        if (targetRectTransforms == null || targetRectTransforms.Count == 0)
        {
            Debug.LogError("No target RectTransforms set!");
            return;
        }

        Vector2 targetPosition = targetRectTransforms[0].anchoredPosition;
        targetPosition = -targetPosition; // X軸とY軸を両方反転

        await AdjustScale(targetScale, scaleDuration);
        await MoveUIToTargetWithScale(targetPosition, moveSpeed);

        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime));
        await MoveUIToInitialPosition(initialAnchoredPosition, moveSpeed);
        await AdjustScale(initialScale, scaleDuration);

        UIBlock.SetActive(false);
    }

    public async UniTask StartMovementToIndex(int index)
    {
        UIBlock.SetActive(true);
        if (targetRectTransforms == null || targetRectTransforms.Count == 0)
        {
            Debug.LogError("No target RectTransforms set!");
            return;
        }

        if (index < 0 || index >= targetRectTransforms.Count)
        {
            Debug.LogError("Invalid index for target RectTransforms!");
            return;
        }

        Vector2 targetPosition = targetRectTransforms[index].anchoredPosition;
        targetPosition = -targetPosition; // X軸とY軸を両方反転

        await MoveUIToTargetWithScale(targetPosition, moveSpeed);
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime));

        UIBlock.SetActive(false);
    }

    public async UniTask StartMovementToIndex_nodelay(int index)
    {
        if (targetRectTransforms == null || targetRectTransforms.Count == 0)
        {
            Debug.LogError("No target RectTransforms set!");
            return;
        }

        if (index < 0 || index >= targetRectTransforms.Count)
        {
            Debug.LogError("Invalid index for target RectTransforms!");
            return;
        }

        Vector2 targetPosition = targetRectTransforms[index].anchoredPosition;
        targetPosition = -targetPosition; // X軸とY軸を両方反転
        await MoveUIToTargetWithScale(targetPosition, moveSpeed * 100000f);
    }

    private async UniTask AdjustScale(Vector3 targetScale, float duration)
    {
        await uiElement.DOScale(targetScale, duration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    private async UniTask MoveUIToTargetWithScale(Vector2 targetPos, float speed)
    {
        Vector3 currentScale = uiElement.localScale;
        Vector2 adjustedTargetPos = new Vector2(targetPos.x * currentScale.x, targetPos.y * currentScale.y);

        float distance = Vector2.Distance(uiElement.anchoredPosition, adjustedTargetPos);
        float duration = distance / speed;

        await uiElement.DOAnchorPos(adjustedTargetPos, duration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    private async UniTask MoveUIToInitialPosition(Vector2 initialPos, float speed)
    {
        float distance = Vector2.Distance(uiElement.anchoredPosition, initialPos);
        float duration = distance / speed;

        await uiElement.DOAnchorPos(initialPos, duration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }
    public void selectmanagerUpdate()
    {

    }
    private async void HandleInitialState()
    {
        if (DataManager.Instance.currentstage == 1)
        {
            StartMovementToIndex(selectupdowns[0].up ? 1 : 2).Forget();
        }
        else if (DataManager.Instance.currentstage == 2)
        {
            StartMovementToIndex_nodelay(1).Forget();
            StartMovementToIndex(3).Forget();
        }
        else if (DataManager.Instance.currentstage == 3)
        {
            StartMovementToIndex_nodelay(2).Forget();
            StartMovementToIndex(4).Forget();
        }
        else if (DataManager.Instance.currentstage == 4 || DataManager.Instance.currentstage == 5)
        {
            StartMovementToIndex_nodelay(3).Forget();
            StartMovementToIndex(5).Forget();
        }
        else if (DataManager.Instance.currentstage == 6 || DataManager.Instance.currentstage == 7)
        {
            StartMovementToIndex_nodelay(4).Forget();
            StartMovementToIndex(6).Forget();
        }
        else if (DataManager.Instance.currentstage == 8)
        {
            StartMovementToIndex_nodelay(5).Forget();
            StartMovementToIndex(7).Forget();
        }
        else if (DataManager.Instance.currentstage == 9)
        {
            StartMovementToIndex_nodelay(6).Forget();
            StartMovementToIndex(7).Forget();
        }
    }

    public void selectstage(int id)
    {
        nextstageid = -1;
        switch (id)
        {
            case 1:
                if (DataManager.Instance.clearcount == 0)
                {
                    nextstageid = 1;
                }
                break;
            case 2:
                if (DataManager.Instance.clearcount == 1 && selectupdowns[0].up)
                {
                    nextstageid = 2;
                }
                break;
            case 3:
                if (DataManager.Instance.clearcount == 1 && !selectupdowns[0].up)
                {
                    nextstageid = 3;
                }
                break;
            case 4:
                if (DataManager.Instance.clearcount == 2 && selectupdowns[0].up && selectupdowns[1].up)
                {
                    nextstageid = 4;
                }
                break;
            case 5:
                if (DataManager.Instance.clearcount == 2 && selectupdowns[0].up && !selectupdowns[1].up)
                {
                    nextstageid = 5;
                }
                break;
            case 6:
                if (DataManager.Instance.clearcount == 2 && !selectupdowns[0].up && selectupdowns[2].up)
                {
                    nextstageid = 6;
                }
                break;
            case 7:
                if (DataManager.Instance.clearcount == 2 && !selectupdowns[0].up && !selectupdowns[2].up)
                {
                    nextstageid = 7;
                }
                break;
            case 8:
                if (DataManager.Instance.clearcount == 3 && selectupdowns[0].up)
                {
                    nextstageid = 8;
                }
                break;
            case 9:
                if (DataManager.Instance.clearcount == 3 && !selectupdowns[0].up)
                {
                    nextstageid = 9;
                }
                break;
            case 10:
                if (DataManager.Instance.clearcount == 4)
                {
                    nextstageid = 10;
                }
                break;


        }
        if (nextstageid != -1)
        {
            StageManger.Instance.Stage_select_off();
            StageManger.Instance.Party_on();
            DataManager.Instance.nextcurrentstage = nextstageid;
        }
    }
}

