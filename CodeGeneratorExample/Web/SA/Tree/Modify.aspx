<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Modify.aspx.cs" Inherits="JSoft.Web.SA.Tree.Modify" Title="修改页" %>

<%@ Register Src="~/Controls/UCDroplistPermission.ascx" TagName="UCDroplistPermission"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="编辑SA_Tree" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以<asp:Literal ID="Literal3" runat="server" Text="编辑SA_Tree" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		NodeID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:label id="lblNodeID" runat="server"></asp:label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		TreeText
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtTreeText" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ParentID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtParentID" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ParentPath
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtParentPath" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Location
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtLocation" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		OrderID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtOrderID" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Comment
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtComment" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Url
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtUrl" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		PermissionID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtPermissionID" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ImageUrl
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtImageUrl" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ModuleID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtModuleID" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		KeShiDM
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtKeShiDM" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		KeshiPublic
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtKeshiPublic" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		TreeType
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:TextBox id="txtTreeType" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Enabled
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:CheckBox ID="chkEnabled" Text="Enabled" runat="server" Checked="False" />
	</td></tr>
	<tr>
	<td class="td_class">
	</td>
	<td height="25">
	<asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
	OnClick="btnSave_Click" class="adminsubmit"></asp:Button>
	<asp:Button ID="btnCancle" runat="server" CausesValidation="false" Text="<%$ Resources:Site, btnCancleText %>"
	OnClick="btnCancle_Click" class="adminsubmit"></asp:Button>
	</td>
	</tr>
</table>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
