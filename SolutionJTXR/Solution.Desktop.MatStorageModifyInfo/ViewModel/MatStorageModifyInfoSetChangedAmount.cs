using Solution.Desktop.MatStorageModifyInfo.Model;

namespace Solution.Desktop.MatStorageModifyInfo.ViewModel
{
    public static class MatStorageModifyInfoSetChangedAmount
    {
        public static void SetChangedAmount(MatStorageModifyInfoModel matstoragemodifyInfo)
        {
            if  (matstoragemodifyInfo.CurrentAmount != null &&
                 matstoragemodifyInfo.OriginalAmount!= null)
            {
                decimal dChangedAmount = matstoragemodifyInfo.CurrentAmount.Value - matstoragemodifyInfo.OriginalAmount.Value;
                //
                matstoragemodifyInfo.ChangedAmount = dChangedAmount;
            }
        }
    }
}
