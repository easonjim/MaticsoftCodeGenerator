<%@ Page language="c#" Codebehind="search.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.search" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../../Controls/copyright.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>search</title>
		
		
		
		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table width="600" border="0" cellspacing="0" cellpadding="0" align="center">
				<tr>
					<td height="22">
						<div align="right">[ <a href="treelist.aspx">����</a> ]
						</div>
					</td>
				</tr>
			</table>
			<table width="600" border="0" align="center" cellpadding="5" cellspacing="0">
				<tr>
					<td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
						<table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'>
							<tr bgcolor="#e4e4e4">
								<td height="22" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>��Ϣ��������������ѡ���ѡȡ��Ӧ�������м���������д��ʾ���������ơ�</td>
							</tr>
							<tr>
								<td height="22"><table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td width="150" height="22">
												<div align="right"><font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
													��ţ�</div>
											</td>
											<td height="22">
												<asp:TextBox id="txtID" runat="server" Width="200px" ToolTip="�˵����"></asp:TextBox></td>
										</tr>
										<tr>
											<td width="150" height="22">
												<div align="right"><font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
													���ƣ�</div>
											</td>
											<td height="22">
												<asp:TextBox id="txtName" runat="server" Width="200px" ToolTip="�˵����ƹؼ���"></asp:TextBox></td>
										</tr>
										<tr>
											<td width="150" height="15" style="HEIGHT: 15px">
												<div align="right"><font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
													���ࣺ</div>
											</td>
											<td height="15" style="HEIGHT: 15px">
												<asp:dropdownlist id="listTarget" runat="server" Width="200px">
													<asp:ListItem Value="0" Selected="True">��Ŀ¼</asp:ListItem>
												</asp:dropdownlist></td>
										</tr>
										<tr>
											<td width="150" height="22">
												<div align="right">
													Ȩ�ޣ�</div>
											</td>
											<td height="22">
												<asp:DropDownList id="listPermission" runat="server" Width="300px"></asp:DropDownList></td>
										</tr>
										<tr>
											<td width="150" height="22">
												<div align="right">
													˵����</div>
											</td>
											<td height="22">
												<asp:TextBox id="txtDescription" runat="server" Width="300px" ToolTip="�˵�˵���Ĺؼ���"></asp:TextBox></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td height="22">
									<div align="center">&nbsp;
										<asp:button id="btnSearch" runat="server" Text="�� ��ѯ ��" onclick="btnSearch_Click"></asp:button>&nbsp;
										<asp:button id="btnCancel" runat="server" Text="�� ���� ��" onclick="btnCancel_Click"></asp:button>
									</div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<uc1:CopyRight id="CopyRight1" runat="server"></uc1:CopyRight>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
		</form>
	</body>
</HTML>
