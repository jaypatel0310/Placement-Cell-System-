<%-- Add THESE 3 FILES to the project ONLY AFTER you made Reports\PlacementReport.rpt (README.txt, Part B).
     If VS complains about the Register line, delete it plus the <CR:...> tag, drag a CrystalReportViewer from the
     Toolbox onto the page and name it CrystalReportViewer1 - VS then writes the right version numbers itself. --%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReport.aspx.cs" Inherits="PlacementDashboard.CrystalReportPage" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><title>Crystal Report - Placements</title></head>
<body>
<form id="form1" runat="server">
  <a href="Default.aspx">&laquo; Back to Dashboard</a>
  <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
      ToolPanelView="None" HasToggleGroupTreeButton="False" />
</form>
</body>
</html>
