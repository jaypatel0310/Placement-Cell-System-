PR9 - PLACEMENT CELL DASHBOARD
ASP.NET Web Application (.NET Framework 4.8) using Web Forms + SQL Server + Data Controls + Crystal Reports
=====================================================================================================

IMPORTANT - HOW "OPEN IN BROWSER" WORKS
  .aspx pages are NOT normal web pages. Double-clicking them (or any .html preview) only shows plain text or
  a picture. They run through Visual Studio's built-in web server (IIS Express): you press F5 / Ctrl+F5 and
  Visual Studio opens Chrome/Edge for you at http://localhost:xxxxx/ . That IS "opening in the browser".

WHAT IS INSIDE
  PlacementDashboard.sln                  open THIS file
  PlacementDashboard\                     the web application project
     Site.Master                          common layout + menu (Master Page)
     Default.aspx                         Dashboard   : FormView (cards), Repeater (dept bars, company boxes), GridView
     Students.aspx                        Students    : GridView (edit/delete/paging) + DetailsView (insert) + DropDownList filter
     Companies.aspx                       Companies   : GridView (edit/delete) + DetailsView (insert)
     Placements.aspx                      Placements  : DetailsView (insert with dropdowns + date picker) + GridView (sort/page/delete)
     Report.aspx                          Printable report with filters (works with no extra install)
     Code\DbInit.cs                       creates the database, tables, view and sample data AUTOMATICALLY
     Code\BasePage.cs                     connects every page to the database
     Web.config                           connection string
  Database\PlacementDB.sql                OPTIONAL manual script (deletes and recreates the database)
  CrystalReports_Setup\                   Crystal Reports page (use after Part B)

PART A - RUN THE DASHBOARD  (about 10 minutes, no database work needed)
-----------------------------------------------------------------------
Step 1. Install (one time)
        a) Visual Studio 2019 or 2022 (Community is free). In the installer tick
           "ASP.NET and web development"  AND  ".NET Framework 4.8 targeting pack/development tools"
           AND "Data storage and processing" (this gives you LocalDB).
        b) Optional: SQL Server Management Studio (SSMS) to look inside the database.
Step 2. Unzip PR9_PlacementDashboard_WebApp.zip to a normal folder, e.g. D:\PR9_PlacementDashboard_WebApp
        (do not run it from inside the zip).
Step 3. Double-click PlacementDashboard.sln  (Visual Studio opens).
        If it asks about "retarget"/"upgrade" -> choose OK / do not retarget (keep .NET Framework 4.8).
Step 4. In Solution Explorer right-click "Default.aspx" -> "Set As Start Page".
        Press Ctrl+F5 (run without debugging) or F5. Visual Studio builds the project and opens your browser.
        The FIRST load takes 10-30 seconds: the site finds SQL Server, creates database PlacementDB,
        the tables, the view and the sample data. Next loads are instant.
Step 5. Red box "Database connection problem"?  Open Web.config and change Data Source to your server name.
        Your server name is shown in the SSMS "Connect to Server" window (e.g. DESKTOP-ABC\SQLEXPRESS).
        Save, refresh the browser.
Step 6. CHECK THE OUTPUT. The Dashboard must show:
        Total Students 10 | Students Placed 7 | Placement Rate 70% | Companies 6 | Highest 8.00 LPA | Average 5.59 LPA
        Department bars: Computer 100%, Electronics 100%, IT 33%, Mechanical 0%
        6 company boxes and "Latest 5 Placements" with dates like 12-Sep-2026.
        Students page: 10 rows. Companies page: 6 rows. Placements page: 12 rows (8 per page, 2 pages).
Step 7. Try the data controls: filter students by department, click Edit/Delete, add a student/company,
        add a placement (pick student + company, choose date) -> Save -> see it in the table and on the Dashboard.
Step 8. Open the "Report" menu -> filter -> "Print / Save as PDF".

CHECK THE DATA INSIDE SQL SERVER (optional)
  Visual Studio: View -> SQL Server Object Explorer -> (localdb)\MSSQLLocalDB (or your server) -> Databases ->
  PlacementDB -> Tables -> right-click dbo.Placements -> View Data. You should see 12 rows.
  Or in SSMS run:  SELECT * FROM vw_PlacementReport;

PART B - CRYSTAL REPORTS  (the .rpt file must be designed by you in Visual Studio, about 10 minutes)
---------------------------------------------------------------------------------------------------
B1. Install (one time), then restart Visual Studio:
    - "SAP Crystal Reports, developer version for Microsoft Visual Studio"   (free, from SAP's website)
    - "SAP Crystal Reports runtime for .NET Framework" - choose the 64-bit one.
    - VS 2022 only: Tools -> Options -> Projects and Solutions -> Web Projects ->
      tick "Use the 64 bit version of IIS Express for web sites and projects".
B2. Design the report
    1. Solution Explorer: right-click the folder "Reports" -> Add -> New Item -> "Crystal Report" -> name it
       PlacementReport.rpt -> Add.   (If VS asks to add Crystal references/web.config entries, say Yes.)
    2. "Using the Report Wizard" -> "Standard" -> OK.
    3. Data: Create New Connection -> OLE DB (ADO) -> "Microsoft OLE DB Driver for SQL Server"
       (or "Microsoft OLE DB Provider for SQL Server") -> Next. Server = your server name
       (same as in Web.config, e.g. (localdb)\MSSQLLocalDB), tick Integrated Security, Database = PlacementDB -> Finish.
    4. Expand PlacementDB -> dbo -> Views -> double-click vw_PlacementReport -> Next.
    5. Fields: FullName, Department, CGPA, CompanyName, Role, PackageLPA, PlacementDate, Status -> Next.
    6. Group by: Department -> Next -> (summary Sum of PackageLPA; you may change it to Average) -> Finish.
    7. Right-click Report Header -> Insert -> Text Object -> type "Placement Cell Report". Save.
B3. Show it in the browser
    1. Right-click the PlacementDashboard project -> Add -> Existing Item -> go to CrystalReports_Setup, select ALL THREE
       files (CrystalReport.aspx, CrystalReport.aspx.cs, CrystalReport.aspx.designer.cs) -> Add.
    2. Ctrl+F5. A "Crystal Report" link now appears in the menu. Click it.
    3. Error on the <%@ Register ... %> line? Delete that line and the <CR:CrystalReportViewer> tag, drag
       "CrystalReportViewer" from the Toolbox onto the page, name it CrystalReportViewer1.

TROUBLESHOOTING
  Red "Database connection problem"           -> fix Data Source in Web.config (Step 5); make sure the SQL Server service runs.
  "Cannot open database / login failed"       -> SSMS -> Security -> Logins -> add your Windows user (role: sysadmin).
  Build error about .NET Framework 4.8        -> install the ".NET Framework 4.8 targeting pack" (Visual Studio Installer).
  Port/IIS Express message                    -> click Yes / OK.
  Crystal: "Could not load file or assembly"  -> runtime missing or 32/64-bit mismatch (B1).
  Want fresh sample data                      -> run Database\PlacementDB.sql in SSMS (it deletes and recreates PlacementDB).
