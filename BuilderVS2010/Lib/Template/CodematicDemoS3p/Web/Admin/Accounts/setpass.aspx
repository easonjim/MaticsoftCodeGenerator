<%@ Page language="c#" Codebehind="SetPass.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.SetPass" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SetPass</title>		
		<LINK href="../../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor="#f5f9ff">
							<table cellSpacing="0" borderColorDark="#d3d8e0" cellPadding="5" width="100%" borderColorLight="#4f7fc9"
								border="1">
								<tr>
									<td bgColor="#e3efff" height="22" align="center"></td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="30%" height="22">
													<div align="right">�û�����</div>
												</td>
												<td height="22"><asp:textbox id="txtUserName" runat="server"></asp:textbox>
													<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName" ErrorMessage="�û�������Ϊ�գ�"></asp:RequiredFieldValidator></td>
											</tr>
											<tr>
												<td width="30%" height="22">
													<div align="right">���룺</div>
												</td>
												<td height="22"><asp:textbox id="txtPassword" runat="server" TextMode="Password"></asp:textbox></td>
											</tr>
											<tr>
												<td width="30%" height="22">
													<div align="right">����ȷ�ϣ�</div>
												</td>
												<td height="22"><asp:textbox id="txtPassword1" runat="server" TextMode="Password"></asp:textbox>
													<asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="���벻һ�£�" ControlToCompare="txtPassword"
														ControlToValidate="txtPassword1"></asp:CompareValidator></td>
											</tr>
											<tr>
												<td colSpan="2" height="22"><asp:label id="lblMsg" runat="server"></asp:label></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td height="22">
										<div align="center"><asp:button id="btnUpdate" runat="server" Text="�� �ύ ��" onclick="btnUpdate_Click"></asp:button><FONT face="����">&nbsp;</FONT>
											<asp:button id="btnCancel" runat="server" Text="�� ���� ��" onclick="btnCancel_Click"></asp:button></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
