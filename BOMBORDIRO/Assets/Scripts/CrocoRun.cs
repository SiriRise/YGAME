using UnityEngine;

public class CrocoRun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement Settings")]
    public float speed = 1f;
    [SerializeField]
    private AnimationCurve trajectoryCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.5f, 1),
        new Keyframe(1, 0)
    );

    [Header("Movement Range")]
    public float maxRightOffset = 2f;
    public float maxLeftOffset = -2f;

    private Vector2 startPosition;
    private float timePosition;
    private Vector3 originalScale;
    private float previousHorizontalOffset;

    private void Start()
    {
        startPosition = transform.position;
        timePosition = 0f;
        originalScale = transform.localScale;
        previousHorizontalOffset = 0f;
    }

    private void Update()
    {
        // ��������� ������� �������
        timePosition += Time.deltaTime * speed;
        if (timePosition > 1f) timePosition -= 1f;

        // �������� �������� ������
        float curveValue = trajectoryCurve.Evaluate(timePosition);
        float horizontalOffset = Mathf.Lerp(maxLeftOffset, maxRightOffset, curveValue);

        // ���������� ����������� ��������
        float deltaMovement = horizontalOffset - previousHorizontalOffset;
        if (deltaMovement > 0)
        {
            // �������� ������
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else if (deltaMovement < 0)
        {
            // �������� �����
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }

        // ��������� �������
        transform.position = startPosition + new Vector2(horizontalOffset, 0);
        previousHorizontalOffset = horizontalOffset;
    }
}
