using System;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

namespace PlacementDashboard
{
    public partial class CrystalReportPage : BasePage
    {
        ReportDocument rpt = new ReportDocument();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (DbError != null) { Response.Write("Database error: " + Server.HtmlEncode(DbError)); Response.End(); return; }

            DataTable dt = new DataTable("vw_PlacementReport");
            using (SqlConnection con = new SqlConnection(DbInit.ConnectionString))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_PlacementReport ORDER BY Department, FullName", con))
            {
                da.Fill(dt);
            }
            rpt.Load(Server.MapPath("~/Reports/PlacementReport.rpt"));
            rpt.SetDataSource(dt);                  // live SQL Server data goes into the report
            CrystalReportViewer1.ReportSource = rpt;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
        }
    }
}
