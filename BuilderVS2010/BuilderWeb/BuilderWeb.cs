using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Maticsoft.IDBO;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;

namespace Maticsoft.BuilderWeb
{
    /// <summary>
    /// Web��������
    /// </summary>
    public class BuilderWeb : IBuilder.IBuilderWeb
    {
        #region ˽���ֶ�
        protected string _key = "ID";//Ĭ�ϵ�һ�������ֶ�		
        protected string _keyType = "int";//Ĭ�ϵ�һ����������        
        protected string _namespace = "Maticsoft"; //���������ռ���
        private string _folder = "";//�����ļ���
        protected string _modelname; //model����           
        protected string _bllname; //model����
        protected List<ColumnInfo> _fieldlist;
        protected List<ColumnInfo> _keys;
        #endregion

        #region ��������
        /// <summary>
        /// ���������ռ��� 
        /// </summary>        
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// �����ļ�����
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }
        /// <summary>
        /// Model����
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// BLL����
        /// </summary>
        public string BLLName
        {
            set { _bllname = value; }
            get { return _bllname; }
        }

        /// <summary>
        /// ʵ��������������ռ�+����
        /// </summary>
        public string ModelSpace
        {
            get
            {
                string _modelspace = _namespace + "." + "Model";
                if (_folder.Trim() != "")
                {
                    _modelspace += "." + _folder;
                }
                _modelspace += "." + ModelName;
                return _modelspace;
            }
        }

        /// <summary>
        /// ҵ���߼���Ĳ��������ƶ���
        /// </summary>
        private string BLLSpace
        {
            get
            {
                string _bllspace = _namespace + "." + "BLL";
                if (_folder.Trim() != "")
                {
                    _bllspace += "." + _folder;
                }
                _bllspace += "." + BLLName;
                return _bllspace;
            }
        }
        /// <summary>
        /// ѡ����ֶμ���
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// �����������ֶ��б� 
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// ������ʶ�ֶ�
        /// </summary>
        protected string Key
        {
            get
            {
                foreach (ColumnInfo key in _keys)
                {
                    _key = key.ColumnName;
                    _keyType = key.TypeName;
                    if (key.IsIdentity)
                    {
                        _key = key.ColumnName;
                        _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                        break;
                    }
                }
                return _key;
            }
        }
        #endregion

        public BuilderWeb()
        {
        }

        #region ��������



        /// <summary>
        /// ����һЩ����Ҫ��ʾ���У�����ҳ���޸�ҳ���б�ҳ��
        /// </summary>
        /// <param name="columnName"></param>
        private bool isFilterColume(string columnName)
        {
            //���Զ���-�����ֶδ���
            //if (
            //        (columnName.IndexOf("_iCreator") > 0) ||  //ҳ�治��Ҫ��4��
            //        (columnName.IndexOf("_dateCreate") > 0) ||
            //        (columnName.IndexOf("_iMaintainer") > 0) ||
            //        (columnName.IndexOf("_dateMaintain") > 0) ||
            //        (columnName.IndexOf("_bValid") > 0) ||
            //        (columnName.IndexOf("_dateValid") > 0) ||
            //        (columnName.IndexOf("_dateExpire") > 0)
            //        )
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;

        }

        #endregion

        #region Aspxҳ��html

        /// <summary>
        /// �õ���ʾ�����Ӵ����html����
        /// </summary>      
        public string GetAddAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            bool hasDate = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if (IsIdentity)
                {
                    continue;
                }

                if (isFilterColume(columnName))
                {
                    continue;
                }
                if (columnType.Trim().ToLower() == "uniqueidentifier")
                {
                    continue;
                }

