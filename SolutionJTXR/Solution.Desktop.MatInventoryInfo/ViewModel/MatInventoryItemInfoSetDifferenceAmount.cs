using Solution.Desktop.MatInventoryInfo.Model;

namespace Solution.Desktop.MatInventoryInfo.ViewModel
{
    public static class MatInventoryItemInfoSetDifferenceAmount
    {
        public static void SetDifferenceAmount(MatInventoryItemInfoModel matinventoryitemInfo)
        {
            if  (matinventoryitemInfo.AccuntAmount != null &&
                 matinventoryitemInfo.ActualAmount != null)
            {
                //decimal dDifferenceAmount = matinventoryitemInfo.AccuntAmount.Value - matinventoryitemInfo.ActualAmount.Value;
                decimal dDifferenceAmount = matinventoryitemInfo.ActualAmount.Value - matinventoryitemInfo.AccuntAmount.Value;
                //
                matinventoryitemInfo.DifferenceAmount = dDifferenceAmount;
            }
        }
    }
}
