using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private SceneInfo[] scenes;

    [SerializeField] private InputActionAsset inputActionsAsset;
    private InputAction interactAction;

    EventBinding<OnPortalEvent> onPortalEventBinding;

    public static CustomSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        interactAction = inputActionsAsset.FindActionMap("Player").FindAction("Interact");
        interactAction.performed += OnInteract;
        interactAction.Enable();

        onPortalEventBinding = new EventBinding<OnPortalEvent>(HandlePortalEvent);
        EventBus<OnPortalEvent>.Register(onPortalEventBinding);
    }

    private void OnDisable()
    {
        interactAction.performed -= OnInteract;
        interactAction.Disable();

        EventBus<OnPortalEvent>.Deregister(onPortalEventBinding);
    }

    void HandlePortalEvent(OnPortalEvent onPortalEvent)
    {
        LoadScene(onPortalEvent.type);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Example: use interact to toggle between scenes
        if (SceneManager.GetActiveScene().name == "CampScene")
        {
            Debug.Log("Interacted: Loading Game Scene");
            EventBus<OnPortalEvent>.Raise(new OnPortalEvent { type = SceneType.Game });
        }
        else
        {
            Debug.Log("Interacted: Loading Camp Scene");
            EventBus<OnPortalEvent>.Raise(new OnPortalEvent { type = SceneType.Camp });
        }
    }

    public void LoadScene(SceneType type = SceneType.None)
    {
        SceneInfo sceneInfo = GetSceneInfo(type);

        if (sceneInfo != null)
            SceneManager.LoadScene(sceneInfo.sceneName);
        else
            Debug.LogError($"Scene of type {type} not found!");
    }

    private SceneInfo GetSceneInfo(SceneType type)
    {
        foreach (SceneInfo sceneInfo in scenes)
            if (sceneInfo.type == type)
                return sceneInfo;
        
        return null;
    }
}

