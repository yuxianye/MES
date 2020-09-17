using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

using Newtonsoft.Json;

using Solution.Core.Contracts;
using Solution.Core.Models.Infos;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Web.Mvc.Security;
using OSharp.Web.Mvc.UI;
using OSharp.Web.Mvc.Extensions;
using System.Threading.Tasks;
using Solution.Core.Dtos.Infos;

using OSharp.Core.Caching;
using OSharp.Core.Data.Extensions;
using OSharp.Core.Security;
using Solution.Web.App.Areas.Admin.Controllers;

namespace Solution.Web.App.Areas.Admin.Controllers
{
    public class DicinfoController :AdminBaseController
    {
        [Description("管理-字典")]
        // GET: Admin/Dicinfo   
        public IInfosContract InfosContract { get; set; }
        public override ActionResult Index()
        {
            return base.View();
        }


        public  ActionResult Form()
        {
            return View();
        }
    
        [HttpPost]
        public JsonResult GetNode(string id)
        {
            var list = InfosContract.GetDicNode(id);
            return Json(list.Select(a => new { id = a.Id, text = a.DicName, state = a.DicType == true ? "closed" : null, iconCls = a.DicType == true ? null : "icon-note" }).ToList());
        }
        [HttpPost]
        public JsonResult GetDicList(int page, int rows, string typeid)
        {
            return Json(InfosContract.GetDicList(typeid, page, rows));
        }

        [HttpPost]
        public JsonResult GetDic(string id)
        {
            return Json(InfosContract.GetDic(id));
        }

        [HttpPost]
        public JsonResult SaveDic(SysDicInfo model)
        {
            bool result = false;
            if (model.Id == Guid.Empty)
            {
                result = InfosContract.AddDic(model);
            }
            else
            {
                result = InfosContract.EditDic(model);
            }
            return Json(new { success = result });
        }


        [HttpPost]
        public JsonResult DelDic(string id)
        {
            bool result = InfosContract.DeletDic(id);
            return Json(new { success = result });
        }
    }
}