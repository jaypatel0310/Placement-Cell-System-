<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="PlacementDashboard.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>Placement Cell Report</h2>

<asp:SqlDataSource ID="sdsDeptFilter" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT DISTINCT Department FROM Students ORDER BY Department" />
<asp:SqlDataSource ID="sdsData" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT * FROM vw_PlacementReport WHERE (@Dept = 'All' OR Department = @Dept) AND (@Status = 'All' OR Status = @Status) ORDER BY Department, FullName">
 <SelectParameters>
  <asp:ControlParameter Name="Dept" ControlID="ddlDept" PropertyName="SelectedValue" Type="String" DefaultValue="All" />
  <asp:ControlParameter Name="Status" ControlID="ddlStatus" PropertyName="SelectedValue" Type="String" DefaultValue="All" />
 </SelectParameters>
</asp:SqlDataSource>

<div class="box noprint">
 Department:
 <asp:DropDownList ID="ddlDept" runat="server" DataSourceID="sdsDeptFilter" DataTextField="Department" DataValueField="Department"
  AppendDataBoundItems="true" AutoPostBack="true"><asp:ListItem Value="All" Text="All" /></asp:DropDownList>
 &nbsp; Status:
 <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true">
  <asp:ListItem>All</asp:ListItem><asp:ListItem>Placed</asp:ListItem><asp:ListItem>Offered</asp:ListItem><asp:ListItem>Pending</asp:ListItem>
 </asp:DropDownList>
 &nbsp; <input type="button" value="Print / Save as PDF" onclick="window.print()" />
</div>
<div class="box"><asp:Label ID="lblTotals" runat="server" /></div>
<asp:GridView ID="gvReport" runat="server" DataSourceID="sdsData" AutoGenerateColumns="False" CssClass="tbl" GridLines="None"
 EmptyDataText="No records found." OnRowDataBound="gvReport_RowDataBound" OnDataBound="gvReport_DataBound">
 <Columns>
  <asp:BoundField DataField="FullName" HeaderText="Student" />
  <asp:BoundField DataField="Department" HeaderText="Department" />
  <asp:BoundField DataField="CGPA" HeaderText="CGPA" DataFormatString="{0:0.00}" HtmlEncode="false" />
  <asp:BoundField DataField="CompanyName" HeaderText="Company" />
  <asp:BoundField DataField="Role" HeaderText="Role" />
  <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" DataFormatString="{0:0.00}" HtmlEncode="false" />
  <asp:BoundField DataField="PlacementDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" />
  <asp:BoundField DataField="Status" HeaderText="Status" />
 </Columns>
</asp:GridView>
<p class="noprint">Crystal Reports version: see README.txt, Part B.</p>
</asp:Content>
