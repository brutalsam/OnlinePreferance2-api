using Newtonsoft.Json;
using OnlinePreferance2_api.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePreferance2_api.Model
{
    public class Player
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string PlayerName { get; set; }
        public Player()
        {

        }
    }

    public class GameCreateModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Description { get; set; }
    }

    public class Game: BaseEntity
    {
        public List<Player> Players { get; set; }
        public List<Deal> Deals { get; set; }
        public bool IsEnded { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }
}