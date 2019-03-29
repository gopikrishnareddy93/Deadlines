using System;

namespace Deadlines.Data.Context.Models
{
    public partial class Deadlines
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
