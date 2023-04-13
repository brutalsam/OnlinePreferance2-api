using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlinePreferance2_api.Model;

namespace OnlinePreferance2_api.Services
{
    public class GameService
    {
        public static async Task<Game> CreateNewGame(GameCreateModel createGameModel, UserManager<IdentityUser> userManager)
        {
            var players = new List<Player>();
            players.Add(await GetPlayerFromEmail(createGameModel.Player1, userManager));
            players.Add(await GetPlayerFromEmail(createGameModel.Player2, userManager));
            players.Add(await GetPlayerFromEmail(createGameModel.Player3, userManager));
            var newGame = new Game
            {
                Players = players,
                IsEnded = false,
                Deals = new List<Deal>(),
                CreationDate = DateTime.Now
             };
            newGame.Deals.Add(new Deal());
            //var initialDeal = CreateDeal();
            //newGame.Deals.Add(initialDeal);
            newGame.Description = createGameModel.Description;
            return newGame;
        }

        public static Deal CreateDeal()
        {
            var newDeal = new Deal();
            var deck = new Deck();

            var list = deck.GetAllShuffledCards();

            // Draw on  three players`
            var player1Cards = new PlayerDeck();
            var player2Cards = new PlayerDeck();
            var player3Cards = new PlayerDeck();

            for (var i = 0; i <= 4; i++)
            {
                player1Cards.Cards.AddRange(list.Skip(i * 6).Take(2));
                player2Cards.Cards.AddRange(list.Skip(i * 6 + 2).Take(2));
                player3Cards.Cards.AddRange(list.Skip(i * 6 + 4).Take(2));
            }

            newDeal.InitialCards.Add(player1Cards);
            newDeal.InitialCards.Add(player2Cards);
            newDeal.InitialCards.Add(player3Cards);

            return newDeal;
        }

        private static async Task<Player> GetPlayerFromEmail(string email, UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);
            return new Player
            {
                Id = user.Id,
                PlayerName = user.UserName
            };
        }
    }
}