using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeskyBugTracker.Models
{
    public class Agent : PageModel
    {

        public Agent() { }

        public Agent(Guid id, string forename, string surnameName, string userName, string password, bool canRaiseBugs, bool canFixBugs, bool canLogin, Guid organisation)
        {
            Id = id;
            Forename = forename;
            SurnameName = surnameName;
            UserName = userName;
            Password = password;
            CanRaiseBugs = canRaiseBugs;
            CanFixBugs = canFixBugs;
            CanLogin = canLogin;
            OrganisationId =  organisation;    
        }

        public Guid Id { get; set; }
        public string Forename { get; set; }
        public string SurnameName { get; set; }


        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }    

        [DefaultValue(false)]
        public bool CanRaiseBugs { get; set;}

        [DefaultValue(false)]
        public bool CanFixBugs { get; set; }

        [DefaultValue(false)]
        public bool CanLogin { get; set; }

        public Guid OrganisationId { get; set; }    

        public string FullName()
        {
                return Forename + " " + SurnameName;
        }
    }

    public class selectAgent
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }

        public selectAgent(Guid id, string fullname)
        {
            Id = id;
            Fullname = fullname;
        }
    }
}
