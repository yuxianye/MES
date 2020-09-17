using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Solution.Utility.Extensions;
using System.Windows.Controls;
using System.Windows;
using Solution.Desktop.Model;
using Solution.Desktop.Core;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
   public static class BillCodeUtility
    {
        public static string getInStorageBillCode()
        {
            string sCode = "";
            //
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MaterialInStorageInfo/GetInStorageBillCode");
            //
            if (result != null && result.Data != null  && result.Data.Any())
            {
                var CodeList = result.Data;
                //
                if (CodeList != null && CodeList.Count > 0)
                {
                    sCode = CodeList.FirstOrDefault();
                }
            }
            return sCode;
        }

        public static string getOutStorageBillCode()
        {
            string sCode = "";
            //
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                            (GlobalData.ServerRootUri + "MaterialOutStorageInfo/GetOutStorageBillCode");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                var CodeList = result.Data;
                //
                if (CodeList != null && CodeList.Count > 0)
                {
                    sCode = CodeList.FirstOrDefault();
                }
            }
            return sCode;
        }

        public static string getInventoryCode()
        {
            string sCode = "";
            //
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatInventoryInfo/GetInventoryCode");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                var CodeList = result.Data;
                //
                if (CodeList != null && CodeList.Count > 0)
                {
                    sCode = CodeList.FirstOrDefault();
                }
            }
            return sCode;
        }

        public static string getStorageMoveCode()
        {
            string sCode = "";
            //
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatStorageMoveInfo/GetStorageMoveCode");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                var CodeList = result.Data;
                //
                if (CodeList != null && CodeList.Count > 0)
                {
                    sCode = CodeList.FirstOrDefault();
                }
            }
            return sCode;
        }

        public static string getStorageModifyCode()
        {
            string sCode = "";
            //
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatStorageModifyInfo/GetStorageModifyCode");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                var CodeList = result.Data;
                //
                if (CodeList != null && CodeList.Count > 0)
                {
                    sCode = CodeList.FirstOrDefault();
                }
            }
            return sCode;
        }

    }
}
