using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Enemy
    {
        private int _id {  get; set; }
        private string _name { get; set; }
        private int _hp { get; set; }
        private int _atk { get; set; }
        private int _spd { get; set; }
        private int _rarity { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Spd { get; set; }
        public int Rarity { get; set; }

        public Enemy(int id, string name, int hp, int atk, int spd, int rarity)
        {
            Id = id;
            Name = name;
            Hp = hp;
            Atk = atk;
            Spd = spd;
            Rarity = rarity;
        }
    }
}
