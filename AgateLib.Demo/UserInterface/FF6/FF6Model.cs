using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6Model
    {
        List<EquipmentSlot> slots = new List<EquipmentSlot>
        {
            new EquipmentSlot("R-Hand", "weapon"),
            new EquipmentSlot("L-Hand", "shield"),
            new EquipmentSlot("Head", "helm"),
            new EquipmentSlot("Body", "armor"),
            new EquipmentSlot("Relic 1", "relic"),
            new EquipmentSlot("Relic 2", "relic"),
        };
        private List<Item> _inventory = new List<Item>();

        public FF6Model()
        {
            Party = new Party(4, slots.Select(x => x.Name).ToArray());
        }

        public IEnumerable<EquipmentSlot> EquipmentSlots => slots;

        public List<Item> Inventory
        {
            get => _inventory;
            set => _inventory = value ?? throw new ArgumentNullException(nameof(Inventory));
        }

        public Party Party { get; set; }

        public Item FindInInventory(string name)
        {
            return Inventory.FirstOrDefault(
                x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void EquipPC(PlayerCharacter pc, Item item)
        {
            var validSlots = GetValidSlots(item);

            if (!validSlots.Any())
                throw new InvalidOperationException($"No slot found for item {item.Name}");

            var firstEmpty = validSlots.FirstOrDefault(slot => pc.Equipment[slot.Name] == null);

            var first = validSlots.First();

            var dest = firstEmpty ?? first;

            EquipPC(pc, item, dest.Name);
        }

        public IEnumerable<Item> ItemsForSlot(PlayerCharacter pc, string slot)
        {
            foreach (var item in Inventory)
            {
                if (GetValidSlots(item).Any(x => x.Name == slot))
                    yield return item;
            }
        }

        private IEnumerable<EquipmentSlot> GetValidSlots(Item item)
        {
            return slots.Where(x => x.AllowedItemTypes.Contains(item.ItemType, StringComparer.OrdinalIgnoreCase));
        }

        public void EquipPC(PlayerCharacter pc, Item item, string slotName)
        {
            EquipmentSlot slot;

            if (item != null)
            {
                var validSlots = GetValidSlots(item);
                slot = validSlots.FirstOrDefault(
                    x => x.Name.Equals(slotName, StringComparison.OrdinalIgnoreCase))
                    ?? throw new InvalidOperationException($"Item {item.Name} is of type {item.ItemType} and cannot be equipped in slot {slotName}");

                Inventory.Remove(item);
            }
            else
            {
                slot = EquipmentSlots.FirstOrDefault(x => x.Name.Equals(slotName, StringComparison.OrdinalIgnoreCase))
                    ?? throw new ArgumentException($"Slot {slotName} does not exist.");
            }

            var existing = pc.Equipment[slot.Name];

            pc.Equipment[slot.Name] = item;

            if (existing != null)
                Inventory.Add(existing);
        }
    }

    public class Item
    {
        public string Name { get; set; }

        public string ItemType { get; set; }

        public string Effect { get; set; }

        public int EffectAmount { get; set; }
    }

    public class Party
    {
        private List<PlayerCharacter> characters = new List<PlayerCharacter>();
        private string[] equipmentSlots;

        public Party(int maxPartySize, params string[] equipmentSlots)
        {
            MaxPartySize = maxPartySize;
            this.equipmentSlots = equipmentSlots;
        }

        public int MaxPartySize { get; }

        public IEnumerable<PlayerCharacter> Characters => characters;

        public void Clear() => characters.Clear();

        public void Add(PlayerCharacter playerCharacter)
        {
            if (characters.Count >= MaxPartySize)
                throw new InvalidOperationException($"Cannot exceed max party size of {MaxPartySize}.");

            characters.Add(playerCharacter);

            foreach (var slot in equipmentSlots)
            {
                playerCharacter.Equipment[slot] = null;
            }
        }

        public PlayerCharacter Find(string pcName)
        {
            return characters.FirstOrDefault(
                x => x.Name.Equals(pcName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class PlayerCharacter
    {
        public PlayerCharacter(string name, int maxHP)
        {
            Name = name;
            MaxHP = maxHP;
            HP = maxHP;
        }

        public string Name { get; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public List<MagicSpell> Magic { get; } = new List<MagicSpell>();

        public Dictionary<string, Item> Equipment { get; }
            = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
    }

    public class MagicSpell
    {
        public MagicSpell(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class EquipmentSlot
    {
        public EquipmentSlot(string name, params string[] allowedItemTypes)
        {
            Name = name;
            AllowedItemTypes.AddRange(allowedItemTypes);
        }

        public string Name { get; set; }

        public List<string> AllowedItemTypes { get; } = new List<string>();
    }
}
