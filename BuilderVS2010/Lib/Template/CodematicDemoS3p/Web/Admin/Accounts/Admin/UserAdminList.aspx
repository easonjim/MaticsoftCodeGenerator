<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAdminList.aspx.cs" Inherits="Maticsoft.Web.Accounts.Admin.UserAdminList" %>
<%@ Register TagPrefix="asp" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Index</title>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <link href="../../../style.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table width="90%" border="0" cellspacing="0" cellpadding="0" align="center" id="Table1">
                <tr>
                    <td class="TableBody1" valign="top">
                        <table width="100%" border="0" align="center" cellpadding="5" cellspacing="0">
                            <tr>
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                                    <table bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                                        cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                                        border="1" cellspacing="0">
                                        <tr>
                                            <td height="25" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                                align="center">
                                                <b>�û�����</b></td>
                                        </tr>
                                        <tr>
                                            <td height="22" valign="middle">
                                                &nbsp;&nbsp; ���ٲ�ѯ��<asp:DropDownList ID="DropUserType" runat="server">
                                                    <asp:ListItem Value="" Selected="True">ȫ���û�</asp:ListItem>
                                                    <asp:ListItem Value="PP">PPC�û�</asp:ListItem>
                                                    <asp:ListItem Value="SU">�����</asp:ListItem>
                                                    <asp:ListItem Value="WS">��վ��</asp:ListItem>
                                                    <asp:ListItem Value="AG">��������</asp:ListItem>
                                                    <asp:ListItem Value="WG">��վ������</asp:ListItem>
                                                    <asp:ListItem Value="PG">PPC������</asp:ListItem>
                                                    <asp:ListItem Value="AA">������Ա</asp:ListItem>
                                                    <asp:ListItem Value="SC">�绰�û�</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                                <asp:Label ID="Label1" runat="server">�û����ؼ��֣�</asp:Label>
                                                <asp:TextBox ID="TextBox1" runat="server" Width="100px" BorderStyle="Groove"></asp:TextBox>
                                                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="..\images\button_search.GIF"
                                                    OnClick="BtnSearch_Click"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                &nbsp;<a href="../add.aspx?List=List">��������û���</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            	<table cellSpacing=0 cellPadding=0 width=100% align=center border=0>					
					<tr id="TrGrid" runat="server">
						<td align="left">�� ҳ�Σ�<asp:label id="lblpage" runat="server" ForeColor="#E78A29"></asp:label>/
							<asp:label id="lblpagesum" runat="server"></asp:label>������<FONT color="#e78a29"><asp:label id="lblrowscount" runat="server"></asp:label></FONT>��</td>
						<td align="right"><asp:linkbutton id="btnFirst" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="First"
								CommandName="Pager" Text="�� ҳ">[�� ҳ]</asp:linkbutton><asp:linkbutton id="btnPrev" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Prev"
								CommandName="Pager" Text="��һҳ">[��һҳ]</asp:linkbutton><asp:linkbutton id="btnNext" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Next"
								CommandName="Pager" Text="��һҳ">[��һҳ]</asp:linkbutton><asp:linkbutton id="btnLast" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Last"
								CommandName="Pager" Text="β ҳ">[β ҳ]</asp:linkbutton></td>
					</tr>
					</table>
                            
                            </td>
                            </tr>
                            <tr>
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AllowPaging="True" HorizontalAlign="Center" PageSize="20">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="���">
                                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                <%# DataGrid1.CurrentPageIndex*DataGrid1.PageSize+DataGrid1.Items.Count+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="UserID" HeaderText="�û���">
                                                <ItemTemplate><a href='../../Agent/Role/RoleAssignment.aspx?UserID=<%# DataBinder.Eval(Container, "DataItem.UserID") %>&PageIndex=<%# DataGrid1.CurrentPageIndex %>'>
                                                    <%# DataBinder.Eval(Container, "DataItem.UserName") %>
                                                </a></ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="TrueName" SortExpression="TrueName" ReadOnly="True" HeaderText="��ʵ����">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Sex" SortExpression="Sex" ReadOnly="True" HeaderText="�Ա�">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Phone" ReadOnly="True" HeaderText="��ϵ�绰"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Email" ReadOnly="True" HeaderText="�����ʼ�"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DepartmentID" HeaderText="������˾"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="�޸�">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="..\images\button_edit.gif"
                                                        CommandName="BtnEdit"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="ɾ��">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnDel" runat="server" ImageUrl="..\images\button_del.gif" CommandName="BtnDel">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="UserID" ReadOnly="True" HeaderText="�û�ID">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle Font-Size="Medium" HorizontalAlign="Right" Wrap="False" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <uc1:CheckRight ID="CheckRight1" runat="server" PermissionID="398"></uc1:CheckRight>
        <uc2:copyright ID="Copyright1" runat="server" />
    </form>
</body>
</html>
