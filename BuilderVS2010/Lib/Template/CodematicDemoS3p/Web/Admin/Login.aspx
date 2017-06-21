<%@ Page Language="c#" CodeBehind="Login.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.Login" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>ϵͳ��¼</title>
    <link href="images/login.css" type="text/css" rel="stylesheet">

    <script language="javascript" type="text/javascript">
   function ChangeCode()
	    {
	   
	        var date = new Date();
	        var myImg = document.getElementById("ImageCheck"); 	     
	        var GUID = document.getElementById("lblGUID");  
	        
	        if(GUID != null)
	        {
	            if(GUID.innerHTML != "" && GUID.innerHTML != null)
	            { 
	                myImg.src = "../ValidateCode.aspx?GUID="+GUID.innerHTML+"&flag="+date.getMilliseconds()
	              
	            }
	        }
	        return false;
	    }

	    function validate() {
	        var username = $("txtUsername");
	        var password = $("txtPass");
	        var checkcode = $("CheckCode");
	        if (isEmptyStr(username.value)) {
	            alert("�������û�����");
	            username.focus();
	            return false;
	        }
	        if (isEmptyStr(password.value)) {
	            alert("���������룡");
	            password.focus();
	            return false;
	        }

	        if (isEmptyStr(checkcode.value)) {
	            alert("��������֤�룡");
	            checkcode.focus();
	            return false;
	        }
	        return true;
	    }



	    function focusNext(nextName, evt, num, t, lastName) {
	        evt = (evt) ? evt : event;
	        var charCode = (evt.charCode) ? evt.charCode :
((evt.which) ? evt.which : evt.keyCode);
	        if (charCode == 13 || charCode == 3) {
	            var nextobj = document.getElementById(nextName);
	            var lastobj = document.getElementById(lastName);

	            if (num == 1 && isEmptyStr(t.value)) {
	                alert("�������û�����");
	                t.focus();
	                return false;
	            }


	            if (num == 2 && isEmptyStr(t.value)) {
	                alert("���������룡");
	                t.focus();
	                return false;
	            }

	            if (lastobj != null && isEmptyStr(lastobj.value)) {
	                alert("�������û�����");
	                lastobj.focus();
	                return false;
	            }

	            nextobj.focus();
	            return false;
	        }
	        return true;
	    }


    </script>

    </script>
</head>
<body  leftmargin="0" topmargin="0" marginheight="0" marginwidth="0">
    <form id="Form1" method="post" runat="server">   
    <br>
    <br>
    <br>
    <br>
    <br>
    <table width="620" border="0" align="center" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td width="620">
                    <img height="11" src="images/login_p_img02.gif" width="650">
                </td>
            </tr>
            <tr>
                <td align="center" background="images/login_p_img03.gif">
                    <br>
                    <table width="570" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="570" border="0">
                                    <tbody>
                                        <tr>
                                            <td width="245" height="80" align="center" valign="top">
                                                <img height="67" src="images/member_t04.jpg" width="245">
                                            </td>
                                            <td align="right" valign="top">
                                                <br>
                                                &nbsp;
                                                <img height="9" src="images/point07.gif" width="13" border="0"><a href="#" onclick="window.external.addFavorite('http://www.maticsoft.com','�������ϵͳ')">�����ղ�</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;<asp:Label ID="lblGUID" runat="server" Style="display: none"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="images/a_te01.gif" width="570" height="3">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" background="images/a_te02.gif">
                                <table width="520" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="123" height="120">
                                            <img height="95" src="images/login_p_img05.gif" width="123" border="0">
                                        </td>
                                        <td align="center">
                                            <table cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td width="210" height="25" valign="top">
                                                        �û�����
                                                        <input class="nemo01" tabindex="1" maxlength="22" size="22" name="user" id="txtUsername"
                                                            runat="server" onkeypress="return focusNext('txtPass', event,1,this,null)">
                                                    </td>
                                                    <td width="80" rowspan="3" align="right" valign="middle">
                                                        <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="images/login_p_img11.gif">
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="bottom" height="12">
                                                        ��&nbsp;&nbsp; �룺
                                                        <input name="user" type="password" class="nemo01" tabindex="1" size="22" maxlength="22"
                                                            id="txtPass" runat="server" onkeypress="return focusNext('CheckCode', event,2,this,'txtUsername')">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="bottom" height="13">
                                                        <table width="100%%" height="25" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="70%" align="left">
                                                                    ��֤�룺
                                                                    <input class="nemo01" id="CheckCode" tabindex="3" maxlength="22" size="11" name="user"
                                                                        runat="server">
                                                                </td>
                                                                <td width="30%" align="left">
                                                                    <a id="A2" href="" onclick="ChangeCode();return false;">
                                                                        <asp:Image ID="ImageCheck" runat="server" ImageUrl="../ValidateCode.aspx?GUID=GUID"
                                                                            ImageAlign="AbsMiddle" ToolTip="�����壬��һ��"></asp:Image></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br>
                                            <asp:Label ID="lblMsg" runat="server" BackColor="Transparent" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#d5d5d5">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="70" align="center">
                                            Copyright(C) 2004-2008 Maticsoft All Rights Reserved.
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <img height="11" src="images/login_p_img04.gif" width="650">
                </td>
            </tr>
        </tbody>
    </table>
    <br>
    </form>
</body>
</html>
