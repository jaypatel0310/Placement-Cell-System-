using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Makes the site work with ZERO manual database steps:
///  1. finds a SQL Server that works (LocalDB, SQLEXPRESS, localhost),
///  2. creates the PlacementDB database, tables, view and sample data if they are missing,
///  3. points every SqlDataSource on the page to that working server.
/// Safe to run many times (it never deletes your data).
/// </summary>
namespace PlacementDashboard
{
public static class DbInit
{
    const string CsName = "PlacementCS";
    static readonly object Lock = new object();
    static bool ready;
    static string cs;

    public static string ConnectionString { get { return cs; } }

    /// <summary>Returns null when everything is OK, otherwise a readable error message.</summary>
    public static string Ensure(Control root)
    {
        lock (Lock)
        {
            if (!ready)
            {
                string err = Setup();
                if (err != null) return err;
            }
        }
        Apply(root);
        return null;
    }

    static string Setup()
    {
        try
        {
            var configured = ConfigurationManager.ConnectionStrings[CsName];
            var b = new SqlConnectionStringBuilder(configured.ConnectionString);
            string db = string.IsNullOrEmpty(b.InitialCatalog) ? "PlacementDB" : b.InitialCatalog;

            var candidates = new List<string>();
            candidates.Add(b.DataSource);
            foreach (string s in new string[] { @"(localdb)\MSSQLLocalDB", @".\SQLEXPRESS", @"localhost", @"(local)\SQLEXPRESS", @"127.0.0.1" })
            {
                bool dup = false;
                foreach (string c in candidates) if (string.Equals(c, s, StringComparison.OrdinalIgnoreCase)) dup = true;
                if (!dup) candidates.Add(s);
            }

            string working = null;
            var failures = new List<string>();
            foreach (string ds in candidates)
            {
                string why;
                if (CanConnect(b, ds, out why)) { working = ds; break; }
                failures.Add(ds + " => " + why);
            }
            if (working == null)
                return "No SQL Server could be reached. Install SQL Server Express (or use Visual Studio LocalDB) and make sure its service is running. Details: " + string.Join(" || ", failures.ToArray());

            // 1) create the database (connect to 'master')
            var m = new SqlConnectionStringBuilder(b.ConnectionString);
            m.DataSource = working; m.InitialCatalog = "master";
            using (var con = new SqlConnection(m.ConnectionString))
            {
                con.Open();
                Exec(con, "IF DB_ID(N'" + db.Replace("'", "''") + "') IS NULL CREATE DATABASE [" + db.Replace("]", "]]") + "]");
            }

            // 2) tables, data, view
            var t = new SqlConnectionStringBuilder(b.ConnectionString);
            t.DataSource = working; t.InitialCatalog = db;
            using (var con = new SqlConnection(t.ConnectionString))
            {
                con.Open();
                foreach (string batch in Batches) Exec(con, batch);
            }

            cs = t.ConnectionString;
            ready = true;
            return null;
        }
        catch (Exception ex) { return ex.Message; }
    }

    static bool CanConnect(SqlConnectionStringBuilder basis, string dataSource, out string why)
    {
        why = "";
        try
        {
            var t = new SqlConnectionStringBuilder(basis.ConnectionString);
            t.DataSource = dataSource; t.InitialCatalog = "master"; t.ConnectTimeout = 15;
            using (var con = new SqlConnection(t.ConnectionString)) { con.Open(); }
            return true;
        }
        catch (Exception ex) { why = ex.Message.Replace("\r", " ").Replace("\n", " "); return false; }
    }

    static void Exec(SqlConnection con, string sql)
    {
        using (var cmd = new SqlCommand(sql, con)) { cmd.ExecuteNonQuery(); }
    }

    static void Apply(Control c)
    {
        var sds = c as SqlDataSource;
        if (sds != null) sds.ConnectionString = cs;
        foreach (Control child in c.Controls) Apply(child);
    }

