<%@ Page language="c#" Codebehind="LogShow.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.LogShow" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LogShow</title>
		
		
		
		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table width="600" border="0" cellspacing="0">
					<tr>
						<td align="right"><a href="LogIndex.aspx">[ ���� ]</a></td>
					</tr>
				</table>
				<table width="600" border="0" align="center" cellpadding="5" cellspacing="0">
					<tr>
						<td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
							<table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'>
								<tr bgcolor="#e4e4e4">
									<td height="22" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>�������ϸ��Ϣ��</td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="100" height="22">
													<div align="right">
														����ʱ�䣺</div>
												</td>
												<td height="22"><%=strtime%>
												</td>
											</tr>
											<tr>
												<td width="100" height="22">
													<div align="right">
														������Ϣ��</div>
												</td>
												<td height="22"><%=errmsg%>
												</td>
											</tr>
											<tr>
												<td width="100" height="22" align="right" valign="top">��ջ��ϸ��
												</td>
												<td height="22"><%=Particular%>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
		</form>
	</body>
</HTML>
