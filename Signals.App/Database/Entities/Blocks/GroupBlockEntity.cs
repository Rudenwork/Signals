using System.ComponentModel.DataAnnotations.Schema;

namespace Signals.App.Database.Entities.Blocks
{
    public class GroupBlockEntity : BlockEntity
    {
        [Column(TypeName = "varchar")]
        public GroupBlockType Type { get; set; }

        //EF Injected
        public List<BlockEntity> Children { get; set; }
    }

    public enum GroupBlockType
    {
        And,
        Or
    }
}
