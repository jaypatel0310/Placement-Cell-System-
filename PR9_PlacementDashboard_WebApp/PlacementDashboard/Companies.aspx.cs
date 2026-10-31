using System;
using System.Web.UI.WebControls;

namespace PlacementDashboard
{
    public partial class Companies : BasePage
    {
        protected void dvCompany_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblMsg.Text = "<span class='err'>Could not add company: " + Server.HtmlEncode(e.Exception.Message) + "</span>";
                e.ExceptionHandled = true; e.KeepInInsertMode = true;
            }
            else { lblMsg.Text = "<span class='ok'>Company added successfully.</span>"; gvCompanies.DataBind(); }
        }

        protected void gvCompanies_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null) { lblMsg.Text = "<span class='err'>" + Server.HtmlEncode(e.Exception.Message) + "</span>"; e.ExceptionHandled = true; }
            else lblMsg.Text = "<span class='ok'>Company deleted.</span>";
        }

        protected void gvCompanies_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblMsg.Text = "<span class='err'>Could not update: " + Server.HtmlEncode(e.Exception.Message) + "</span>";
                e.ExceptionHandled = true; e.KeepInEditMode = true;
            }
            else lblMsg.Text = "<span class='ok'>Company updated.</span>";
        }
    }
}
