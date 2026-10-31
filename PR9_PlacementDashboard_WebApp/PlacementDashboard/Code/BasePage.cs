using System;

namespace PlacementDashboard
{
    /// <summary>Every page inherits this. It makes sure the database exists and every SqlDataSource is connected.</summary>
    public class BasePage : System.Web.UI.Page
    {
        public string DbError { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            DbError = DbInit.Ensure(this);
            base.OnInit(e);
        }
    }
}
