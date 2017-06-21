﻿/**  模板制作：Create by Jim
* Delete.cs
*
* 功 能： [N/A]
* 类 名： Delete.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace <$$namespace$$>.Web.Demo
{
    public partial class delete : PageBaseAdmin
    {
        /*权限配置开始*/
        /*请在【功能行为管理】添加相应的数据取得返回的ID，后台配置：【角色】对应->【权限管理】对应->【功能行为管理】*/
        protected new int Act_DelData = 16;//删除数据-单个，如：CMS_内容管理_删除数据
        protected override int Act_PageLoad { get { return Act_DelData; } } //页面权限，如：CMS_内容管理_列表页；注意：这里直接使用按钮权限进行配置，请根据实际情况设置
        /*权限配置结束*/

        protected void Page_Load(object sender, EventArgs e)
        {
            <$$DeleteAspxCs$$>
        }
    }
}