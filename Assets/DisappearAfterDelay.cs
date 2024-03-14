using UnityEngine;

public class DisappearAfterDelay : MonoBehaviour
{
    public float delayInSeconds = 3f; // �����ӳ�ʱ�䣬Ĭ��Ϊ3��
    [SerializeField] string stringName = "none";

    private void OnEnable()
    {
        // �����屻����ʱ��ʼ��ʱ
        Invoke("HideObject", delayInSeconds);
    }

    private void HideObject()
    {
        // ����ʱ����ʱ����������Ϊ�Ǽ���״̬
        gameObject.SetActive(false);

        // start the ending camera stuff
        GameManager.Instance.TurnOnAndCloseOnceReachEnd(stringName);
    }
}
