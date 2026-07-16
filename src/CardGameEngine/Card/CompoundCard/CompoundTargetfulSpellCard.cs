namespace CardGameEngine;

[Serializable]
public class CompoundTargetfulSpellCard : CompoundCard, ITargetfulSpellCard
{
    public CompoundTargetfulSpellCard(List<ISpellCard> components, string name = "")
        : base([], name)
    {
        if(components.Find(c => c is ITargetfulSpellCard) == null)
        {
            throw new CardGameEngineException("Tried to construct a CompoundTargetfulSpellCard " +
                "without a component of type ITargetfulSpellCard.\n" +
                "Use CompoundTargetlessSpellCard instead.");
        }
        components.ForEach(c => Components.Add(c));
    }

    public CompoundTargetfulSpellCard(ISpellCard spellCard)
        : this([spellCard])
    {
    }

    public override object Clone()
    {
        return new CompoundTargetfulSpellCard(Components.ConvertAll(c => (ISpellCard)c.Clone()));   
    }

    public HashSet<ICharacter> GetPotentialTargets(IGameState gameState)
    {
        //Compute the intersection of all potential targets
        HashSet<ICharacter> potentialTargets = default!;
        foreach(ICard card in Components.FindAll(c => c is ITargetful)) //TODO: check if this actually works!
        {
            if (potentialTargets == default!)
            {
                potentialTargets = ((ITargetful)card).GetPotentialTargets(gameState);
            }
            else
            {
                HashSet<ICharacter> potTargets = ((ITargetful)card).GetPotentialTargets(gameState);
                potentialTargets.RemoveWhere(t => !potTargets.Contains(t));
            }
        }
        return potentialTargets;
    }

    public void Cast(IGame game, ICharacter targetCharacter)
    {
        foreach (ICard card in Components)
        {
            if (card is ITargetfulSpellCard c) //TODO: check if this actually works!
            {
                if (c.IsCastable(game))
                {
                    c.Cast(game, targetCharacter);
                }
            } 
            else
            {
                if ((card is ITargetlessSpellCard sc) && sc.IsCastable(game))
                {
                    sc.Cast(game);
                }
            }
        }
    }
}
