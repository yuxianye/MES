using Solution.Desktop.MaterialOutStorageInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;

namespace Solution.Desktop.MaterialOutStorageInfo.ViewModel
{
    public static class MaterialOutStorageInfoSetPalletQuantity
    {
        public static void SetPalletQuantity(MaterialOutStorageInfoModel materialoutstorageInfo)
        {
            //成品手动出库
            if (materialoutstorageInfo.Quantity != null &&
                materialoutstorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.ProductManuallyOutStorageType)
            {
                if (materialoutstorageInfo.FullPalletQuantity != null && materialoutstorageInfo.FullPalletQuantity.Value != 0)
                {
                    decimal dPalletQuantity = materialoutstorageInfo.Quantity.Value / materialoutstorageInfo.FullPalletQuantity.Value;
                    if (dPalletQuantity < 1 && dPalletQuantity > 0 )
                    {
                        dPalletQuantity = 1;
                    }
                    dPalletQuantity = decimal.Ceiling(dPalletQuantity);
                    //
                    materialoutstorageInfo.PalletQuantity = dPalletQuantity;
                }
            }
        }
    }
}
