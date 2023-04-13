using OnlinePreferance2_api.Database.Repositories;
using System.Collections;
using System.Collections.Generic;

namespace OnlinePreferance2_api.Model
{
    public class PlayerDeck: BaseEntity
    {
        public List<Card> Cards { get; set; }

        public PlayerDeck()
        {
            Cards = new List<Card>();
        }
    }

    public class Round: BaseEntity
    {
        public List<Card> Rounds { get; set; }
        public Round()
        {
            Rounds = new List<Card>();
        }
    }
    public class Deal : BaseEntity
    {
        public int Dealer { get; set; }
        public Contract DealContract { get; set; }
        //Card Deck per player
        public List<PlayerDeck> InitialCards { get; set; }
        public List<Round> Rounds { get; set; }
        public Deal()
        {
            Rounds = new List<Round>();
            InitialCards = new List<PlayerDeck>();
        }
    }
}