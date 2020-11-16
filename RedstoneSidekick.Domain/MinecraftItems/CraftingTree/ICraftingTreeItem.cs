namespace RedstoneSidekick.Domain.MinecraftItems
{
    public interface ICraftingTreeItem : IMinecraftItem
    {
        public int Count { get; set; }

        public bool IsChecked { get; set; }

    }
}