using Solution.Desktop.MaterialOutStorageInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.MaterialOutStorageInfo.ViewModel
{
    public static class MaterialOutStorageInfoSetPalletQuantity
    {
        public static void SetPalletQuantity(MaterialOutStorageInfoModel materialoutstorageInfo)
        {
            if (materialoutstorageInfo.Quantity != null &&
                materialoutstorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.成品手动出库)
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
