using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Maticsoft.CodeHelper;
using Maticsoft.Utility;
namespace Maticsoft.BuilderModel
{
    /// <summary>
    /// ���ӱ��model����
    /// </summary>
    public class BuilderModelT : BuilderModel
    {
        #region  ��������
        private string _modelnameson=""; //model����        
        public string ModelNameSon
        {
            set { _modelnameson = value; }
            get { return _modelnameson; }
        }
        
        #endregion

        public BuilderModelT()
        {
        }     

        #region ������������Model��

        /// <summary>
        /// ��������Model��
        /// </summary>		
        public string CreatModelMethodT()
        {            
            StringPlus strclass = new StringPlus();         
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(2, "private List<" + ModelNameSon + "> _" + ModelNameSon.ToLower() + "s;");//˽�б���
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// ��Model��: ���� ");     
            strclass.AppendSpaceLine(2, "/// </summary>");
            //strclass.AppendSpaceLine(2, "[Serializable]");
            strclass.AppendSpaceLine(2, "public List<" + ModelNameSon + "> " + ModelNameSon + "s");//����
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "set{" + " _" + ModelNameSon.ToLower() + "s=value;}");
            strclass.AppendSpaceLine(3, "get{return " + "_" + ModelNameSon.ToLower() + "s;}");
            strclass.AppendSpaceLine(2, "}");

            if (Modelpath.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", Modelpath.Split('.')[0]);
            }
        }
        #endregion
                
    }
}
