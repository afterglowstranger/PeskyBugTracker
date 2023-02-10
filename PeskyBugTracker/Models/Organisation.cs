using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeskyBugTracker.Models
{
    public class Organisation
    {


        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        //public Agent PrimaryContact { get; set; }


        public Organisation() {
            Name = "";
        }

        public Organisation(Guid id)
        {
            Id = id;
        }

        public Organisation(string id, string name, string description)
        {
            Id = Guid.Parse(id);
            Name = name;
            Description= description;   

        }
    }
}
