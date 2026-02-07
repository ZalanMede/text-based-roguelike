using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Model
{
    internal class LootPool
    {
        public LootPool(int id, int rarity, int itemOrWeaponId, int dropChance)
        {
            Id = id;
            Rarity = rarity;
            ItemOrWeaponId = itemOrWeaponId;
            DropChance = dropChance;
        }

        private int _id { get; set; }
        private int _rarity { get; set; }
        private int _itemOrWeaponId { get; set; }
        private int _dropChance { get; set; }

        public int Id { get; set; }
        public int Rarity { get; set; }
        public int ItemOrWeaponId { get; set; }
        public int DropChance { get; set; }
    }
}
