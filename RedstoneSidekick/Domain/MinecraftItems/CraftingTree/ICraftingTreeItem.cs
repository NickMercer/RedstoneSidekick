
namespace RedstoneSidekick.Domain.MinecraftItems.CraftingTree
{
    public interface ICraftingTreeItem
    {
        public MinecraftItem Item { get; set; }

        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public bool IsChecked { get; set; }

    }
}