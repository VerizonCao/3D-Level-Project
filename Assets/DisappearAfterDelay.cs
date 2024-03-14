using UnityEngine;

public class DisappearAfterDelay : MonoBehaviour
{
    public float delayInSeconds = 3f; // 设置延迟时间，默认为3秒
    [SerializeField] string stringName = "none";

    private void OnEnable()
    {
        // 在物体被激活时开始计时
        Invoke("HideObject", delayInSeconds);
    }

    private void HideObject()
    {
        // 当计时结束时将物体设置为非激活状态
        gameObject.SetActive(false);

        // start the ending camera stuff
        GameManager.Instance.TurnOnAndCloseOnceReachEnd(stringName);
    }
}
