using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PeskyBugTracker.Models
{
    public class NoteEntry
    {

        public NoteEntry(string noteText, Guid agentID)
        {
            //var agentID = "";


            Id =Guid.NewGuid();
            NoteText = noteText;
            TimeStamp= DateTime.Now;
            AgentID = agentID;
            
        }


        public Guid Id { get; set; }

        [DisplayName("Note")]
        public string NoteText { get; set; }

        [DisplayName("Note Added")]
        public DateTime TimeStamp { get; set;}

        //[DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Agent? AgentAuthor { get; set; }

        public Guid AgentID { get; set; }

        public Guid PeskyBugID { get; set; }

        public string NoteToString()
        {
         
            return TimeStamp.ToString() + " - " + NoteText;
        }

        


    }
}
