using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Character
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Hp {  get; set; }
        public int Atk { get; set; }
        public int Spd { get; set; }
        public List<Item> Items { get; set; }
        public Character(int id, string name, int hp, int atk, int spd, List<Item> items)
        {
            Id = id;
            Name = name;
            Hp = hp;
            Atk = atk;
            Spd = spd;
            Items = items;
        }

        private int id { get; set; }
        private string name { get; set; }
        private int hp { get; set; }
        private int atk { get; set; }
        private int spd { get; set; }
        private List<Item> items { get; set; }

        public override string ToString()
        {
            return ($"\n{Name} \t- ATK: {Atk}\n\t - HP: {Hp}\n\t - SPD: {Spd}");
        }
    }
}
