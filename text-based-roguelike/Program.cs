using FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using text_based_roguelike.Database;
using text_based_roguelike.Model;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    public static readonly string connectionString = "Server=localhost;Database=roguelike_db;User=root;";
    private static Character playerCharacter;
    private static int playerCharacterListIndex;
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
        DataTable ItemsDBT = DatabaseServices.GetAllData("items", connectionString);
        DataTable EnemyLootPoolDBT = DatabaseServices.GetAllData("enemylootpool", connectionString);
        DataTable WeaponsDBT = DatabaseServices.GetAllData("weapons", connectionString);

        foreach (DataRow r in CharactersDBT.Rows)
        {
            Character tempChar = new Character(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), Array.Empty<Item>());
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
            Console.WriteLine("Start Game: ");
            foreach (var item in allCharacterListImported)
            {
                Console.WriteLine($"{item}");
            }

            Console.WriteLine($"Choose a character from 1 to {allCharacterListImported.Count}: ");
            try
            {
                numIn = Convert.ToInt32(Console.ReadLine());
                if (numIn < 1 || allCharacterListImported.Count < numIn )
                {
                    throw(new Exception("RangeError"));
                }

                // Good
                stupidChecker = false;
                playerCharacterListIndex = numIn - 1;
                playerCharacter = allCharacterListImported[playerCharacterListIndex];
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Incorrect input!");
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

        Console.WriteLine($"A {tempEnemy.Name} appeared!");
        Console.WriteLine("");
        EnemyEncounter(tempEnemy);
    }

    private static void RstConsole()
    {
        Console.Clear();
        Console.WriteLine($"{playerCharacter.Name} || HP: {playerCharacter.Hp}/{allCharacterListImported[playerCharacterListIndex].Hp} || ATK:{playerCharacter.Atk} || SPD:{playerCharacter.Atk}");
        Console.WriteLine("  ");
    }

    private static void EnemyEncounter(Enemy currectEnemy)
    {
        // Turn reset
        inBlock = false;

        bool stupidChecker = true;
        while (stupidChecker)
        {
            // Action log
            Console.WriteLine($"The {currectEnemy.Name} stands idly");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"\t[1] Attack");
            Console.WriteLine($"\t[2] Use an item");
            Console.WriteLine($"\t[3] Block");
            Console.WriteLine("");

            try
            {
                int battleInputInt = Convert.ToInt32(Console.ReadLine());

                // Good
                stupidChecker = false;

                // Battle Input
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

                if (currectEnemy.Hp <= 0)
                {
                    Console.WriteLine($"The {currectEnemy.Name} died!");
                    RollEnemyLootPool(currectEnemy);
                }

                EnemyTurn(currectEnemy);
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Incorrect input!");
            }
        }
    }

    private static void RollEnemyLootPool(Enemy currectEnemy)
    {
        Console.WriteLine($"You search the {currectEnemy.Name}'s corpse for loot...");
        Console.WriteLine($"You get a potion!");
    }

    private static void EnemyTurn(Enemy currentEnemy)
    {
        RstConsole();
        Console.WriteLine($"The {currentEnemy.Name} attacks!");
        if (inBlock)
        {
            Console.WriteLine("You partially blocked the attack!");
            int damage = (int)Math.Floor(currentEnemy.Atk * 0.25);
            Console.WriteLine($"You take {damage} damage!");
        }
        else
        {
            Console.WriteLine("You got hit!");
            playerCharacter.Hp -= currentEnemy.Atk;
            Console.WriteLine($"You take {currentEnemy.Atk} damage!");
        }
    }

    private static void ItemMenu()
    {
        RstConsole();

        Console.WriteLine("Your items:");
        if (playerCharacter.Items.Length == 0)
        {
            Console.WriteLine("\tYou have no items!");
        }
        else 
        { 
            foreach (var item in playerCharacter.Items)
            {
                Console.WriteLine($"\t{item}");
            }
        }
    }

    private static void AttackEnemy()
    {
        
    }
}