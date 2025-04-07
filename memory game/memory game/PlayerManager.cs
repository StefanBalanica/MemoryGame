using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace memory_game
{
    internal class PlayerManager
    {
        private static string filePath = "players.json";

        public static void SavePlayers(List<Player> players)
        {
            string json = JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static List<Player> LoadPlayers()
        {
            if (!File.Exists(filePath))
                return new List<Player>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Player>>(json);
        }
    }
}