                deText = CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                strclass.AppendSpaceLine(1, "<tr>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\" class=\"td_class\">");
                strclass.AppendSpaceLine(2, deText);
                strclass.AppendSpaceLine(1, "��</td>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\" class=\"td_width\">");
                
                
                switch (columnType.Trim().ToLower())
                {
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "<asp:TextBox ID=\"txt" + columnName + "\" runat=\"server\" Width=\"70px\"  onfocus=\"WdatePicker()\"></asp:TextBox>");
                        hasDate = true;
                        break;
                    case "bit":
                        strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
                        break;
                    case "uniqueidentifier":
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
                        break;
                }
                strclass.AppendSpaceLine(1, "</td></tr>");
            } 
            strclass.AppendSpaceLine(1,"<tr>");
            strclass.AppendSpaceLine(1,"<td class=\"td_class\">");
            strclass.AppendSpaceLine(1,"</td>");
            strclass.AppendSpaceLine(1,"<td height=\"25\">");
            strclass.AppendSpaceLine(1,"<asp:Button ID=\"btnSave\" runat=\"server\" Text=\"<%$ Resources:Site, btnSaveText %>\"");
            strclass.AppendSpaceLine(1,"OnClick=\"btnSave_Click\" class=\"adminsubmit\"></asp:Button>");
            strclass.AppendSpaceLine(1,"<asp:Button ID=\"btnCancle\" runat=\"server\" CausesValidation=\"false\" Text=\"<%$ Resources:Site, btnCancleText %>\"");
            strclass.AppendSpaceLine(1,"OnClick=\"btnCancle_Click\" class=\"adminsubmit\"></asp:Button>");
            strclass.AppendSpaceLine(1,"</td>");
            strclass.AppendSpaceLine(1,"</tr>");                      
            strclass.AppendLine("</table>");
            if (hasDate)
            {
                strclass.AppendLine("<script src=\"/scripts/Calendar3/WdatePicker.js\" type=\"text/javascript\"></script>");
            }
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }

        }

        /// <summary>
        /// �õ���ʾ�����Ӵ����html����
        /// </summary>      
        public string GetUpdateAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            bool hasDate = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);
                if (isFilterColume(columnName))
                {
                    continue;
                }

                if ((ispk) || (IsIdentity) || (columnType.Trim().ToLower() == "uniqueidentifier"))
                {
                    strclass.AppendSpaceLine(1, "<tr>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\" class=\"td_class\">");
                    strclass.AppendSpaceLine(2, deText);
                    strclass.AppendSpaceLine(1, "��</td>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\" class=\"td_width\">");
                    strclass.AppendSpaceLine(2, "<asp:label id=\"lbl" + columnName + "\" runat=\"server\"></asp:label>");
                    strclass.AppendSpaceLine(1, "</td></tr>");
                }
                else
                {
                    //
                    strclass.AppendSpaceLine(1, "<tr>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\" class=\"td_class\">");
                    strclass.AppendSpaceLine(2, deText);
                    strclass.AppendSpaceLine(1, "��</td>");
                    strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\" class=\"td_width\">");
                                                           
                    switch (columnType.Trim().ToLower())
                    {
                        case "datetime":
                        case "smalldatetime":
                            //strclass.AppendSpaceLine(2, "<INPUT onselectstart=\"return false;\" onkeypress=\"return false\" id=\"txt" + columnName + "\" onfocus=\"setday(this)\"");
                            //strclass.AppendSpaceLine(2, " readOnly type=\"text\" size=\"10\" name=\"Text1\" runat=\"server\">");
                            strclass.AppendSpaceLine(2, "<asp:TextBox ID=\"txt" + columnName + "\" runat=\"server\" Width=\"70px\"  onfocus=\"WdatePicker()\"></asp:TextBox>");
                            hasDate = true;
                            break;
                        case "bit":
                            strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />");
                            break;
                        default:
                            strclass.AppendSpaceLine(2, "<asp:TextBox id=\"txt" + columnName + "\" runat=\"server\" Width=\"200px\"></asp:TextBox>");
                            break;
                    }
                    strclass.AppendSpaceLine(1, "</td></tr>");
                }
            }
            strclass.AppendSpaceLine(1, "<tr>");
            strclass.AppendSpaceLine(1, "<td class=\"td_class\">");
            strclass.AppendSpaceLine(1, "</td>");
            strclass.AppendSpaceLine(1, "<td height=\"25\">");
            strclass.AppendSpaceLine(1, "<asp:Button ID=\"btnSave\" runat=\"server\" Text=\"<%$ Resources:Site, btnSaveText %>\"");
            strclass.AppendSpaceLine(1, "OnClick=\"btnSave_Click\" class=\"adminsubmit\"></asp:Button>");
            strclass.AppendSpaceLine(1, "<asp:Button ID=\"btnCancle\" runat=\"server\" CausesValidation=\"false\" Text=\"<%$ Resources:Site, btnCancleText %>\"");
            strclass.AppendSpaceLine(1, "OnClick=\"btnCancle_Click\" class=\"adminsubmit\"></asp:Button>");
            strclass.AppendSpaceLine(1, "</td>");
            strclass.AppendSpaceLine(1, "</tr>");
            
            strclass.AppendLine("</table>");
            if (hasDate)
            {
                strclass.AppendLine("<script src=\"/scripts/Calendar3/WdatePicker.js\" type=\"text/javascript\"></script>");
            }
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ���ʾ����ʾ�����html����
        /// </summary>     
        public string GetShowAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendLine("<table cellSpacing=\"0\" cellPadding=\"0\" width=\"100%\" border=\"0\">");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

               
                strclass.AppendSpaceLine(1, "<tr>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"30%\" align=\"right\" class=\"td_class\">");
                strclass.AppendSpaceLine(2, deText);
                strclass.AppendSpaceLine(1, "��</td>");
                strclass.AppendSpaceLine(1, "<td height=\"25\" width=\"*\" align=\"left\" class=\"td_width\">");
                switch (columnType.Trim().ToLower())
                {
                    //case "bit":
                    //    strclass.AppendSpaceLine(2, "<asp:CheckBox ID=\"chk" + columnName + "\" Text=\"" + deText + "\" runat=\"server\" Checked=\"False\" />" );
                    //    break;
                    default:
                        strclass.AppendSpaceLine(2, "<asp:Label id=\"lbl" + columnName + "\" runat=\"server\"></asp:Label>");
                        break;
                }
                strclass.AppendSpaceLine(1, "</td></tr>");
            }
            strclass.AppendSpaceLine(1,"<tr>");
            strclass.AppendSpaceLine(1,"<td class=\"td_classshow\">");
            strclass.AppendSpaceLine(1,"</td>");
            strclass.AppendSpaceLine(1,"<td height=\"25\">");
            strclass.AppendSpaceLine(1,"<asp:Button ID=\"btnCancle\" runat=\"server\" CausesValidation=\"false\" Text=\"<%$Resources:Site,btnBackText%>\" class=\"adminsubmit_short\" OnClick=\"btnCancle_Click\"  OnClientClick=\"javascript:parent.$.colorbox.close();\"></asp:Button>");
            strclass.AppendSpaceLine(1,"</td>");
            strclass.AppendSpaceLine(1,"</tr>");
            strclass.AppendLine("</table>");
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ���ʾ���б����html����
        /// </summary>     
        public string GetListAspx()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                if (IsIdentity)
                {
                    continue;
                }

                if (isFilterColume(columnName))
                {
                    continue;
                }                               

                switch (columnType.Trim().ToLower())
                {
                    case "bit":
                    {
                        strclass.AppendSpaceLine(2, "<%--ģ���в������ٷ���--%><%--<asp:BoundField DataField=\"" + columnName + "\" HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\"  />--%> ");
                        strclass.AppendSpaceLine(2, "<asp:TemplateField HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\">");
                        strclass.AppendSpaceLine(3, "<ItemTemplate>");
                        strclass.AppendSpaceLine(4, "<%#Eval(\"" + columnName + "\").ToString().ToLower() == \"true\" ? \"��\" : \"��\"%>");
                        strclass.AppendSpaceLine(3, "</ItemTemplate>");
                        strclass.AppendSpaceLine(2, "</asp:TemplateField>");
                    }
                        break;
                    case "dateTime":
                        strclass.AppendSpaceLine(2, "<%--ģ���в������ٷ���--%><%--<asp:BoundField DataField=\"" + columnName + "\" HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\"  />--%> ");
                        strclass.AppendSpaceLine(2, "<asp:TemplateField HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\">");
                        strclass.AppendSpaceLine(3, "<ItemTemplate>");
                        strclass.AppendSpaceLine(4, "<%#Eval(\"" + columnName + "\")%>");
                        strclass.AppendSpaceLine(3, "</ItemTemplate>");
                        strclass.AppendSpaceLine(2, "</asp:TemplateField>");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "<%--ģ���в������ٷ���--%><%--<asp:BoundField DataField=\"" + columnName + "\" HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\"  />--%> ");
                        strclass.AppendSpaceLine(2, "<asp:TemplateField HeaderText=\"" + deText + "\" SortExpression=\"" + columnName + "\" ItemStyle-HorizontalAlign=\"Center\">");
                        strclass.AppendSpaceLine(3, "<ItemTemplate>");
                        strclass.AppendSpaceLine(4, "<%#Eval(\"" + columnName + "\")%>");
                        strclass.AppendSpaceLine(3, "</ItemTemplate>");
                        strclass.AppendSpaceLine(2, "</asp:TemplateField>");
                        break;
                }
            }
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }


        /// <summary>
        /// ��ɾ��3��ҳ�����
        /// </summary>      
        public string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine(" <!--******************************����ҳ�����********************************-->");
                strclass.AppendLine(GetAddAspx());
            }
            if (UpdateForm)
            {
                strclass.AppendLine(" <!--******************************�޸�ҳ�����********************************-->");
                strclass.AppendLine(GetUpdateAspx());
            }
            if (ShowForm)
            {
                strclass.AppendLine("  <!--******************************��ʾҳ�����********************************-->");
                strclass.AppendLine(GetShowAspx());
            }
            if (SearchForm)
            {
                strclass.AppendLine("  <!--******************************�б�ҳ�����********************************-->");
                strclass.AppendLine(GetListAspx());
            }
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }
        #endregion

        #region ��ʾ�� CS

        /// <summary>
        /// ���ɱ�ʾ��ҳ���CS����
        /// </summary>
        /// <param name="ExistsKey"></param>
        /// <param name="AddForm">�Ƿ��������Ӵ���Ĵ���</param>
        /// <param name="UpdateForm">�Ƿ������޸Ĵ���Ĵ���</param>
        /// <param name="ShowForm">�Ƿ�������ʾ����Ĵ���</param>
        /// <param name="SearchForm">�Ƿ����ɲ�ѯ����Ĵ���</param>
        /// <returns></returns>
        public string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine("  /******************************���Ӵ������********************************/");
                strclass.AppendLine(GetAddAspxCs());
            }
            if (UpdateForm)
            {
                strclass.AppendLine("  /******************************�޸Ĵ������********************************/");
                strclass.AppendLine("  /*�޸Ĵ���-��ʾ */");
                strclass.AppendLine(GetUpdateShowAspxCs());
                strclass.AppendLine("  /*�޸Ĵ���-�ύ���� */");
                strclass.AppendLine(GetUpdateAspxCs());
            }
            if (ShowForm)
            {
                strclass.AppendLine("  /******************************��ʾ�������********************************/");
                strclass.AppendLine(GetShowAspxCs());
            }
            //if (DelForm)
            //{
            //    strclass.Append("  /******************************ɾ���������********************************/");
            //    strclass.Append("");
            //    strclass.Append(CreatDeleteForm());
            //}
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ���ʾ�����Ӵ���Ĵ���
        /// </summary>      
        public string GetAddAspxCs()
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass0 = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, "string strErr=\"\";");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if ((IsIdentity))
                {
                    continue;
                }
                if ("uniqueidentifier" == columnType.ToLower())
                {
                    continue;
                }
                //���Զ���-�����ֶδ���
                //if ((!ishasuser) && ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0)))
                //{
                //    strclass0.AppendSpaceLine(3, "User currentUser;");
                //    strclass0.AppendSpaceLine(3, "if (Session[\"UserInfo\"] != null)");
                //    strclass0.AppendSpaceLine(3, "{");
                //    strclass0.AppendSpaceLine(4, "currentUser = (User)Session[\"UserInfo\"];");
                //    strclass0.AppendSpaceLine(3, "}else{");
                //    strclass0.AppendSpaceLine(4, "return;");
                //    strclass0.AppendSpaceLine(3, "}");
                //    ishasuser = true;
                //}

                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

               
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                       strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");
                        break;
                    case "datetime":
                    case "smalldatetime":
                        strclass0.AppendSpaceLine(3, "DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");

                        break;
                    case "bool":
                        strclass0.AppendSpaceLine(3, "bool " + columnName + "=this.chk" + columnName + ".Checked;");
                        break;
                    case "byte[]":
                        strclass0.AppendSpaceLine(3, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        break;
                    case "long":
                        strclass0.AppendSpaceLine(3, "long " + columnName + "=long.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");
                        
                        break;
                    default:
                        strclass0.AppendSpaceLine(3, "string " + columnName + "=this.txt" + columnName + ".Text;");
                        strclass1.AppendSpaceLine(3, "if(this.txt" + columnName + ".Text.Trim().Length==0)");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "����Ϊ�գ�\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");
                        break;
                }
                strclass2.AppendSpaceLine(3, "model." + columnName + "=" + columnName + ";");
            }
            strclass.AppendLine(strclass1.ToString());
            strclass.AppendSpaceLine(3, "if(strErr!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "MessageBox.ShowFailTip(this,strErr);");
            strclass.AppendSpaceLine(4, "return;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendLine(strclass0.ToString());
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendLine(strclass2.ToString());
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(3, "bll.Add(model);");
            strclass.AppendSpaceLine(3, "Maticsoft.Common.MessageBox.ShowSuccessTip(this,\"����ɹ���\",\"add.aspx\");");
            strclass.AppendSpaceLine(3, "/*������־*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format(\"Add����{0}��\", new JavaScriptSerializer().Serialize(model)), this);");
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ��޸Ĵ���Ĵ���
        /// </summary>      
        public string GetUpdateAspxCs()
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass0 = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, "string strErr=\"\";");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;

                //���Զ���-�����ֶδ���
                //if (
                //    (columnName.IndexOf("_iCreator") > 0) ||  //ҳ�治��Ҫ��2��
                //    (columnName.IndexOf("_dateCreate") > 0) ||
                //    (columnName.IndexOf("_bValid") > 0) ||
                //    (columnName.IndexOf("_dateValid") > 0) ||
                //    (columnName.IndexOf("_dateExpire") > 0)
                //    )
                //{
                //    continue;
                //}
                //if ((!ishasuser) && (columnName.IndexOf("_iMaintainer") > 0))
                //{
                //    strclass0.AppendSpaceLine(4, "User currentUser;");
                //    strclass0.AppendSpaceLine(3, "if (Session[\"UserInfo\"] != null)");
                //    strclass0.AppendSpaceLine(3, "{");
                //    strclass0.AppendSpaceLine(4, "currentUser = (User)Session[\"UserInfo\"];");
                //    strclass0.AppendSpaceLine(3, "}else{");
                //    strclass0.AppendSpaceLine(4, "return;");
                //    strclass0.AppendSpaceLine(3, "}");
                //    ishasuser = true;
                //}


                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                //���Զ���-�����ֶδ���
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))//���Դ���
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistLanguage1.LanguageCode;");
                //    continue;
                //}
                //if (columnName.IndexOf("_iAuthority") > 0)//Ȩ�޽�ɫ����
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistPermission1.PermissionID;");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrency") > 0)//���Ҵ���
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistCurrency1.CurrencyCode;");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrencyUnit") > 0)//���Ҵ���
                //{
                //    strclass2.AppendSpaceLine(3, "model." + columnName + "= UCDroplistCurrencyUnit1.CurrencyUnitID;");
                //    continue;
                //}

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "smallint":
                        if ((ispk) || (IsIdentity))//���Ϊ����
                        {
                            strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(Request.Params[\"id\"]);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "int " + columnName + "=int.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "long":                    
                        if ((ispk) || (IsIdentity))//���Ϊ����
                        {
                            strclass0.AppendSpaceLine(3, "long " + columnName + "=long.Parse(Request.Params[\"id\"]);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "long " + columnName + "=long.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsNumber(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "float":
                    case "numeric":
                    case "decimal":
                        if ((ispk) || (IsIdentity))//���Ϊ����
                        {
                            strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(Request.Params[\"id\"]);");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "decimal " + columnName + "=decimal.Parse(this.txt" + columnName + ".Text);");
                            strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDecimal(txt" + columnName + ".Text))");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "datetime":
                    case "smalldatetime":                        
                        strclass0.AppendSpaceLine(3, "DateTime " + columnName + "=DateTime.Parse(this.txt" + columnName + ".Text);");
                        strclass1.AppendSpaceLine(3, "if(!PageValidate.IsDateTime(txt" + columnName + ".Text))");
                        strclass1.AppendSpaceLine(3, "{");
                        strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "��ʽ����\\\\n\";	");
                        strclass1.AppendSpaceLine(3, "}");

                        break;
                    case "bool":
                        strclass0.AppendSpaceLine(3, "bool " + columnName + "=this.chk" + columnName + ".Checked;");
                        break;
                    case "byte[]":
                        strclass0.AppendSpaceLine(3, "byte[] " + columnName + "= new UnicodeEncoding().GetBytes(this.txt" + columnName + ".Text);");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass0.AppendSpaceLine(3, "Guid " + columnName + "= new Guid(this.lbl" + columnName + ".Text);");
                        break;
                    default:
                        if ((ispk) || (IsIdentity))//���Ϊ����
                        {
                            strclass0.AppendSpaceLine(3, "string " + columnName + "=Request.Params[\"id\"]");
                        }
                        else
                        {
                            strclass0.AppendSpaceLine(3, "string " + columnName + "=this.txt" + columnName + ".Text;");
                            strclass1.AppendSpaceLine(3, "if(this.txt" + columnName + ".Text.Trim().Length==0)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "strErr+=\"" + deText + "����Ϊ�գ�\\\\n\";	");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                }
                strclass2.AppendSpaceLine(3, "model." + columnName + "=" + columnName + ";");

            }
            strclass.AppendLine(strclass1.ToString());
            strclass.AppendSpaceLine(3, "if(strErr!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "MessageBox.ShowFailTip(this,strErr);");
            strclass.AppendSpaceLine(4, "return;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendLine(strclass0.ToString());
            strclass.AppendLine();
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            //strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendSpaceLine(3, ModelSpace + " model=bll.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");//���µ�ʱ���ٲ�ѯһ�Σ���ҵ����Ҫ��ֱ��newһ��ʵ���Լ������");
            strclass.AppendSpaceLine(3, "/*������־*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format(\"Modify-Old����{0}��\", new JavaScriptSerializer().Serialize(model)), this);");
            strclass.AppendLine(strclass2.ToString());
            strclass.AppendSpaceLine(3, "bll.Update(model);");
            strclass.AppendSpaceLine(3, "Maticsoft.Common.MessageBox.ShowSuccessTip(this,\"����ɹ���\",\"list.aspx\");");
            strclass.AppendSpaceLine(3, "/*������־*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format(\"Modify-New����{0}��\", new JavaScriptSerializer().Serialize(model)), this);");
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ��޸Ĵ���Ĵ���
        /// </summary>       
        public string GetUpdateShowAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            string key = Key;
            strclass.AppendSpaceLine(1, "private void ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(2, ModelSpace + " model=bll.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;

                if (isFilterColume(columnName))
                {
                    continue;
                }

                //���Զ���-�����ֶδ���
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistLanguage1.LanguageCode =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_iAuthority") > 0)
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistPermission1.PermissionID =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrency") > 0)//���Ҵ���
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistCurrency1.CurrencyCode =model." + columnName + ";");
                //    continue;
                //}
                //if (columnName.IndexOf("_cCurrencyUnit") > 0)//���Ҵ���
                //{
                //    strclass.AppendSpaceLine(2, "UCDroplistCurrencyUnit1.CurrencyUnitID =model." + columnName + ";");
                //    continue;
                //}

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "long":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        }
                        else
                        {
                            strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ".ToString();");
                        }
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "this.chk" + columnName + ".Checked=model." + columnName + ";");
                        break;
                    case "byte[]":
                        strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    default:
                        if ((ispk) || (IsIdentity))
                        {
                            strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ";");
                        }
                        else
                        {
                            strclass.AppendSpaceLine(2, "this.txt" + columnName + ".Text=model." + columnName + ";");
                        }
                        break;
                }
            }
            strclass.AppendLine();
            strclass.AppendSpaceLine(1, "}");
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }


        /// <summary>
        /// �õ���ʾ����ʾ����Ĵ���
        /// </summary>       
        public string GetShowAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            string key = Key;
            strclass.AppendSpaceLine(1, "private void ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, BLLSpace + " bll=new " + BLLSpace + "();");
            strclass.AppendSpaceLine(2, ModelSpace + " model=bll.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            //bool ishasuser = false;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                //if ((ispk) || (IsIdentity))
                //{
                //    continue;
                //}

                #region �����ֶδ���
                //���Զ���-�����ֶδ���
                //if (columnName.IndexOf("_iAuthority") > 0)
                //{
                //    continue;
                //}
                //if ((columnName.IndexOf("_cLang") > 0) && (columnType.Trim().ToLower() == "varchar"))//���Դ���
                //{
                //    strclass.AppendSpaceLine(2, "BLL.SysManage.MultiLanguage bllML = new BLL.SysManage.MultiLanguage();");
                //    strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text= bllML.GetLanguageNameByCache(model." + columnName + ");");
                //    continue;
                //}
                //if ((!ishasuser) && ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0)))
                //{
                //    strclass.AppendSpaceLine(2, "Maticsoft.Accounts.Bus.User user = new Maticsoft.Accounts.Bus.User();");
                //    ishasuser = true;
                //}
                //if ((columnName.IndexOf("_iCreator") > 0) || (columnName.IndexOf("_iMaintainer") > 0))
                //{
                //    strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text= user.GetTrueNameByCache(model." + columnName + ");");
                //    ishasuser = true;
                //    continue;
                //}
                #endregion

                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "int":
                    case "long":
                    case "smallint":
                    case "float":
                    case "numeric":
                    case "decimal":
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + "?\"��\":\"��\";");
                        break;
                    case "byte[]":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    case "guid":
                    case "uniqueidentifier":
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ".ToString();");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "this.lbl" + columnName + ".Text=model." + columnName + ";");
                        break;
                }
            }
            strclass.AppendLine();
            strclass.AppendSpaceLine(1, "}");
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// �õ���ʾ���б���Ĵ���
        /// </summary>       
        public string GetListAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpace(2, BLLSpace + " bll = new " + BLLSpace + "();");

            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }
        }

        /// <summary>
        /// ɾ��ҳ��
        /// </summary>
        /// <returns></returns>
        public string GetDeleteAspxCs()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(1, "if(!Page.IsPostBack)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, BLLSpace + " bll=new " + BLLSpace + "();");
            switch (_keyType.Trim())
            {
                case "int":
                case "long":
                case "smallint":
                case "float":
                case "numeric":
                case "decimal":
                case "datetime":
                case "smalldatetime":
                    strclass.AppendSpaceLine(3, _keyType + " " + _key + "=" + _keyType + ".Parse(Request.Params[\"id\"]);");
                    break;
                default:
                    strclass.AppendSpaceLine(3, "string " + _key + "=Request.Params[\"id\"];");
                    break;
            }
            strclass.AppendSpaceLine(3, "/*������־*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format(\"Delete����{0}��\", new JavaScriptSerializer().Serialize(bll.GetModel(" + _key + "))), this);");
            strclass.AppendSpaceLine(3, "bll.Delete(" + _key + ");");
            strclass.AppendSpaceLine(3, "Response.Redirect(\"list.aspx\");");
            strclass.AppendSpaceLine(2, "}");
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }

        }

        public string CreatSearchForm()
        {
            StringPlus strclass = new StringPlus();

            return strclass.Value;
        }



        #endregion//��ʾ��

        #region  ����aspx.designer.cs
        /// <summary>
        /// ���Ӵ����html����
        /// </summary>      
        public string GetAddDesigner()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                if (IsIdentity)
                {
                    continue;
                }
                if (isFilterColume(columnName))
                {
                    continue;
                }
                if ("uniqueidentifier" == columnType.ToLower())
                {
                    continue;
                }
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    case "datetime":
                    case "smalldatetime":
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                        break;
                    case "bool":
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                        break;
                    default:
                        strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                        break;
                }
            }
            //��ť
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnSave;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }

        }

        /// <summary>
        /// �޸Ĵ����html����
        /// </summary>      
        public string GetUpdateDesigner()
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);

                if (isFilterColume(columnName))
                {
                    continue;
                }

                if ((ispk) || (IsIdentity) || (columnType.Trim().ToLower() == "uniqueidentifier"))
                {
                    strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
                }
                else
                {                    
                    switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                    {
                        case "datetime":
                        case "smalldatetime":
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                            break;
                        case "bool":
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                            break;
                        default:
                            strclass.AppendSpaceLine(2, "protected global::System.Web.UI.WebControls.TextBox txt" + columnName + ";");
                            break;
                    }
                }
            }

            //��ť            
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnSave;");
            strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Button btnCancel;");
            //return strclass.Value;
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }

        }

        /// <summary>
        /// ��ʾ�����html����
        /// </summary>     
        public string GetShowDesigner()
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;

                deText = Maticsoft.CodeHelper.CodeCommon.CutDescText(deText, 15, columnName);
                switch (CodeCommon.DbTypeToCS(columnType.Trim().ToLower()).ToLower())
                {
                    //case "bool":
                    //    strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.CheckBox chk" + columnName + ";");
                    //    break;
                    default:
                        strclass.AppendSpaceLine(1, "protected global::System.Web.UI.WebControls.Label lbl" + columnName + ";");
                        break;
                }

            }
            //return strclass.ToString();
            if (NameSpace.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", NameSpace.Split('.')[0]);
            }

        }

        /// <summary>
        /// �б����html����
        /// </summary>     
        public string GetListDesigner()
        {
            StringPlus strclass = new StringPlus();            
            return strclass.ToString();
        }


        #endregion



    }


}
