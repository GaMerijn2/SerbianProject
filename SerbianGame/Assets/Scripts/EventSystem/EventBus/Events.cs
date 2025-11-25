public interface IEvent { }

public struct HarvestEvent : IEvent { }

public struct EnemyManagerEvent : IEvent { }

public struct PlayerEvent : IEvent
{
    public ActionType actionType;
}

public struct PlayerMagicEvent : IEvent
{
    public int spellIndex;
}

public struct OnPortalEvent : IEvent 
{ 
    public SceneType type;
}

public enum ActionType
{
    None,
    Attack,
    Harvest,
}
