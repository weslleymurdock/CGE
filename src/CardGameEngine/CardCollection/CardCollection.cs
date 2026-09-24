using System;
using System.Collections.Generic;

namespace CardGameEngine
{
    [Serializable]
    public abstract class CardCollection : ICardCollection
    {
        /// <summary>
        /// Abstract representation of a collection of Cards.
        /// </summary>
        public CardCollection()
        {
        }

        public abstract int Size { get; }
        public abstract List<ICard> AllCards { get; }
        public abstract bool IsEmpty { get; }
/// <summary>Creates a copy of the current object.</summary>
/// <returns>The result of the operation.</returns>
        public abstract object Clone();
/// <summary>Performs the Contains operation.</summary>
/// <param name="card">The card value.</param>
/// <returns>The result of the operation.</returns>
        public abstract bool Contains(ICard card);
    }
}
