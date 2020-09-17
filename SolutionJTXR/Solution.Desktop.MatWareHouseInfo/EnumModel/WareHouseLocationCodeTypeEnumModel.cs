using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class WareHouseLocationCodeTypeEnumModel
    {
        public enum WareHouseLocationCodeType
        {

            [Description("区域名_列_层_命名方式")]
            WareHouseLocationCodeType01 = 1,

            [Description("序号_命名方式")]
            WareHouseLocationCodeType02 = 2,
        }
    }
}