    static readonly string[] Batches = new string[]
    {
@"IF OBJECT_ID('dbo.Students','U') IS NULL
CREATE TABLE dbo.Students(
    StudentId  INT IDENTITY(1,1) PRIMARY KEY,
    FullName   VARCHAR(100) NOT NULL,
    Department VARCHAR(50)  NOT NULL,
    CGPA       DECIMAL(3,2) NOT NULL,
    Email      VARCHAR(100) NULL)",

@"IF OBJECT_ID('dbo.Companies','U') IS NULL
CREATE TABLE dbo.Companies(
    CompanyId   INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName VARCHAR(100) NOT NULL,
    Sector      VARCHAR(50)  NOT NULL,
    PackageLPA  DECIMAL(5,2) NOT NULL)",

@"IF OBJECT_ID('dbo.Placements','U') IS NULL
CREATE TABLE dbo.Placements(
    PlacementId   INT IDENTITY(1,1) PRIMARY KEY,
    StudentId     INT NOT NULL REFERENCES dbo.Students(StudentId) ON DELETE CASCADE,
    CompanyId     INT NOT NULL REFERENCES dbo.Companies(CompanyId) ON DELETE CASCADE,
    Role          VARCHAR(80) NOT NULL,
    PlacementDate DATE NOT NULL,
    Status        VARCHAR(20) NOT NULL CHECK (Status IN ('Placed','Offered','Pending')))",

@"IF NOT EXISTS (SELECT 1 FROM dbo.Students) AND NOT EXISTS (SELECT 1 FROM dbo.Companies) AND NOT EXISTS (SELECT 1 FROM dbo.Placements)
BEGIN
    INSERT INTO dbo.Students(FullName,Department,CGPA,Email) VALUES
    ('Aarav Patel','Computer',8.90,'aarav@mail.com'),
    ('Diya Shah','Computer',9.10,'diya@mail.com'),
    ('Kabir Mehta','IT',8.20,'kabir@mail.com'),
    ('Isha Joshi','IT',7.80,'isha@mail.com'),
    ('Rohan Desai','Electronics',7.50,'rohan@mail.com'),
    ('Meera Trivedi','Computer',8.60,'meera@mail.com'),
    ('Jay Vyas','IT',8.00,'jay@mail.com'),
    ('Priya Raval','Mechanical',7.20,'priya@mail.com'),
    ('Harsh Parmar','Computer',9.30,'harsh@mail.com'),
    ('Nidhi Bhatt','Electronics',8.40,'nidhi@mail.com');

    INSERT INTO dbo.Companies(CompanyName,Sector,PackageLPA) VALUES
    ('TCS','IT Services',3.60),
    ('Infosys','IT Services',4.00),
    ('Wipro','IT Services',3.80),
    ('Accenture','Consulting',6.50),
    ('Zoho','Product',8.00),
    ('L&T','Engineering',5.20);

    INSERT INTO dbo.Placements(StudentId,CompanyId,Role,PlacementDate,Status)
    SELECT s.StudentId, c.CompanyId, v.Role, CONVERT(date, v.PDate, 23), v.Status
    FROM (VALUES
      ('aarav@mail.com','Zoho','Software Developer','2026-08-10','Placed'),
      ('diya@mail.com','Accenture','Associate Engineer','2026-08-12','Placed'),
      ('kabir@mail.com','TCS','System Engineer','2026-08-15','Placed'),
      ('isha@mail.com','Infosys','Systems Specialist','2026-08-18','Offered'),
      ('rohan@mail.com','L&T','Graduate Engineer Trainee','2026-08-20','Placed'),
      ('meera@mail.com','Wipro','Project Engineer','2026-08-22','Placed'),
      ('jay@mail.com','TCS','System Engineer','2026-09-01','Offered'),
      ('priya@mail.com','L&T','Design Trainee','2026-09-03','Pending'),
      ('harsh@mail.com','Zoho','Backend Developer','2026-09-05','Placed'),
      ('nidhi@mail.com','Infosys','Systems Engineer','2026-09-08','Placed'),
      ('isha@mail.com','Wipro','Analyst','2026-09-10','Pending'),
      ('jay@mail.com','Accenture','Technology Analyst','2026-09-12','Offered')
    ) v(Email, Company, Role, PDate, Status)
    JOIN dbo.Students s ON s.Email = v.Email
    JOIN dbo.Companies c ON c.CompanyName = v.Company
END",

@"IF OBJECT_ID('dbo.vw_PlacementReport','V') IS NOT NULL DROP VIEW dbo.vw_PlacementReport",

@"CREATE VIEW dbo.vw_PlacementReport AS
SELECT p.PlacementId, s.StudentId, s.FullName, s.Department, s.CGPA,
       c.CompanyId, c.CompanyName, c.Sector, c.PackageLPA,
       p.Role, p.PlacementDate, p.Status
FROM dbo.Placements p
JOIN dbo.Students  s ON s.StudentId = p.StudentId
JOIN dbo.Companies c ON c.CompanyId = p.CompanyId"
    };
}
}
