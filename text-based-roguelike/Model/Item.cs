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

        private int id { get; set; }
        private string name { get; set; }
        private string stat { get; set; }
        private int valueIncrease { get; set; }

        public Item(int id, string name, string stat, int valueIncrease)
        {
            this.id = id;
            this.name = name;
            this.stat = stat;
            this.valueIncrease = valueIncrease;
        }
    }
}
