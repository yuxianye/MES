﻿using Solution.Desktop.Core;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Solution.Desktop.ProManufacturingBillManage.View
{
    /// <summary>
    /// ProManufacturingBOMBillInfoAddView.xaml 的交互逻辑
    /// </summary>
    public partial class ProManufacturingBOMBillItemInfoAddView : UserControlBase
    {
        public ProManufacturingBOMBillItemInfoAddView()
        {
            InitializeComponent();
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
