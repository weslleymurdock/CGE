using System;

namespace CardGameEngine
{
    [Serializable]
    public class EndPlayMonsterCardEvent : EndPlayCardEvent
    {
/// <summary>Initializes a new instance of the <see cref="EndPlayMonsterCardEvent"/> type.</summary>
/// <param name="monsterCard">The monsterCard value.</param>
/// <param name="boardIndex">The boardIndex value.</param>
        public EndPlayMonsterCardEvent(IMonsterCard monsterCard, int boardIndex)
            : base(monsterCard)
        {
            BoardIndex = boardIndex;
        }

/// <summary>Initializes a new instance of the <see cref="EndPlayMonsterCardEvent"/> type.</summary>
/// <param name="getMonsterCard">The getMonsterCard value.</param>
/// <param name="boardIndex">The boardIndex value.</param>
        public EndPlayMonsterCardEvent(Func<IMonsterCard> getMonsterCard, int boardIndex)
            : base(getMonsterCard)
        {
            BoardIndex = boardIndex;
        }

        public int BoardIndex { get; protected set; }
    }
}
