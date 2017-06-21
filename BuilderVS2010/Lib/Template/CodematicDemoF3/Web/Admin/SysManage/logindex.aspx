<%@ Page language="c#" Codebehind="LogIndex.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.LogIndex" %>

<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LogIndex</title>
		
		
		
		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="CheckBox.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table border="0" cellpadding="5" width="700">
					<tr>
						<td align="center"><STRONG><FONT face="����" size="4">ϵͳ��־����</FONT></STRONG></td>
					</tr>
				</table>
				<table cellSpacing="0" cellPadding="5" width="700" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' 
    ><asp:datagrid id="grid" runat="server" DataKeyField="ID" Width="100%" AutoGenerateColumns="False"
								AllowPaging="True">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="ID" ReadOnly="True" HeaderText="���">
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="ѡ��">
										<HeaderStyle Wrap="False" HorizontalAlign="Center" Width="25px"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
										<ItemTemplate>
											<asp:CheckBox id="DeleteThis" onclick="javascript:CCA(this);" runat="server"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="datetime" ReadOnly="True" HeaderText="����" DataFormatString="{0:d}">
										<HeaderStyle Wrap="False" Width="50px"></HeaderStyle>
										<ItemStyle Wrap="False"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="datetime" HeaderText="ʱ��" DataFormatString="{0:T}">
										<HeaderStyle Width="43px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="logshow.aspx?id={0}" DataTextField="loginfo"
										HeaderText="��Ϣ"></asp:HyperLinkColumn>
								</Columns>
								<PagerStyle Visible="False"></PagerStyle>
							</asp:datagrid></td>
					</tr>
				</table>
				<cc1:page02 id="Page021" runat="server" Page_Index="LogIndex.aspx" Page_Size="10" PageStep="6"></cc1:page02>
				<table border="0" cellpadding="0" width="700">
					<tr>
						<td><input type="checkbox" name="allbox" onclick="CA();">ȫѡ</td>
						<td><asp:Button Text=" ɾ �� " ID="Confirm" runat="server" BorderStyle="Groove" BackColor="Transparent"
								BorderColor="RoyalBlue" Height="20px" BorderWidth="1px" Font-Size="XX-Small" onclick="Confirm_Click" /></td>
						<td><asp:Button Text="���ȫ����־" ID="btnDelAll" runat="server" BorderStyle="Groove" BackColor="Transparent"
								BorderColor="RoyalBlue" Height="20px" BorderWidth="1px" Font-Size="XX-Small" onclick="btnDelAll_Click" /></td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
