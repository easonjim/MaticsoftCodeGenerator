<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="Show.aspx.cs" Inherits="JSoft.Web.SA.Tree.Show" Title="显示页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="查看SA_Tree" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以<asp:Literal ID="Literal3" runat="server" Text="查看SA_Tree" />
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
		<asp:Label id="lblNodeID" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		TreeText
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblTreeText" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ParentID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblParentID" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ParentPath
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblParentPath" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Location
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblLocation" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		OrderID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblOrderID" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Comment
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblComment" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Url
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblUrl" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		PermissionID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblPermissionID" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ImageUrl
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblImageUrl" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		ModuleID
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblModuleID" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		KeShiDM
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblKeShiDM" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		KeshiPublic
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblKeshiPublic" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		TreeType
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblTreeType" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right" class="td_class">
		Enabled
	：</td>
	<td height="25" width="*" align="left" class="td_width">
		<asp:Label id="lblEnabled" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td class="td_classshow">
	</td>
	<td height="25">
	<asp:Button ID="btnCancle" runat="server" CausesValidation="false" Text="<%$Resources:Site,btnBackText%>" class="adminsubmit_short" OnClick="btnCancle_Click"  OnClientClick="javascript:parent.$.colorbox.close();"></asp:Button>
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
