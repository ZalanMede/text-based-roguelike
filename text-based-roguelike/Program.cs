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
    private static int originalPlayerIndex;
    private static int currentDifficulty = 1;
    private static bool inBlock;
    private static bool usedItem;
    private static Enemy currentEnemy;
    private static List<Character> allCharacterListImported = new List<Character>();
    private static List<Enemy> allEnemiesListImported = new List<Enemy>();
    private static List<Weapon> allWeaponsListImported = new List<Weapon>();
    private static int[] potionStatIncreases = new int[3] { 0, 0, 0};
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
        DataTable ItemsDBT = DatabaseServices.GetAllData("characteritems", connectionString);
        DataTable EnemyLootPoolDBT = DatabaseServices.GetAllData("enemylootpool", connectionString);
        DataTable WeaponsDBT = DatabaseServices.GetAllData("weapons", connectionString);

        foreach (DataRow r in CharactersDBT.Rows)
        {
            Character tempChar = new Character(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), new List<Item>());
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
                Character tempCharacterForCloning = allCharacterListImported[numIn - 1];
                playerCharacter = new Character(tempCharacterForCloning.Id, tempCharacterForCloning.Name, tempCharacterForCloning.Hp, tempCharacterForCloning.Atk, tempCharacterForCloning.Spd, tempCharacterForCloning.Items);
                originalPlayerIndex = numIn - 1;
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
        //Enemy[] currentDifficultyEnemies = allEnemiesListImported.FindAll(x => x.Rarity == currentDifficulty).ToArray();
        //Random rnd = new Random();
        //currentEnemy = allEnemiesListImported[rnd.Next(0, currentDifficultyEnemies.Length - 1)];

        Enemy tempEnemyForCloning = allEnemiesListImported[0];
        currentEnemy = new Enemy(tempEnemyForCloning.Id, tempEnemyForCloning.Name, tempEnemyForCloning.Hp, tempEnemyForCloning.Atk, tempEnemyForCloning.Spd, tempEnemyForCloning.Rarity);

        playerCharacter.Atk -= potionStatIncreases[1];
        playerCharacter.Spd -= potionStatIncreases[2];

        potionStatIncreases[0] = 0;
        potionStatIncreases[1] = 0;
        potionStatIncreases[2] = 0;

        RstConsole();
        if (usedItem)
        {
            Console.WriteLine("Your stat increases from previously used items fade away...");
            usedItem = false;
        }

        Console.WriteLine($"A {currentEnemy.Name} appeared!");
        Console.WriteLine("");
        EnemyEncounter();
    }

    private static void RstConsole()
    {
        Console.Clear();
        Console.WriteLine($"{playerCharacter.Name} || HP: {playerCharacter.Hp}/{allCharacterListImported[originalPlayerIndex].Hp} || ATK:{playerCharacter.Atk} || SPD:{playerCharacter.Atk}");
        Console.WriteLine("  ");
    }

    private static void EnemyEncounter()
    {
        // Turn reset
        inBlock = false;

        bool stupidChecker = true;
        while (stupidChecker)
        {
            // Action log
            Console.WriteLine($"The {currentEnemy.Name} stands idly");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine($"\t[1] Attack");
            Console.WriteLine($"\t[2] Use an item");
            Console.WriteLine($"\t[3] Block");
            Console.WriteLine("");

            try
            {
                int battleInputInt = Convert.ToInt32(Console.ReadLine());

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
                        EnemyTurn();
                        break;
                    default:
                        throw (new Exception("RangeError"));
                }
            }
            catch (Exception)
            {
                RstConsole();
                Console.WriteLine("Incorrect input!");
            }
        }
    }

    private static void RollEnemyLootPool()
    {
        Console.WriteLine(" ---- ");
        Console.WriteLine($"You search the {currentEnemy.Name}'s corpse for loot...");
        Console.WriteLine($"You get a Potion of Strength!");
        playerCharacter.Items.Add(new Item(1, "Potion of Strength", "Atk", 5));
        WaitForInput();
        RstConsole();
        currentDifficulty++;
        EncounterEnemyRoll();
    }

    private static void EnemyTurn()
    {
        int damage = 0;
        bool dodged = false;
        Random rnd = new Random();
        int hitMark = rnd.Next(0, 100);

        if (hitMark <= playerCharacter.Spd)
        {
            dodged = true;
        }
        else
        {
            if (inBlock)
            {
                damage = (int)Math.Floor(currentEnemy.Atk * 0.25);
            }
            else
            {
                damage = currentEnemy.Atk;
            }
            playerCharacter.Hp -= damage;
        }

        // Writing
        RstConsole();
        Console.WriteLine($"The {currentEnemy.Name} attacks!");

        if (dodged)
        {
            Console.WriteLine("You dodged the attack!");
        }
        else
        {
            Console.WriteLine(inBlock ? ("You partially blocked the attack!") : ("You got hit!"));
            Console.WriteLine($"You take {damage} damage!");
        }

        WaitForInput();
        RstConsole();
        EnemyEncounter();
    }

    private static void WaitForInput()
    {
        Console.WriteLine("Press enter to continute");
        Console.ReadLine();
    }

    private static void ItemMenu()
    {
        RstConsole();

        bool stupidInputChecker = true;
        while (stupidInputChecker)
        {
            try
            {
                Console.WriteLine("Your items:");
                if (playerCharacter.Items.Count == 0)
                {
                    Console.WriteLine("\tYou have no items!");
                    WaitForInput();
                    RstConsole();
                    EnemyEncounter();
                }
                else
                {
                    foreach (var item in playerCharacter.Items)
                    {
                        Console.WriteLine($"\t{item.Name} - {item} - {item.Stat} - {item.ValueIncrease}");
                    }
                    Console.WriteLine("");
                }

                Console.WriteLine($"Choose an item to use from 1 to {playerCharacter.Items.Count} \nInput 0 to Cancel");

                int itemInputInt = Convert.ToInt32(Console.ReadLine());
                if (itemInputInt == 0)
                {
                    RstConsole();
                    EnemyEncounter();
                }

                Item chosenItem = playerCharacter.Items[itemInputInt - 1];
                usedItem = true;
                switch (chosenItem.Stat)
                {
                    case "Hp":
                        playerCharacter.Hp += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and restored {chosenItem.ValueIncrease} HP!");
                        potionStatIncreases[0] += chosenItem.ValueIncrease;
                        break;
                    case "Atk":
                        playerCharacter.Atk += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and increased your ATK by {chosenItem.ValueIncrease}!");
                        potionStatIncreases[1] += chosenItem.ValueIncrease;
                        break;
                    case "Spd":
                        playerCharacter.Spd += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and increased your SPD by {chosenItem.ValueIncrease}!");
                        potionStatIncreases[3] += chosenItem.ValueIncrease;
                        break;
                    default:
                        throw (new Exception("No Such Stat"));
                }

                playerCharacter.Items.Remove(chosenItem);
                playerCharacter.Items.Sort((x, y) => x.Id.CompareTo(y.Id));

                //Good
                stupidInputChecker = false;
                WaitForInput();
                RstConsole();
                EnemyTurn();
            }
            catch (Exception)
            {
                RstConsole();
                Console.WriteLine("Incorrect input!");
            }
        }
    }

    private static void AttackEnemy()
    {
        RstConsole();
        Console.WriteLine(currentEnemy.Hp);
        Console.WriteLine(allEnemiesListImported[0].Hp);
        Random rnd = new Random();
        int hitMark = rnd.Next(0, 100);

        if (hitMark <= currentEnemy.Spd)
        {
            // Missed
            Console.WriteLine($"You attack the {currentEnemy.Name} but miss!");
        }
        else
        {
            Console.WriteLine($"You attack the {currentEnemy.Name} and hit for {playerCharacter.Atk} damage!");
            currentEnemy.Hp -= playerCharacter.Atk;
        }
        Console.WriteLine(currentEnemy.Hp);

        if (currentEnemy.Hp <= 0)
        {
            Console.WriteLine($"The {currentEnemy.Name} died!");
            RollEnemyLootPool();
        }

        WaitForInput();
        RstConsole();
        EnemyTurn();
    }
}