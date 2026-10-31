using System;
using System.Data;
using System.Web.UI.WebControls;

namespace PlacementDashboard
{
    public partial class Report : BasePage
    {
        int rows = 0; decimal total = 0, max = 0;

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal p = Convert.ToDecimal(((DataRowView)e.Row.DataItem)["PackageLPA"]);
                rows++; total += p; if (p > max) max = p;
            }
        }

        protected void gvReport_DataBound(object sender, EventArgs e)
        {
            lblTotals.Text = rows == 0 ? "No records." :
                "<b>Records:</b> " + rows + " &nbsp;|&nbsp; <b>Average Package:</b> " + (total / rows).ToString("0.00") +
                " LPA &nbsp;|&nbsp; <b>Highest Package:</b> " + max.ToString("0.00") + " LPA";
        }
    }
}
