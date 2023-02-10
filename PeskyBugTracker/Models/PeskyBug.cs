using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeskyBugTracker.Models
{
    public class PeskyBug
    {

        public enum eSeverity: int
        {
            Subterranean=0,
            Low=1,
            Medium=2,
            High=3,
            Stratospheric=4
        }

        public enum eState:int
        {
            New=0,
            Closed=1,
            Assigned=2,
            Investigated=3,
            FixApplied=4,
            AwaitingDeployment=5,
            MoreInfoRequested=6,
            Cancelled=7,
            Duplicate=8,
        }
        
        public PeskyBug()
        {
            Notes = new List<NoteEntry>();
            State = eState.New;

        }


        public PeskyBug(string id, string title, string description, string? raisedByName, string raisedBy)  
        {
            ID = Guid.Parse(id);
            Title = title;
            Description = description;
            RaisedByName = raisedByName;
            RaisedBy = Guid.Parse(raisedBy);
            RaisedAt = DateTime.Now;
            this.severity = eSeverity.Subterranean;
            State = eState.New;
            Notes = new List<NoteEntry>();
            
        }

        public Guid ID { get; set; } 
        
        public string Title{ get; set; }
        
        public string Description { get; set; }

        [DisplayName("Raised By")]
        [NotMapped]
        public string? RaisedByName { get; set; }


        public Guid? RaisedBy { get; set;}

        [DisplayName("Created")]
        public DateTime RaisedAt { get; set;}

        private eSeverity? severity;

        public eSeverity? GetSeverity()
        {
            return severity;
        }

        public void SetSeverity(eSeverity? value)
        {
            severity = value;
        }

        [DefaultValue(eState.New)]
        public eState? State { get; set; }

        [DisplayName("Duplicate Id")]
        public Guid? DuplicateId { get; set; }

        [DisplayName("Assigned To")]
        [DefaultValue(00000000-0000-0000-0000-000000000000)]
        public Guid? AssignedTo { get; set; }

        [DisplayName("Assigned To")]
        [NotMapped]
        public string? AssignedToName { get; set; }

        public List<NoteEntry> Notes { get; set; }


    }

    public class NewNote {
        public Guid ID { get; set; }
        public string NoteText { get; set; }

        public NewNote()
        {

        }
    }
}
