<%@ Page Title="SA_Tree" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="JSoft.Web.SA.Tree.List" %>

<%@ Register Assembly="JSoft.Web" Namespace="JSoft.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                         <asp:Literal ID="Literal1" runat="server" Text="SA_Tree" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以新增、修改、删除、查看<asp:Literal ID="Literal3" runat="server" Text="SA_Tree" />
                    </td>
                </tr>
            </table>
        </div>
    <!--Title end -->
    <!--Add  -->
    <!--Add end -->
    <!--Search -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
        <tr>
            <td width="1%" height="30" bgcolor="#FFFFFF" class="newstitlebody">
                <img src="/Admin/Images/icon-1.gif" width="19" height="19" />
            </td>
            <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                    OnClick="btnSearch_Click" class="adminsubmit"></asp:Button>
            </td>
        </tr>
    </table>
    <!--Search end-->
    <br />
    <div class="newslist">
        <div class="newsicon">
            <ul>
                <li style="background: url(/images/icon8.gif) no-repeat 5px 3px" runat="server" id="btnAdd"><a href="add.aspx">添加</a> <b>|</b> </li>
                <li style="background: url(/admin/images/delete.gif) no-repeat" runat="server" id="btnDelete2"><a href="#" runat="server" onserverclick="btnDelete_Click" onclick="return confirm('你确认要删除吗？');">删除</a><b>|</b></li>
                <li style="background: url(/admin/images/list.gif) no-repeat"><a href="list.aspx">浏览</a><b>|</b></li>
                <li style="background: url(/admin/images/redo.gif) no-repeat"><a href="#" runat="server" onserverclick="btnReturn_Click" >返回</a><b>|</b></li>
            </ul>
        </div>
    </div>
    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
        ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
        OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
        Width="100%" PageSize="10" ShowExportExcel="True" ShowExportWord="True" ExcelFileName="FileName1"
        CellPadding="0" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="NodeID" SortExpressionStr="NodeID" OnRowCommand="gridView_RowCommand" >
        <columns>
            
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="TreeText" HeaderText="TreeText" SortExpression="TreeText" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="TreeText" SortExpression="TreeText" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("TreeText")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="ParentID" HeaderText="ParentID" SortExpression="ParentID" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="ParentID" SortExpression="ParentID" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("ParentID")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="ParentPath" HeaderText="ParentPath" SortExpression="ParentPath" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="ParentPath" SortExpression="ParentPath" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("ParentPath")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="Location" SortExpression="Location" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("Location")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="OrderID" HeaderText="OrderID" SortExpression="OrderID" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="OrderID" SortExpression="OrderID" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("OrderID")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="Comment" SortExpression="Comment" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("Comment")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="Url" HeaderText="Url" SortExpression="Url" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="Url" SortExpression="Url" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("Url")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="PermissionID" HeaderText="PermissionID" SortExpression="PermissionID" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="PermissionID" SortExpression="PermissionID" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("PermissionID")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="ImageUrl" HeaderText="ImageUrl" SortExpression="ImageUrl" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="ImageUrl" SortExpression="ImageUrl" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("ImageUrl")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="ModuleID" HeaderText="ModuleID" SortExpression="ModuleID" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="ModuleID" SortExpression="ModuleID" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("ModuleID")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="KeShiDM" HeaderText="KeShiDM" SortExpression="KeShiDM" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="KeShiDM" SortExpression="KeShiDM" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("KeShiDM")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="KeshiPublic" HeaderText="KeshiPublic" SortExpression="KeshiPublic" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="KeshiPublic" SortExpression="KeshiPublic" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("KeshiPublic")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="TreeType" HeaderText="TreeType" SortExpression="TreeType" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="TreeType" SortExpression="TreeType" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("TreeType")%>
			</ItemTemplate>
		</asp:TemplateField>
		<%--模板列操作快速方法--%><%--<asp:BoundField DataField="Enabled" HeaderText="Enabled" SortExpression="Enabled" ItemStyle-HorizontalAlign="Center"  />--%> 
		<asp:TemplateField HeaderText="Enabled" SortExpression="Enabled" ItemStyle-HorizontalAlign="Center">
			<ItemTemplate>
				<%#Eval("Enabled").ToString().ToLower() == "true" ? "是" : "否"%>
			</ItemTemplate>
		</asp:TemplateField>
                           
            
            <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnDetailText %>" ControlStyle-Width="50" DataNavigateUrlFields="NodeID" DataNavigateUrlFormatString="Show.aspx?id={0}"
                Text="<%$ Resources:Site, btnDetailText %>"  ItemStyle-HorizontalAlign="Center" />
            <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnEditText %>" ControlStyle-Width="50" DataNavigateUrlFields="NodeID" DataNavigateUrlFormatString="Modify.aspx?id={0}"
                Text="<%$ Resources:Site, btnEditText %>"  ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"   Visible="false"  ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                         Text="<%$ Resources:Site, btnDeleteText %>" OnClientClick="return confirm('你确认要删除吗？');"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </columns>
        <footerstyle height="25px" horizontalalign="Right" />
        <headerstyle height="25px" />
        <pagerstyle height="25px" horizontalalign="Right" />
        <sorttip ascimg="~/Images/up.JPG" descimg="~/Images/down.JPG" />
        <rowstyle height="25px" />
        <sortdirectionstr>DESC</sortdirectionstr>
    </cc1:GridViewEx>
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td style="width: 1px;">
            </td>
            <td>
                <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"  class="adminsubmit" OnClick="btnDelete_Click" />
                <asp:Button ID="btnReturn"  runat="server" Text="返回"  class="adminsubmit"  OnClick="btnReturn_Click" />
            </td>
        </tr>
    </table></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
