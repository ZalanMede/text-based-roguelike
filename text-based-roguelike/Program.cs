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
    private static int characterBaseHP;
    private static int currentDifficulty = 1;
    private static bool inBlock;
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
        DataTable CharacterItemsDBT = DatabaseServices.GetAllData("characteritems", connectionString);
        DataTable EnemyLootPoolDBT = DatabaseServices.GetAllData("enemylootpool", connectionString);
        DataTable WeaponsDBT = DatabaseServices.GetAllData("weapons", connectionString);

        foreach (DataRow r in CharactersDBT.Rows)
        {
            Character tempChar = new Character(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), Array.Empty<int>());
            allCharacterListImported.Add(tempChar);
        }

        foreach (DataRow r in EnemiesDBT.Rows)
        {
            Enemy tempChar = new Enemy(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), r.Field<int>("rarity"));
            allEnemiesListImported.Add(tempChar);
        }

        foreach (DataRow r in WeaponsDBT.Rows)
        {
            Weapon tempChar = new Weapon(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("atk"));
            allWeaponsListImported.Add(tempChar);
        }
    }

    private static void StartGame()
    {
        int numIn = 1;
        bool stupidChecker = true;
        while (stupidChecker)
        {
            Console.WriteLine("Válassz karaktert: ");
            foreach (var item in allCharacterListImported)
            {
                Console.WriteLine($"{item}");
            }

            Console.WriteLine($"Írd be a választott karakter sorszámát (1-{allCharacterListImported.Count})");
            try
            {
                numIn = Convert.ToInt32(Console.ReadLine());

                // Good
                stupidChecker = false;
                playerCharacter = allCharacterListImported[numIn-1];
                characterBaseHP = allCharacterListImported[numIn - 1].Hp;
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Rossz input");
            }
        }

        EncounterEnemyRoll();
    }

    private static void EncounterEnemyRoll()
    {
        RstConsole();

        Enemy[] currentDifficultyEnemies = allEnemiesListImported.FindAll(x => x.Rarity == currentDifficulty).ToArray();
        Random rnd = new Random();
        Enemy tempEnemy = allEnemiesListImported[rnd.Next(0, currentDifficultyEnemies.Length - 1)];

        Console.WriteLine($"Egy {tempEnemy.Name} megjelent");
        Console.WriteLine("");
        EnemyEncounter(tempEnemy);
    }

    private static void RstConsole()
    {
        Console.Clear();
        Console.WriteLine($"{playerCharacter.Name} || HP: {playerCharacter.Hp}/{characterBaseHP} || ATK:{playerCharacter.Atk} || SPD:{playerCharacter.Atk}");
        Console.WriteLine("  ");
    }

    private static void EnemyEncounter(Enemy currectEnemy)
    {
        // Turn reset
        inBlock = false;

        // Action log
        Console.WriteLine($"A(z) {currectEnemy.Name} ácsorog");
        Console.WriteLine("Mit teszel?");
        Console.WriteLine($"\t[1] Támadás");
        Console.WriteLine($"\t[2] Tárgy használata");
        Console.WriteLine($"\t[3] Védekezés");
        Console.WriteLine("");

        // Battle Input
        int battleInputInt = Convert.ToInt32(Console.ReadLine());
        switch (battleInputInt)
        {
            case 1:
                AttackEnemy();
                break;
            case 2:
                ItemMenu();
                break;
            case 3:
                inBlock = true;
                break;
        }
    }

    private static void ItemMenu()
    {
        
    }

    private static void AttackEnemy()
    {
        
    }
}