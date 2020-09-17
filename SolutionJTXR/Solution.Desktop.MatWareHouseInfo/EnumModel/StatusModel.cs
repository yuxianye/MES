using Solution.Desktop.Core;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public class StatusModel : ModelBase
    {
        #region Code
        private string code;

        public string Code
        {
            get { return code; }
            set { Set(ref code, value); }
        }
        #endregion

        #region Name
        private string name;

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }
        #endregion

        protected override void Disposing()
        {
            Code = null;
            Name = null;
        }
    }
}
