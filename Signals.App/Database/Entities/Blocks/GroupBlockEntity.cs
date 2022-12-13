namespace Signals.App.Database.Entities.Blocks
{
    public class GroupBlockEntity : BlockEntity
    {
        public GroupBlockType GroupType { get; set; }

        public enum GroupBlockType
        {
            And,
            Or
        }
    }
}
