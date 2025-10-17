
public interface ICommand
{
    void Execute();
}

public class AbilityCommand : ICommand
{
    private readonly AbilityData data;
    public float duration => data.duration;

    public AbilityCommand(AbilityData data)
    {
        this.data = data;
    }

    public void Execute()
    {
        EventBus<PlayerAnimationEvent>.Raise(new PlayerAnimationEvent
        {
            animationHash = data.animationHash, tweenId = data.tweenId
        });
        EventBus<PlayerEvent>.Raise(new PlayerEvent
        {
            actionType = data.actionType
        });
    }
}

public struct PlayerAnimationEvent : IEvent
{
    public string tweenId;
    public int animationHash;
}
