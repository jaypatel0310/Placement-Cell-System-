using System;
using System.Web.UI.WebControls;

namespace PlacementDashboard
{
    public partial class Placements : BasePage
    {
        protected void dvPlacement_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblMsg.Text = "<span class='err'>Could not save: " + Server.HtmlEncode(e.Exception.Message) + "</span>";
                e.ExceptionHandled = true; e.KeepInInsertMode = true;
                return;
            }
            lblMsg.Text = "<span class='ok'>Placement saved successfully.</span>";
            gvPlacements.DataBind();   // refresh the table
        }

        protected void gvPlacements_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null) { lblMsg.Text = "<span class='err'>" + Server.HtmlEncode(e.Exception.Message) + "</span>"; e.ExceptionHandled = true; }
            else lblMsg.Text = "<span class='ok'>Placement deleted.</span>";
        }
    }
}
