# PeskyBugTracker
A simple Bug Tracking App


Pesky Bug Tracker ReadMe

Overview
I have written a .Net MVC (Core) web application using a SQL Express database for storage.  Whilst I have included functionaility to add, edit and close bugs and progress notes for bugs, the users(Agents) and organisations have no CRUD interface.

User can be configured to be allowed to raise bugs, have bugs assigned to them and login or not or any combination of the above, though obviously restricting their login will impact on their ability to raise or field bugs.  

There is a separate Console app which if set as the start project will create the database and seed an organsisation, four users and a bug.

There is a menu link to "Api Test Page" which details 3 end points;
      All Bugs
      All Open Bugs
      A Bug by id.

A simple Login and Logout mechanism has been included with the credentials below.

Tour Guide
The functionaility requested was: Your goal is to implement a simple bug tracker. The features we would like, in no particular order:
● It should be possible to view the list of open bugs, including their titles
      The default view once logged in is "Open Bugs".  There is a button to switch this view to "Closed Bugs".  And back!  The Bugs have numerous states to hopefully reflect real world bug states, these are {New, Closed, Assigned, Investigated, FixApplied, Awaiting Deployment, MoreInfoRequested, Cancelled, Duplicate}
● It should be possible to view the detail of individual bugs, including the title the full
description, and when it was opened.  Click the Details Button next to any Bug to see the detail and add progress notes.
● It should be possible to create bugs - click the "Create a New Bug" green button on the Bugs Listing screen.
● It should be possible to close bugs- Close a bug changes it's state, it doesn't delete it, as this provides audit trail, reporting etc.
● It should be possible to assign bugs to people-Yes via the Edit screen and Create New bug screen.
● It should be possible to add people to the system- Follow the Agents top bar menu item to list, create, edit and delete Agents (users)
● It should be possible to change people’s names- Follow the Agents top bar menu item to list, then edit alongside the chosen Agent
● The web application should look nice - no comment(!) I had to "acquire" the logo, don't ask too many questions ok?
● The web application should expose some sort of API- via the Api Test Page link or 
● The data should be stored in some sort of database - using MS SQL Server Express as a test bench DB.  The database is called "PeskyBugTracker"


SetUp

The MS SQL Express Database should (!) be installed as part of the extras with Visual Studio.  If you aren't using Visual Studio then the Connection string will need to be manually edited. 

It resides in "\PeskyBugTracker\PeskyBugTracker\appsettings.json" line 10
and
"\PeskyBugTracker\BugData\BugData.cs" line 21


To create the database and add some initial agents, a bug and organisation set the BugTrackerConsole as startup project. Leg this run, F5 and it should complete in a few seconds with the message "Press any key to close this window..."  Note it starts with the reasuringly familiar "Hello World"

Switch the start up project to "Pesky Bug Tracker" and F5 this.  You should be presented with a login screen.


Api End Points
https://localhost:7043/PeskyBugValues/GetAllBugs
https://localhost:7043/PeskyBugValues/GetAllOpenBugs
https://localhost:7043/PeskyBugValues/GetBug/4d450e15-b70d-4626-ba05-6f24586a01d8

Note the last one is using a pre added Bug, swap the GUID at the end of the url to another valid bug id to test further


Details

User credentials
peter, tony, steve
All have password, lower case "password" with no quotes.


Limitations and Future Enhancements
1. The handling of passwords isn't very smart, plain text etc.
2. Adding contact details to Agents, email, phone etc would be useful
