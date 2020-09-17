using Solution.Desktop.MaterialInStorageInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.MaterialInStorageInfo.ViewModel
{
    public static class MaterialInStorageInfoSetPalletQuantity
    {
        public static void SetPalletQuantity(MaterialInStorageInfoModel materialinstorageInfo)
        {
            //原料手动入库
            if ( materialinstorageInfo.Quantity != null && 
                 materialinstorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType)
            {
                if (materialinstorageInfo.FullPalletQuantity != null && materialinstorageInfo.FullPalletQuantity.Value != 0)
                {
                    decimal dPalletQuantity = materialinstorageInfo.Quantity.Value / materialinstorageInfo.FullPalletQuantity.Value;
                    if (dPalletQuantity < 1 && dPalletQuantity > 0)
                    {
                        dPalletQuantity = 1;
                    }
                    dPalletQuantity = decimal.Ceiling(dPalletQuantity);
                    //
                    materialinstorageInfo.PalletQuantity = dPalletQuantity;
                }
            }
        }
    }
}
