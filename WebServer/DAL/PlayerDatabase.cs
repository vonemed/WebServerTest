using SharedLibrary.Models;

namespace WebServer.DAL;

public static class PlayerDatabase
{
    private static readonly List<Player> Players = new List<Player>();

    public static void AddPlayer(Player player)
    {
        if(Players.Exists(x => x.Name == player.Name)) Console.WriteLine("The player with this name already exists");
        else
        {
            Players.Add(player);
            Console.WriteLine("Added player to database");
        }
    }

    public static Player? GetPlayerByName(string name)
    {
        var player = Players.Find(x => x.Name == name);

        if (player != null) return player;
        else
        {
            Console.WriteLine($"Can't find the player with that name {name}");
            return null;
        }
    }
    
    // public static Player? GetPlayerById(Guid id)
    // {
    //     var player = Players.Find(x => x.Id == id);
    //
    //     if (player != null) return player;
    //     else
    //     {
    //         Console.WriteLine($"Can't find the player with that Id {id}");
    //         return null;
    //     }
    // }

    public static void DisplayPlayers()
    {
        foreach (var player in Players)
        {
            Console.WriteLine($"Name: {player.Name}");
        }
    }
}