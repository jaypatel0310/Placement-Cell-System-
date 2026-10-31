<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PlacementDashboard._Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Placement Overview</h2>

<%-- DATA CONTROL: SqlDataSource + FormView (summary cards) --%>
<asp:SqlDataSource ID="sdsStats" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT (SELECT COUNT(*) FROM Students) AS TotalStudents,
 (SELECT COUNT(DISTINCT StudentId) FROM Placements WHERE Status='Placed') AS PlacedStudents,
 ISNULL(CAST(100.0 * (SELECT COUNT(DISTINCT StudentId) FROM Placements WHERE Status='Placed') / NULLIF((SELECT COUNT(*) FROM Students),0) AS INT),0) AS PlacementPercent,
 (SELECT COUNT(*) FROM Companies) AS TotalCompanies,
 ISNULL((SELECT MAX(c.PackageLPA) FROM Placements p JOIN Companies c ON c.CompanyId=p.CompanyId WHERE p.Status='Placed'),0) AS HighestPackage,
 ISNULL((SELECT AVG(c.PackageLPA) FROM Placements p JOIN Companies c ON c.CompanyId=p.CompanyId WHERE p.Status='Placed'),0) AS AveragePackage" />
<asp:FormView ID="fvStats" runat="server" DataSourceID="sdsStats">
 <ItemTemplate>
  <div class="card"><b><%# Eval("TotalStudents") %></b>Total Students</div>
  <div class="card"><b><%# Eval("PlacedStudents") %></b>Students Placed</div>
  <div class="card"><b><%# Eval("PlacementPercent") %>%</b>Placement Rate</div>
  <div class="card"><b><%# Eval("TotalCompanies") %></b>Companies</div>
  <div class="card"><b><%# Eval("HighestPackage", "{0:0.00}") %> LPA</b>Highest Package</div>
  <div class="card"><b><%# Eval("AveragePackage", "{0:0.00}") %> LPA</b>Average Package</div>
 </ItemTemplate>
</asp:FormView>

<%-- DATA CONTROL: Repeater (department bars) --%>
<div class="box"><h3>Department-wise Placement</h3>
<asp:SqlDataSource ID="sdsDept" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT s.Department, COUNT(DISTINCT s.StudentId) AS Total, COUNT(DISTINCT p.StudentId) AS Placed,
  CAST(100.0 * COUNT(DISTINCT p.StudentId) / COUNT(DISTINCT s.StudentId) AS INT) AS Pct
  FROM Students s LEFT JOIN Placements p ON p.StudentId=s.StudentId AND p.Status='Placed'
  GROUP BY s.Department ORDER BY s.Department" />
<asp:Repeater ID="rptDept" runat="server" DataSourceID="sdsDept">
 <ItemTemplate>
  <div class="brow"><span><%# Eval("Department") %></span>
   <div class="bar"><div class="fill" style="width:<%# Eval("Pct") %>%"></div></div>
   <b><%# Eval("Pct") %>%</b> (<%# Eval("Placed") %> of <%# Eval("Total") %>)</div>
 </ItemTemplate>
</asp:Repeater></div>

<%-- DATA CONTROL: Repeater (company boxes) --%>
<div class="box"><h3>Companies &amp; Hires</h3>
<asp:SqlDataSource ID="sdsCompanyHires" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT c.CompanyName, c.Sector, c.PackageLPA, COUNT(p.PlacementId) AS Hires
  FROM Companies c LEFT JOIN Placements p ON p.CompanyId=c.CompanyId AND p.Status='Placed'
  GROUP BY c.CompanyName, c.Sector, c.PackageLPA ORDER BY c.CompanyName" />
<asp:Repeater ID="rptCompanies" runat="server" DataSourceID="sdsCompanyHires">
 <ItemTemplate>
  <div class="co"><b><%# Eval("CompanyName") %></b><br />
   <%# Eval("Sector") %> | <%# Eval("PackageLPA", "{0:0.00}") %> LPA<br />Hired: <%# Eval("Hires") %></div>
 </ItemTemplate>
</asp:Repeater></div>

<%-- DATA CONTROL: GridView (latest placements) --%>
<div class="box"><h3>Latest 5 Placements</h3>
<asp:SqlDataSource ID="sdsRecent" runat="server" ConnectionString="<%$ ConnectionStrings:PlacementCS %>"
 SelectCommand="SELECT TOP 5 FullName, Department, CompanyName, Role, PackageLPA, PlacementDate, Status FROM vw_PlacementReport ORDER BY PlacementDate DESC, PlacementId DESC" />
<asp:GridView ID="gvRecent" runat="server" DataSourceID="sdsRecent" AutoGenerateColumns="False" CssClass="tbl" GridLines="None">
 <Columns>
  <asp:BoundField DataField="FullName" HeaderText="Student" />
  <asp:BoundField DataField="Department" HeaderText="Department" />
  <asp:BoundField DataField="CompanyName" HeaderText="Company" />
  <asp:BoundField DataField="Role" HeaderText="Role" />
  <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" DataFormatString="{0:0.00}" HtmlEncode="false" />
  <asp:BoundField DataField="PlacementDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" />
  <asp:BoundField DataField="Status" HeaderText="Status" />
 </Columns>
</asp:GridView></div>

</asp:Content>
