using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Weapon
    {
        private int _id {  get; set; }
        private string _name { get; set; }
        private int _atk { get; set; }

        public Weapon(int id, string name, int atk)
        {
            Id = id;
            Name = name;
            Atk = atk;
        }

        public int Id {  get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
