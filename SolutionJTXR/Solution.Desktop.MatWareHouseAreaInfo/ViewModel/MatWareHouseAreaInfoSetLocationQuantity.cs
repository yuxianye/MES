using Solution.Desktop.MatWareHouseAreaInfo.Model;

namespace Solution.Desktop.MatWareHouseAreaInfo.ViewModel
{
    public static class MatWareHouseAreaInfoSetLocationQuantity
    {
        public static void SetLocationQuantity(MatWareHouseAreaInfoModel matwarehouseareaInfo)
        {
            if ( matwarehouseareaInfo.ColumnNumber != null &&
                 matwarehouseareaInfo.LayerNumber != null )
            {
                int dLocationQuantity = matwarehouseareaInfo.ColumnNumber.Value * matwarehouseareaInfo.LayerNumber.Value;
                //
                matwarehouseareaInfo.LocationQuantity = dLocationQuantity;
            }
        }
    }
}
