using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Solution.Utility.Extensions;
using System.Windows.Controls;
using System.Windows;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
   public static class NumberUtility
    {
        public static void isIntegerNumber(object sender,int iNumberLen)
        {
            string sText = ((TextBox)sender).Text;
            //
            if (!sText.Trim().Equals("") && !sText.Equals("0"))
            {
                int iNumber = 0;
                bool bReturn = int.TryParse(sText, out iNumber);
                if (!bReturn)
                {
                    if (sText.IndexOf(".00") != -1 )
                    {
                        sText = sText.Replace(".00", "");
                        bReturn = int.TryParse(sText, out iNumber);
                        if (bReturn)
                        {
                            ((TextBox)sender).Text = sText.ToString();
                        }
                    }
                }
                if (!(bReturn&&
                        (iNumber > 0 && iNumber <= iNumberLen)))
                {
                   //MessageBox.Show(string.Format("请输入小于等于" + iNumberLen.ToString() + "的数值！" + System.Environment.NewLine), "", MessageBoxButton.OK);
                   //((TextBox)sender).Text = "0";
                   //((TextBox)sender).SelectAll();
                   //((TextBox)sender).Text = "";
                   //
                   Application.Current.Resources["UiMessage"] = string.Format("请输入小于等于" + iNumberLen.ToString() + "的数值！" + "操作失败，请联系管理员！");
                }
                else
                {
                    Application.Current.Resources["UiMessage"] = "";
                }
            }
            else if ( string.IsNullOrEmpty(sText) || sText.Equals("") || sText.Trim().Equals(""))
            {
                ((TextBox)sender).Text = "0";
                ((TextBox)sender).SelectAll();
            }
        }

        public static void isDecimalNumber(object sender, int iNumberLen)
        {
            string sText = ((TextBox)sender).Text;
            //
            if (!sText.Trim().Equals("") && !sText.Equals("0"))
            {
                decimal dNumber = 0;
                if (!(decimal.TryParse(sText, out dNumber) &&
                        (dNumber > 0 && dNumber <= iNumberLen)))
                {
                    //MessageBox.Show(string.Format("请输入小于等于" + iNumberLen.ToString() + "的数值！" + System.Environment.NewLine), "", MessageBoxButton.OK);
                    //((TextBox)sender).Text = "0";
                    //((TextBox)sender).SelectAll();
                    //((TextBox)sender).Text = "";
                    //
                    Application.Current.Resources["UiMessage"] = string.Format("请输入小于等于" + iNumberLen.ToString() + "的数值！" + "操作失败，请联系管理员！");
                }
                else
                {
                    Application.Current.Resources["UiMessage"] = "";
                }
            }
            else if (string.IsNullOrEmpty(sText) || sText.Equals("") || sText.Trim().Equals(""))
            {
                ((TextBox)sender).Text = "0";
                ((TextBox)sender).SelectAll();
            }
        }
    }
}
