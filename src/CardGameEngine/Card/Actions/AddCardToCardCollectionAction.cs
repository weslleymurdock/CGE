using Newtonsoft.Json;

namespace CardGameEngine;

public class AddCardToCardCollectionAction : Action
{
    [JsonProperty]
    protected ICardCollection cardCollection = null!;

    [JsonProperty]
    protected ICard card = null!;

    protected AddCardToCardCollectionAction() { }

    public AddCardToCardCollectionAction(ICardCollection cardCollection, ICard card, bool isAborted = false)
        : base(isAborted)
    {
        this.cardCollection = cardCollection;
        this.card = card;
    }

    [JsonIgnore]
    public ICardCollection CardCollection
    {
        get => cardCollection;
    }

    [JsonIgnore]
    public ICard Card
    {
        get => card;
    }

    public override void Execute(IEngine game)
    {
        CardCollection.Add(Card);
    }

    public override bool IsExecutable(IEngineState gameState)
    {
        return Card != null && !CardCollection.IsFull;
    }
}
