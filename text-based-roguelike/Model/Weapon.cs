using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Weapon
    {
        private int id {  get; set; }
        private string name { get; set; }
        private string atk { get; set; }

        public Weapon(int id, string name, string atk)
        {
            Id = id;
            Name = name;
            Atk = atk;
        }

        public int Id {  get; set; }
        public string Name { get; set; }
        public string Atk { get; set; }
    }
}
