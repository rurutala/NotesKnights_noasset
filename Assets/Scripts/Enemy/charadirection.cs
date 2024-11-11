/*
using UnityEngine;

public class SetRotation : MonoBehaviour
{
    public Transform parentObject;  // �e�I�u�W�F�N�g

    void Update()
    {
        float parentYRotation = parentObject.eulerAngles.y;

        // �e�I�u�W�F�N�g��Y����180�x�̏ꍇ
        if (Mathf.Approximately(parentYRotation, 180f))
        {
            // �q�I�u�W�F�N�g��X����20�AY����180�AZ����0�ɃZ�b�g
            transform.rotation = Quaternion.Euler(20f, 0f, 0f);
            Debug.Log("Parent Y axis is 180. Child rotation set to (20, 180, 0).");
        }
        // �e�I�u�W�F�N�g��Y����-90�x�̏ꍇ�iUnity�ł�270�x�Ƃ��Ĉ�����j
        else if (Mathf.Approximately(parentYRotation, 270f))
        {
            // �q�I�u�W�F�N�g��X����20�AY����-270�AZ����0�ɃZ�b�g
            transform.rotation = Quaternion.Euler(20f, -360f, 0f);
            Debug.Log("Parent Y axis is -90 (270 in Unity). Child rotation set to (20, -270, 0).");
        }
        // �e�I�u�W�F�N�g��Y����90�x�̏ꍇ
        else if (Mathf.Approximately(parentYRotation, 90f))
        {
            // �q�I�u�W�F�N�g��X����-20�AY����90�AZ����0�ɃZ�b�g
            transform.rotation = Quaternion.Euler(-20f, 180f, 0f);
            Debug.Log("Parent Y axis is 90. Child rotation set to (-20, 90, 0).");
        }
        // �e�I�u�W�F�N�g��Y����0�x�̏ꍇ
        else if (Mathf.Approximately(parentYRotation, 0f))
        {
            // �q�I�u�W�F�N�g��X����20�AY����0�AZ����0�ɃZ�b�g
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
    private Quaternion initialRotation; // ������]��ێ�

    void Start()
    {
        // �q�I�u�W�F�N�g��SpriteRenderer���擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �e�I�u�W�F�N�g��Transform���擾
        parentTransform = transform.parent;

        if (parentTransform == null)
        {
            Debug.LogError("Parent object is not assigned.");
            return;
        }

        // �q�I�u�W�F�N�g�̏�����]��ۑ�
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // �����̉�]���ێ�
        transform.rotation = initialRotation;

        // �e�I�u�W�F�N�g�̃��[�J����]�p�x���擾
        float parentYRotation = parentTransform.localEulerAngles.y;

        // �e�I�u�W�F�N�g�̌����ɂ���ăX�v���C�g�𔽓]
        if (IsFacingRight(parentYRotation) || IsFacingDown(parentYRotation))
        {
            spriteRenderer.flipX = true; // �E�����܂��͉������F���]
        }
        else if (IsFacingUp(parentYRotation) || IsFacingLeft(parentYRotation))
        {
            spriteRenderer.flipX = false; // ������܂��͍������F���]�Ȃ�
        }
    }

    // �E�������� (90�x �} 10�x)
    private bool IsFacingRight(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 90f)) <= 10f;
    }

    // ���������� (-90�x �} 10�x)
    private bool IsFacingLeft(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, -90f)) <= 10f;
    }

    // ��������� (0�x �} 10�x)
    private bool IsFacingUp(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 0f)) <= 10f;
    }

    // ���������� (180�x �} 10�x)
    private bool IsFacingDown(float yRotation)
    {
        return Mathf.Abs(Mathf.DeltaAngle(yRotation, 180f)) <= 10f;
    }
}
