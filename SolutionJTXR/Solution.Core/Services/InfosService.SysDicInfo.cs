using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Core.Data;
using OSharp.Core.Mapping;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Infos;
using Solution.Core.Models.Infos;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;

namespace Solution.Core.Services
{
  public   partial class InfosService
    {

        
    public IQueryable<SysDicInfo> SysDicInfo
    {
        get
        {
            return SysDicInfoRepository.Entities;
        }
    }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public bool AddDic(SysDicInfo dic)
        {

            try
            {
                dic.Id = CombHelper.NewComb();
                dic.CreatedTime = DateTime.Now;
                if (dic.DicParentID == null) { dic.DicParentID = Guid.Empty; }
                SysDicInfoRepository.Insert(dic);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 修改一个字典
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool EditDic(SysDicInfo dic)
        {

            try
            {
                SysDicInfo old_Dic = SysDicInfo.FirstOrDefault(d => d.Id == dic.Id);
                old_Dic.DicCode = dic.DicCode;
                old_Dic.DicName = dic.DicName;
                old_Dic.DicParentID = dic.DicParentID;
                old_Dic.DicLevel = dic.DicLevel;
                old_Dic.DicType = dic.DicType;
                old_Dic.DicValue = dic.DicValue;
                old_Dic.Remark = dic.Remark;
                old_Dic.DicSetValue = dic.DicSetValue;
                SysDicInfoRepository.Update(old_Dic);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 删除一个字典
        /// </summary>
        /// <param name="dic">对象中RoleID有值即可</param>
        /// <returns></returns>
        public bool DeletDic(string id)
        {
            try
            {
                SysDicInfoRepository.DeleteDirect(new Guid(id));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取字典节点
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<SysDicInfo> GetDicNode(string pid)
        {
            if (string.IsNullOrEmpty(pid)) { pid = Guid.Empty.ToString(); }
            return SysDicInfo.Where(a => a.DicParentID.Value.ToString() == pid).OrderBy(x=>x.CreatedTime).ToList();
        }
        /// <summary>
        /// 获取字典分页
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public object GetDicList(string typeid, int page = 1, int rows = 10)
        {
            if (string.IsNullOrEmpty(typeid)) { typeid = Guid.Empty.ToString(); }
            try
            {
                int count = SysDicInfo.Where(a => a.DicParentID.Value.ToString() == typeid).Count();
                List<SysDicInfo> lt_data = SysDicInfo.Where(a => a.DicParentID.Value.ToString() == typeid).OrderBy(x => x.CreatedTime).Skip((page - 1) * rows).Take(rows).ToList();
                return new { rows = lt_data, total = count };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取某一字典信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDicInfo GetDic(string id)
        {
            return SysDicInfo.FirstOrDefault(a => a.Id == new Guid(id));
        }
    }
}
