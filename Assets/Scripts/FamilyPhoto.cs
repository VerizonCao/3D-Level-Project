using UnityEngine;

public class FamilyPhoto : MonoBehaviour
{
    [SerializeField] private DetectionCircle detectionCircle;
    [SerializeField] private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            player.switchCamera(true);

      

        }
        else
        {
            player.switchCamera(false);
        }

    }
}
