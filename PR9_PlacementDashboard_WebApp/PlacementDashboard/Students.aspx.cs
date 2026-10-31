using System;
using System.Web.UI.WebControls;

namespace PlacementDashboard
{
    public partial class Students : BasePage
    {
        protected void dvStudent_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblMsg.Text = "<span class='err'>Could not add student: " + Server.HtmlEncode(e.Exception.Message) + "</span>";
                e.ExceptionHandled = true; e.KeepInInsertMode = true;
            }
            else { lblMsg.Text = "<span class='ok'>Student added successfully.</span>"; gvStudents.DataBind(); }
        }

        protected void gvStudents_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null) { lblMsg.Text = "<span class='err'>" + Server.HtmlEncode(e.Exception.Message) + "</span>"; e.ExceptionHandled = true; }
            else lblMsg.Text = "<span class='ok'>Student deleted.</span>";
        }

        protected void gvStudents_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                lblMsg.Text = "<span class='err'>Could not update: " + Server.HtmlEncode(e.Exception.Message) + "</span>";
                e.ExceptionHandled = true; e.KeepInEditMode = true;
            }
            else lblMsg.Text = "<span class='ok'>Student updated.</span>";
        }
    }
}
