using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlinePreferance2_api.Model;
using OnlinePreferance2_api.Model.Auth;

namespace OnlinePreferance2_api.Services
{
    public class GameService
    {
        public static async Task<Game> CreateNewGame(GameCreateModel createGameModel, UserManager<ApplicationUser> userManager)
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
            //newGame.Deals.Add(new Deal());
            var initialDeal = CreateDeal(players);
            initialDeal.DealContract = new Contract { ContractValue = (int)ContractValue.Six, Trumps = CardSuit.Heart};
            newGame.Deals.Add(initialDeal);
            newGame.Description = createGameModel.Description;
            return newGame;
        }

        public static Deal CreateDeal(List<Player> players)
        {
            var newDeal = new Deal();
            var deck = new Deck();

            var list = deck.GetAllShuffledCards();

            // Draw on  three players
            var player1Cards = new PlayerDeck(players[0]);
            var player2Cards = new PlayerDeck(players[1]);
            var player3Cards = new PlayerDeck(players[2]);

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

        private static async Task<Player> GetPlayerFromEmail(string email, UserManager<ApplicationUser> userManager)
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