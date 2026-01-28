using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Enemy
    {
        private int id {  get; set; }
        private string name { get; set; }
        private int hp { get; set; }
        private int atk { get; set; }
        private int spd { get; set; }
        private int rarity { get; set; }
        private Dictionary<int, int>[] lootPool { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Spd { get; set; }
        public int Rarity { get; set; }
        public Dictionary<int, int>[] LootPool { get; set; }


        public Enemy(int id, string name, int hp, int atk, int spd, int rarity , Dictionary<int, int>[] lootPool)
        {
            Id = id;
            Name = name;
            Hp = hp;
            Atk = atk;
            Spd = spd;
            Rarity = rarity;
            LootPool = lootPool;
        }
    }
}
