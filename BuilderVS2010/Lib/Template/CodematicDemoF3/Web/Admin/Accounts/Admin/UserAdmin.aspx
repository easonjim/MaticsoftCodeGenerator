<%@ Register TagPrefix="cc1" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<%@ Page language="c#" Codebehind="UserAdmin.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.UserAdmin" %>

<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Index</title>
		<META http-equiv="Content-Type" content="text/html; charset=gb2312">		
		<LINK href="../../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table width="90%" border="0" cellspacing="0" cellpadding="0" align="center" id="Table1">
					<tr>
						<td class="TableBody1" valign="top">
							<table width="100%" border="0" align="center" cellpadding="5" cellspacing="0">
								<tr>
									<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
										<table 
										borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' cellPadding=5 
      width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
										border="1" cellSpacing=0 >
											<tr>
												<td height="25" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' align="center"><b>�û�����</b></td>
											</tr>
											<tr>
												<td height="22" valign="middle">
												&nbsp;&nbsp; ���ٲ�ѯ��<asp:DropDownList ID="DropUserType" runat="server">
                            <asp:ListItem Value="">ȫ���û�</asp:ListItem>                           
                            <asp:ListItem Value="AA" Selected="True">������Ա</asp:ListItem>
                        </asp:DropDownList>&nbsp;
													<asp:Label id="Label1" runat="server">�û����ؼ��֣�</asp:Label>
													<asp:TextBox id="TextBox1" runat="server" Width="100px" BorderStyle="Groove"></asp:TextBox>
													<asp:ImageButton id="BtnSearch" runat="server" ImageUrl="..\images\button_search.GIF" OnClick="BtnSearch_Click"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
													&nbsp;<a href="../add.aspx">��������û���</a>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
										<asp:DataGrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
											HorizontalAlign="Center" PageSize="20">
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
													<ItemTemplate>
														<a href='RoleAssignment.aspx?UserID=<%# DataBinder.Eval(Container, "DataItem.UserID") %>&PageIndex=<%# DataGrid1.CurrentPageIndex %>'>
															<%# DataBinder.Eval(Container, "DataItem.UserName") %>
														</a>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn DataField="TrueName" SortExpression="TrueName" ReadOnly="True" HeaderText="��ʵ����"></asp:BoundColumn>
												<asp:BoundColumn DataField="Sex" SortExpression="Sex" ReadOnly="True" HeaderText="�Ա�">
													<HeaderStyle Width="30px"></HeaderStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Phone" ReadOnly="True" HeaderText="��ϵ�绰"></asp:BoundColumn>
												<asp:BoundColumn DataField="Email" ReadOnly="True" HeaderText="�����ʼ�"></asp:BoundColumn>
												<asp:BoundColumn DataField="DepartmentID" HeaderText="������˾"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="�޸�">
													<HeaderStyle Width="30px"></HeaderStyle>
													<ItemTemplate>
														<asp:ImageButton id="BtnEdit" runat="server" ImageUrl="..\images\button_edit.gif" CommandName="BtnEdit"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="ɾ��">
													<HeaderStyle Width="30px"></HeaderStyle>
													<ItemTemplate>
														<asp:ImageButton id="BtnDel" runat="server" ImageUrl="..\images\button_del.gif" CommandName="BtnDel"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn Visible="False" DataField="UserID" ReadOnly="True" HeaderText="�û�ID">
													<HeaderStyle Width="30px"></HeaderStyle>
												</asp:BoundColumn>
											</Columns>
											<PagerStyle Font-Size="Medium" HorizontalAlign="Right" Wrap="False" Mode="NumericPages"></PagerStyle>
										</asp:DataGrid>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server" PermissionID="3"></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
