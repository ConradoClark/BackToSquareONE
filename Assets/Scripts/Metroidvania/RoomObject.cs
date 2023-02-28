using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine.SceneManagement;

public class RoomObject : BaseGameObject
{
    public Room Room { get; private set; }
    protected RoomManager RoomManager { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        Room = GetComponentInParent<Room>(true);
        RoomManager = RoomManager.FromScene();

        Room.OnActivation += Room_OnActivation;
        Room.OnDeactivation += Room_OnDeactivation;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        Room.OnActivation -= Room_OnActivation;
        Room.OnDeactivation -= Room_OnDeactivation;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
    }

    private void Room_OnDeactivation()
    {
        gameObject.SetActive(false);
    }

    private void Room_OnActivation()
    {
        gameObject.SetActive(true);
    }
}
