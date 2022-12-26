namespace Signals.App.Database.Entities.Blocks
{
    public class GroupBlockEntity : BlockEntity
    {
        ///TODO: Make just Type
        public GroupBlockType GroupType { get; set; }

        public enum GroupBlockType
        {
            And,
            Or
        }
    }
}
