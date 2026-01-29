using FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using text_based_roguelike.Database;
using text_based_roguelike.Model;

internal class Program
{
    public static readonly string connectionString = "Server=localhost;Database=roguelike_db;User=root;";
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
        Console.WriteLine("DB olvasás:");
        DatabaseServices.DBConnectionCheck(connectionString);

        DataTable CharactersDBT = DatabaseServices.GetAllData("characters", connectionString);
        DataTable EnemiesDBT = DatabaseServices.GetAllData("enemies", connectionString);
        DataTable CharacterItemsDBT = DatabaseServices.GetAllData("charactersitems", connectionString);
        DataTable EnemyLootPoolDBT = DatabaseServices.GetAllData("enemylootpool", connectionString);
        DataTable WeaponsDBT = DatabaseServices.GetAllData("weapons", connectionString);

        foreach (DataRow r in CharactersDBT.Rows)
        {
            Character tempChar = new Character(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), Array.Empty<int>());
        }

        foreach (DataRow r in EnemiesDBT.Rows)
        {
            Enemy tempChar = new Enemy(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), r.Field<int>("rarity"));
        }

        foreach (DataRow r in WeaponsDBT.Rows)
        {
            Weapon tempChar = new Weapon(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("atk"));
        }
    }

    private static void StartGame()
    {
        Console.WriteLine("Válassz karaktert: ");
        foreach (var item in allCharacterListImported)
        {
            Console.WriteLine($"\t {item}");
        }
        Console.WriteLine($"Írd be a választott karakter sorszámát (1-{allCharacterListImported.Count})");
    }
}