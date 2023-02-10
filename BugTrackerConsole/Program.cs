using PeskyBugTracker;
using BugData;
using PeskyBugTracker.Models;

Console.WriteLine("Hello, World!");

Console.WriteLine("This routine will build the Database and tables based on the Model Classes.");


//using BugDataContext;


using (BugContext context= new BugContext())
{
    context.Database.EnsureCreated();

    context.Organisations.Add(new PeskyBugTracker.Models.Organisation("7A3EB554-A508-4B8F-9205-1636D313A1F0", "Acme Inc", "Cartoon Supplies"));
    context.SaveChanges();
    Organisation organisation = context.Organisations.First();    

    context.Agents.Add(new PeskyBugTracker.Models.Agent(
                        Guid.Parse("BD23C971-748A-43B1-84B3-39BF13D01E60"),
                        "System", "User",
                        "SystemUser",
                        "7646DF52-2634-459D-96FF-A82605B76C2B",
                        false, false, false,
                        organisation.Id
                        ));

    context.Agents.Add(new PeskyBugTracker.Models.Agent(
                        Guid.Parse("6F331EE1-927A-47F2-97B4-7829981CC110"),
                        "Peter", "Gabriel",
                        "peter",
                        "password",
                        true,true,true,
                        organisation.Id
                        ));

    context.Agents.Add(new PeskyBugTracker.Models.Agent(
                        Guid.Parse("EF472892-A64B-4FF5-A74F-7CC6A6029845"),
                        "Steve", "Hackett",
                        "steve",
                        "password",
                        true, false, true,
                        organisation.Id
                        ));

    context.Agents.Add(new PeskyBugTracker.Models.Agent(
                        Guid.Parse("D4AF3BED-FECF-4408-915A-3BCFA61770E7"),
                        "Tony", "Banks",
                        "tony",
                        "password",
                        false, true, true,
                        organisation.Id
                        ));

    context.PeskyBugs.Add(new PeskyBug("A248FEA8-E67E-4F89-B8E7-4A3834D506E9", "Page X odd behaviour (Test Bug)", "On page X the background keeps playing Prog Rock Videos", "", "BD23C971-748A-43B1-84B3-39BF13D01E60"));
        //A248FEA8 - E67E - 4F89 - B8E7 - 4A3834D506E9

    context.SaveChanges();
}
