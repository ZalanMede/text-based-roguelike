using System;
using System.IO;
using FileIO;
using text_based_roguelike.Model;

internal class Program
{
    private static Character playerCharacter;
    private static List<Character> allCharacterListImported = new List<Character>();
    private static List<Enemy> allEnemiesListImported = new List<Enemy>();
    private static List<Weapon> allWeaponsListImported = new List<Weapon>();
    private static void Main(string[] args)
    {
        ReadFromDatabase();
        StartGame();
    }

    private static void ReadFromDatabase()
    {
        Console.WriteLine("DB olvasás");
    }

    private static void StartGame()
    {
        Console.WriteLine("Válassz karaktert: ");
        foreach (var item in allCharacterListImported)
        {
            Console.WriteLine($"\t {item}");
        }
    }
}