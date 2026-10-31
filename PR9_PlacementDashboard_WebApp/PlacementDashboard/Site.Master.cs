using System;
using System.IO;
using System.Web.UI;

namespace PlacementDashboard
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var bp = Page as BasePage;
            if (bp != null && bp.DbError != null)
            {
                pnlContent.Visible = false;
                pnlError.Visible = true;
                lblDbError.Text = Server.HtmlEncode(bp.DbError);
            }
            // The Crystal Report link appears only after you add CrystalReport.aspx (README Part B)
            lnkCrystal.Visible = File.Exists(Server.MapPath("~/CrystalReport.aspx"));
        }
    }
}
