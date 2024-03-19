namespace Toolaccounting.Server.Models
{
    public class ToolUsed
    {
        public uint Id { get; set; }
        public uint ToolId { get; set; }
        public Tool Tool { get; set; }
        public uint UserId { get; set; }
        public User User { get; set; }
        public uint ToolCount { get; set; }

        public override string ToString()
        {
            return $"Id:{Id} UserId:{UserId}  ToolId:{ToolId} ToolCount:{ToolCount}";
        }




    }
}
