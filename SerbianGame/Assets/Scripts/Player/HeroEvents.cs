using UnityEngine;

public class HeroEvents : MonoBehaviour
{
    private EventBinding<PlayerEvent> playerEventBinding;
    //private EventBinding<HarvestEvent> harvestEventBinding;

    private SpellStrategy[] spells;
    private Hero hero;

    public void Initialize(SpellStrategy[] spells, Hero hero)
    {
        this.spells = spells;
        this.hero = hero;
    }

    public void RegisterEvents()
    {
        playerEventBinding = new EventBinding<PlayerEvent>(i => HandleActionType(i.actionType));
        //harvestEventBinding = new EventBinding<HarvestEvent>(Harvest);

        //EventBus<HarvestEvent>.Register(harvestEventBinding);
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }

    public void DeRegisterEvents()
    {
        //EventBus<HarvestEvent>.Deregister(harvestEventBinding);
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }

    private void HandleActionType(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Attack:
                hero.GetComponent<SwordAttack>().EnableSwordCollider();
                break;
            case ActionType.Harvest:
                hero.GetComponent<HarvestSystem>().Harvest();
                break;
            case ActionType.None:
                Debug.Log("No action performed.");
                break;
            default:
                Debug.LogWarning($"Unhandled action type: {actionType}");
                break;
        }
    }

    private void InitiateCastSpell(int index)
    {
        spells[index].CastSpell(transform);
    }

    private void Harvest()
    {
        hero.HarvestSystem.Harvest();
    }
}

