<%@ Page Language="c#" Codebehind="Add.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Add" %>

<%@ Register Src="../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Add</title>
    <link href="../../style.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../js/date.js"></script>

</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="90%" align="center" border="0">
                <tr>
                    <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr bgcolor="#e4e4e4">
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                    height="25" align="center">
                                    �� <strong>�����û� </strong>��</td>
                            </tr>
                            <tr>
                                <td height="25">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    <font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
                                                    �û�����</div>
                                            </td>
                                            <td height="25">
                                                <asp:TextBox ID="txtUserName" TabIndex="1" runat="server" Width="249px" MaxLength="20"
                                                    BorderStyle="Groove"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="�û�������Ϊ��"
                                                    Display="Dynamic" ControlToValidate="txtUserName"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    <font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
                                                    ��&nbsp;&nbsp;&nbsp;&nbsp;�룺</div>
                                            </td>
                                            <td height="25">
                                                <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" Width="249px" MaxLength="20"
                                                    BorderStyle="Groove" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="���벻��Ϊ��"
                                                    Display="Dynamic" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    <font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
                                                    ������֤��</div>
                                            </td>
                                            <td height="25">
                                                <asp:TextBox ID="txtPassword1" TabIndex="3" runat="server" Width="249px" MaxLength="20"
                                                    BorderStyle="Groove" TextMode="Password"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="�����������벻��"
                                                    Display="Dynamic" ControlToValidate="txtPassword1" ControlToCompare="txtPassword"></asp:CompareValidator></td>
                                        </tr>
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    <font color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>'>*</font>
                                                    ��ʵ������</div>
                                            </td>
                                            <td height="25">
                                                <asp:TextBox ID="txtTrueName" TabIndex="4" runat="server" Width="249px" MaxLength="20"
                                                    BorderStyle="Groove"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="��ʵ��������Ϊ��"
                                                    Display="Dynamic" ControlToValidate="txtTrueName"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td align="right" width="150" height="25">
                                                �û��Ա�
                                            </td>
                                            <td height="25">
                                                <font face="����">&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="RadioButton1" runat="server"
                                                    GroupName="optSex" Checked="True" Text="��"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton
                                                        ID="RadioButton2" runat="server" GroupName="optSex" Text="Ů"></asp:RadioButton></font></td>
                                        </tr>
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    ��ϵ�绰��</div>
                                            </td>
                                            <td style="height: 3px" height="3">
                                                <asp:TextBox ID="txtPhone" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150" height="25">
                                                <div align="right">
                                                    �������䣺</div>
                                            </td>
                                            <td height="25">
                                                <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px" width="150" height="5">
                                                <div align="right">
                                                    ������</div>
                                            </td>
                                            <td style="height: 5px" height="5">
                                                <asp:DropDownList ID="dropStyle" runat="server" Width="200px">
                                                    <asp:ListItem Value="1">Ĭ����</asp:ListItem>
                                                    <asp:ListItem Value="2">�����</asp:ListItem>
                                                    <asp:ListItem Value="3">���</asp:ListItem>
                                                    <asp:ListItem Value="4">����</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="25">
                                    <div align="center">
                                        <asp:Button ID="btnAdd" runat="server" Text="�� �ύ ��" OnClick="btnAdd_Click"></asp:Button>
                                        <input type="button" name="button1" value="�� �� �� ��" onclick="history.back()"></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <uc1:copyright ID="Copyright1" runat="server" />
        <uc2:checkright ID="Checkright1" runat="server" />
    </form>
</body>
</html>
