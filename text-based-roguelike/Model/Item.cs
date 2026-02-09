using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class Item
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Stat { get; set; }
        public int ValueIncrease { get; set; }

        private int _id { get; set; }
        private string _name { get; set; }
        private string _stat { get; set; }
        private int _valueIncrease { get; set; }

        public Item(int id, string name, string stat, int valueIncrease)
        {
            Id = id;
            Name = name;
            Stat = stat;
            ValueIncrease = valueIncrease;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
