using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static public GameManager Instance;

    [SerializeField] private Vector3 playerPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the new scene and set its position
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = GameManager.Instance.playerPosition;
            // Apply any other player state adjustments here
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      

        // Check if the Z key was pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("here");
            // Find the player in the new scene and set its position
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                playerPosition = player.transform.position;
            }
            

            // Load the scene by name or index
            SceneManager.LoadScene("BaseScene2");
            Debug.Log("scene switch");
        }


    }
}
