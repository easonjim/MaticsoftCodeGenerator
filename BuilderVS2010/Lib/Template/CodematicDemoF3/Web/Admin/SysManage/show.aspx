<%@ Page language="c#" Codebehind="show.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.show" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../../Controls/copyright.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>show</title>
		
		
		
		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<cc1:Navigation01 id="Navigation011" runat="server" Key_Str="id" Page_Mode="Show" Page_Index="treelist.aspx"
					Page_Add="add.aspx" Page_Modify="modify.aspx"></cc1:Navigation01>
				<table width="600" border="0" align="center" cellpadding="5" cellspacing="0">
					<tr>
						<td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
							<table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'>
								<tr bgcolor="#e4e4e4">
									<td height="22" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>��ϸ��Ϣ������޸Ŀ����޸ĵ�ǰ��Ϣ�����ɾ����ɾ����ǰ��Ϣ��</td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> ��ţ�</div>
												</td>
												<td height="22">
													<asp:Label id="lblID" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> ���ƣ�</div>
												</td>
												<td height="22">
													<asp:Label id="lblName" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> ���ࣺ</div>
												</td>
												<td height="22">
													<asp:Label id="lblTarget" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> ����ţ�</div>
												</td>
												<td height="22">
													<asp:Label id="lblOrderid" runat="server"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> ·����</div>
												</td>
												<td height="22">
													<asp:Label id="lblUrl" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">ͼ��(16x16)��</div>
												</td>
												<td height="22">
													<asp:Label id="lblImgUrl" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22"><div align="right">Ȩ�ޣ�</div>
												</td>
												<td style="HEIGHT: 3px" height="3">
													<asp:Label id="lblPermission" runat="server" Width="100%"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">˵����</div>
												</td>
												<td height="22">
													<asp:Label id="lblDescription" runat="server" Width="100%"></asp:Label></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="center"></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CopyRight id="CopyRight1" runat="server"></uc1:CopyRight>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
		</form>
	</body>
</HTML>
