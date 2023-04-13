using System;
using System.Collections.Generic;
using System.Linq;
using OnlinePreferance2_api.Database.Repositories;
using OnlinePreferance2_api.Utils;

namespace OnlinePreferance2_api.Model
{
    public enum CardSuit
    {
        None = 0,
        Spade = 1,
        Club = 2,
        Diamond = 3,
        Heart = 4,
    }

    public enum CardValue
    {
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }

    public class Card: BaseEntity
    {
        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }
        public CardValue Value { get; set; }
        public CardSuit Suit { get; set; }

        public override string ToString()
        {
            return $"{Value.ToString()} {Suit.ToString()}";
        }
    }

    public class Deck
    {
        private List<Card> cards;
        public Deck()
        {

            cards = new List<Card>();
            for (var i = 1; i <= 4; i++)
            {
                for (var j = 7; j <= 14; j++)
                {
                    cards.Add(new Card((CardValue)j, (CardSuit)i));
                }
            }
        }

        public IEnumerable<Card> GetAllCards()
        {
            return cards;
        }

        public IEnumerable<Card> GetAllShuffledCards()
        {
            var list = cards.ToList();
            list.Shuffle();
            return list;
        }
    }
}