<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Page language="c#" Codebehind="userupdate.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.userupdate" %>

<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>userupdate</title>		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
		
</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' 
    >
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
								<tr bgColor="#e4e4e4">
									<td 
          bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' 
          height=22 align=center>�� <STRONG>�û���Ϣ�޸� </STRONG>��</td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> �û�����</div>
												</td>
												<td height="22"><asp:label id="lblName" runat="server"></asp:label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">��ʵ������</div>
												</td>
												<td height="22"><asp:textbox id="txtTrueName" runat="server" Width="200px"></asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">�����룺</div>
												</td>
												<td height="22"><asp:textbox id="txtPassword" runat="server" Width="200px" TextMode="Password"></asp:textbox></td>
											</tr>
											<tr>
												<td align="right" width="150" height="22">�û��Ա�
												</td>
												<td height="22"><FONT face="����">&nbsp;&nbsp;&nbsp;<asp:radiobutton id="RadioButton1" runat="server" GroupName="optSex" Checked="True" Text="��"></asp:radiobutton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:radiobutton id="RadioButton2" runat="server" GroupName="optSex" Text="Ů"></asp:radiobutton></FONT></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">��ϵ�绰��</div>
												</td>
												<td style="HEIGHT: 3px" height="3">
													<asp:TextBox id="txtPhone" runat="server" Width="200px"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">�������䣺</div>
												</td>
												<td height="22"><asp:textbox id="txtEmail" runat="server" Width="200px"></asp:textbox></td>
											</tr>
											
											<tr>
												<td style="HEIGHT: 5px" width="150" height="5"><div align="right">������</div>
												</td>
												<td style="HEIGHT: 5px" height="5"><asp:dropdownlist id="dropStyle" runat="server" Width="200px">
														<asp:ListItem Value="1">Ĭ����</asp:ListItem>
														<asp:ListItem Value="2">�����</asp:ListItem>
														<asp:ListItem Value="3">���</asp:ListItem>
														<asp:ListItem Value="4">����</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
											<tr>
												<td colSpan="2"><asp:label id="lblMsg" runat="server" ForeColor="Red"></asp:label></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td><asp:label id="RoleList" Visible="False" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td height="22">
										<div align="center"><asp:button id="btnAdd" runat="server" Text="�� �ύ ��" onclick="btnAdd_Click"></asp:button><FONT face="����">&nbsp;</FONT>
											<input type="button" name="button1" value="�� �� �� ��" onclick="history.back()"></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
