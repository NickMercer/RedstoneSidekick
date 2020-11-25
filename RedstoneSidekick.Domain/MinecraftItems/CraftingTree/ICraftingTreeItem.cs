
namespace RedstoneSidekick.Domain.MinecraftItems
{
    public interface ICraftingTreeItem : IMinecraftItem
    {
        public int RequiredAmount { get; set; }

        public int CurrentAmount { get; set; }

        public bool IsChecked { get; set; }

    }
}