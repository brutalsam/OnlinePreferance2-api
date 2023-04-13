using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnlinePreferance2_api.Model
{
    public class GameVievItem
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        public List<Player> Players { get; set; }
        public bool IsEnded { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }

    public static class GameVievItemExtension
    {
        public static GameVievItem ToPoco(this Game game)
        {
            return new GameVievItem()
            {
                Id = game.Id,
                Players = game.Players,
                IsEnded = game.IsEnded,
                Description = game.Description
            };
        }
    }
}