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
    private static int originalPlayerIndex;
    private static int currentDifficulty = 1;
    private static bool inBlock;
    private static bool usedItem;
    private static bool needSpeedResetFromBlock = false;
    private static Enemy currentEnemy;
    private static List<Character> allCharacterListImported = new List<Character>();
    private static List<Enemy> allEnemiesListImported = new List<Enemy>();
    private static List<Weapon> allWeaponsListImported = new List<Weapon>();
    private static List<Item> allItemsListImported = new List<Item>();
    private static List<LootPool> allEnemyLootPoolListImported = new List<LootPool>();
    private static int[] potionStatIncreases = [ 0, 0, 0 ];
    private static bool difficultyFlipFlop = false;
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
            Character tempChar = new Character(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), new List<Item>());
            allCharacterListImported.Add(tempChar);
        }

        foreach (DataRow r in EnemiesDBT.Rows)
        {
            Enemy tempChar = new Enemy(r.Field<int>("id"), r.Field<string>("name"), r.Field<int>("hp"), r.Field<int>("atk"), r.Field<int>("spd"), r.Field<int>("rarity"));
            allEnemiesListImported.Add(tempChar);
        }

        foreach (DataRow r in ItemsDBT.Rows)
        {
            Item tempChar = new Item(r.Field<int>("id"), r.Field<string>("name"), r.Field<string>("stat"), r.Field<int>("inc"));
            allItemsListImported.Add(tempChar);
        }

        foreach (DataRow r in EnemyLootPoolDBT.Rows)
        {
            LootPool tempLootPool = new LootPool(r.Field<int>("id"), r.Field<int>("enemy_rarity"), r.Field<int>("item_and_weapon_id"), r.Field<int>("drop_chance"));
            allEnemyLootPoolListImported.Add(tempLootPool);
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
        List<Enemy> currentDifficultyEnemies = allEnemiesListImported.FindAll(x => x.Rarity == currentDifficulty).ToList();
        Random rnd = new Random();
        Enemy tempEnemyForCloning = currentDifficultyEnemies[rnd.Next(0, currentDifficultyEnemies.Count - 1)];
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

        Console.WriteLine("");
        Console.WriteLine($"A {currentEnemy.Name} appeared!");
        Console.WriteLine("");
        EnemyEncounter();
    }

    private static void RstConsole()
    {
        Console.Clear();
        Console.WriteLine($"{playerCharacter.Name} || HP: {playerCharacter.Hp}/{allCharacterListImported[originalPlayerIndex].Hp} || ATK:{playerCharacter.Atk} || SPD:{playerCharacter.Spd}");
        Console.WriteLine("");
    }

    private static void EnemyEncounter()
    {
        // Turn reset
        if (!inBlock && needSpeedResetFromBlock)
        {
            playerCharacter.Spd -= 50;
            needSpeedResetFromBlock = false;
        }
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
        if (currentDifficulty > 5)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("");
            Console.WriteLine("\tYOU WIN!!!!");
            Console.WriteLine("");
            Console.WriteLine("-------------------------");

            WinOrLoss(true);

            WaitForInput();
            Environment.Exit(0);
        }

        Console.WriteLine(" ---- ");
        Console.WriteLine($"You search the {currentEnemy.Name}'s corpse for loot...");

        // Lootpool roll
        int gottenItemId = RollForItemId();
        if (gottenItemId != 0)
        {
            if (gottenItemId < 100)
            {
                Item gottenItem = allItemsListImported.Find(x => x.Id == gottenItemId);
                playerCharacter.Items.Add(gottenItem);
                playerCharacter.Items.Sort((x, y) => x.Id.CompareTo(y.Id));
                Console.WriteLine($"You found a {gottenItem.ToString()}!");
            }
            else
            {
                Weapon gottenWeapon = allWeaponsListImported.Find(x => x.Id == gottenItemId);
                playerCharacter.Atk += gottenWeapon.Atk;
                Console.WriteLine($"You found a {gottenWeapon.ToString()} and equip it, increasing your ATK by {gottenWeapon.Atk}!");
            }
        }
        else
        {
            Console.WriteLine("You found nothing...");
        }

        WaitForInput();
        RstConsole();

        // Increase difficulty every 2 encounters
        if (!difficultyFlipFlop)
        {
            difficultyFlipFlop = true;
        }
        else
        {
            currentDifficulty++;
            difficultyFlipFlop = false;
        }

        EncounterEnemyRoll();
    }

    private static void WinOrLoss(bool winBool)
    {
        List<string> writtenItems = new List<string>();

        writtenItems.Add(winBool ? "Sucessful run!" : "Run FAILED!");
        writtenItems.Add(" //// ");
        writtenItems.Add("Character stats:");
        writtenItems.Add(playerCharacter.Name);
        writtenItems.Add(winBool ? $"\tRemaining HP: {playerCharacter.Hp}" : $"\tStarting HP: {allCharacterListImported[originalPlayerIndex].Hp}");
        writtenItems.Add($"\tATK: { playerCharacter.Atk}");
        writtenItems.Add($"\tSPD: { playerCharacter.Spd}");

        writtenItems.Add($"Item List:");
        foreach (var item in playerCharacter.Items)
        {
            writtenItems.Add($"\t{item.Name}");
        }

        writtenItems.Add($"  ");
        writtenItems.Add($" ---------- ");
        writtenItems.Add($"  ");

        SaveRunInFile(writtenItems);
    }

    private static void SaveRunInFile(List<string> writtenItems)
    {
        try
        {
            FileIO.WriteToFile writer = new FileIO.WriteToFile();
            writer.FileWrite("Past_Runs.txt", writtenItems);
        }
        catch (IOException)
        {
            Console.WriteLine("Failed to write to file :c");
        }
    }

    private static int RollForItemId()
    {
        List<LootPool> currentEnemyLootPool = allEnemyLootPoolListImported.FindAll(x => x.Rarity == currentEnemy.Rarity);
        Random rnd = new Random();
        int poolSize = 0;
        foreach (var item in currentEnemyLootPool)
        {
            poolSize += item.DropChance;
        }
        int poolRoll = rnd.Next(0, poolSize);

        int rollingThroughPool = 0;
        foreach (var item in currentEnemyLootPool)
        {
            rollingThroughPool += item.DropChance;
            if (poolRoll <= rollingThroughPool)
            {
                return item.ItemOrWeaponId;
            }
        }
        return 0;
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
                if(!needSpeedResetFromBlock)
                {
                    playerCharacter.Spd += 50;
                    needSpeedResetFromBlock = true;
                }
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
            Console.WriteLine(inBlock ? ("You partially blocked the attack! \nYou predicted your enemy's next move, you have a higher chance to dodge next turn!") : ("You got hit!"));
            Console.WriteLine($"You take {damage} damage!");
        }

        if(playerCharacter.Hp <= 0)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("");
            Console.WriteLine("\tYOU DIED!");
            Console.WriteLine("");
            Console.WriteLine("-------------------------");

            WinOrLoss(false);

            WaitForInput();
            Environment.Exit(0);
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
                        Console.WriteLine($"\t{item.Name}");
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
                    case "hp":
                        playerCharacter.Hp += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and restored {chosenItem.ValueIncrease} HP!");
                        potionStatIncreases[0] += chosenItem.ValueIncrease;
                        break;
                    case "atk":
                        playerCharacter.Atk += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and increased your ATK by {chosenItem.ValueIncrease}!");
                        potionStatIncreases[1] += chosenItem.ValueIncrease;
                        break;
                    case "spd":
                        playerCharacter.Spd += chosenItem.ValueIncrease;
                        Console.WriteLine($"You used {chosenItem.Name} and increased your SPD by {chosenItem.ValueIncrease}!");
                        potionStatIncreases[2] += chosenItem.ValueIncrease;
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
            catch (Exception ex)
            {
                RstConsole();
                Console.WriteLine("Incorrect input!");
                Console.WriteLine(ex);
            }
        }
    }

    private static void AttackEnemy()
    {
        RstConsole();

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