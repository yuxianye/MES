using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using Solution.Utility.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solution.Web.App.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //添加测试
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseInfoInputDto[] dto = new EnterpriseInfoInputDto[]
            {
                #region 创建数据
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "aaa",
                    EnterpriseCode = "1",
                    EnterpriseAddress = "aaa",
                    EnterprisePhone = "11111",
                    Description = "aaa",
                    Remark = "aaa",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "bbb",
                    EnterpriseCode = "2",
                    EnterpriseAddress = "bbb",
                    EnterprisePhone = "2222",
                    Description = "bbb",
                    Remark = "bbb",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "ccc",
                    EnterpriseCode = "3",
                    EnterpriseAddress = "ccc",
                    EnterprisePhone = "3333",
                    Description = "ccc",
                    Remark = "ccc",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "ddd",
                    EnterpriseCode = "4",
                    EnterpriseAddress = "ddd",
                    EnterprisePhone = "4444",
                    Description = "ddd",
                    Remark = "ddd",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "eee",
                    EnterpriseCode = "5",
                    EnterpriseAddress = "eee",
                    EnterprisePhone = "5555",
                    Description = "eee",
                    Remark = "eee",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "fff",
                    EnterpriseCode = "6",
                    EnterpriseAddress = "fff",
                    EnterprisePhone = "6666",
                    Description = "fff",
                    Remark = "fff",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "ggg",
                    EnterpriseCode = "7",
                    EnterpriseAddress = "ggg",
                    EnterprisePhone = "7777",
                    Description = "ggg",
                    Remark = "ggg",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "hhh",
                    EnterpriseCode = "8",
                    EnterpriseAddress = "hhh",
                    EnterprisePhone = "8888",
                    Description = "hhh",
                    Remark = "hhh",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "iii",
                    EnterpriseCode = "9",
                    EnterpriseAddress = "iii",
                    EnterprisePhone = "9999",
                    Description = "iii",
                    Remark = "iii",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    EnterpriseName = "jjj",
                    EnterpriseCode = "10",
                    EnterpriseAddress = "jjj",
                    EnterprisePhone = "1010",
                    Description = "jjj",
                    Remark = "jjj",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                }
                #endregion
            };

            var result = HttpClientHelper.PostResponse<OperationResult>(@"http://localhost:13800/api/service/" + "EnterpriseInfo/Add", Utility.JsonHelper.ToJson(dto));
            OperationResultType type = result.ResultType;
            string data = Convert.ToString(result.Data);
        }

        //编辑
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseInfoInputDto[] dto = new EnterpriseInfoInputDto[]
            {
                #region 创建数据
                new EnterpriseInfoInputDto()
                {
                    Id = Guid.Parse("12F5E905-1D6D-E811-8696-002324E261D0"),
                    EnterpriseName = "uuuu",
                    EnterpriseCode = "01",
                    EnterpriseAddress = "uuuu",
                    EnterprisePhone = "1",
                    Description = "uuuu",
                    Remark = "uuuu",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    Id = Guid.Parse("13F5E905-1D6D-E811-8696-002324E261D0"),
                    EnterpriseName = "kkkk",
                    EnterpriseCode = "02",
                    EnterpriseAddress = "kkkk",
                    EnterprisePhone = "2",
                    Description = "kkkk",
                    Remark = "kkkk",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    Id = Guid.Parse("14F5E905-1D6D-E811-8696-002324E261D0"),
                    EnterpriseName = "llll",
                    EnterpriseCode = "03",
                    EnterpriseAddress = "llll",
                    EnterprisePhone = "3",
                    Description = "llll",
                    Remark = "llll",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    Id = Guid.Parse("15F5E905-1D6D-E811-8696-002324E261D0"),
                    EnterpriseName = "oooo",
                    EnterpriseCode = "04",
                    EnterpriseAddress = "oooo",
                    EnterprisePhone = "4",
                    Description = "oooo",
                    Remark = "oooo",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfoInputDto()
                {
                    Id = Guid.Parse("16F5E905-1D6D-E811-8696-002324E261D0"),
                    EnterpriseName = "pppp",
                    EnterpriseCode = "05",
                    EnterpriseAddress = "pppp",
                    EnterprisePhone = "5",
                    Description = "pppp",
                    Remark = "pppp",
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                }
                #endregion
            };

            var result = HttpClientHelper.PostResponse<OperationResult>(@"http://localhost:13800/api/service/" + "EnterpriseInfo/Update", Utility.JsonHelper.ToJson(dto));
            OperationResultType type = result.ResultType;
            string data = Convert.ToString(result.Data);
        }

        //删除
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Guid[] ids = new Guid[]
            {
                Guid.Parse("12F5E905-1D6D-E811-8696-002324E261D0"),
                Guid.Parse("13F5E905-1D6D-E811-8696-002324E261D0")
            };
            var result = HttpClientHelper.PostResponse<OperationResult>(@"http://localhost:13800/api/service/" + "EnterpriseInfo/Remove", Utility.JsonHelper.ToJson(ids));
            OperationResultType type = result.ResultType;
            string data = Convert.ToString(result.Data);
        }

        //读取单个
        private void ReadBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //读取列表
        private void ReadListBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //逻辑删除
        private void RecycleBtn_Click(object sender, RoutedEventArgs e)
        {

            EnterpriseInfo[] enterinfo = new EnterpriseInfo[]
            {
                new EnterpriseInfo()
                {
                    Id = Guid.Parse("E95CF06C-4B6D-E811-8696-002324E261D0"),
                    EnterpriseName = "oooo",
                    EnterpriseCode = "04",
                    EnterpriseAddress = "oooo",
                    EnterprisePhone = "4",
                    Description = "oooo",
                    Remark = "oooo",
                    //IsLocked = false,
                    CreatedTime = DateTime.Now,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfo()
                {
                    Id = Guid.Parse("EA5CF06C-4B6D-E811-8696-002324E261D0"),
                    EnterpriseName = "pppp",
                    EnterpriseCode = "05",
                    EnterpriseAddress = "pppp",
                    EnterprisePhone = "5",
                    Description = "pppp",
                    Remark = "pppp",
                    CreatedTime = DateTime.Now,
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                }
            };

            var result = HttpClientHelper.PostResponse<OperationResult>(@"http://localhost:13800/api/service/" + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(enterinfo));
            OperationResultType type = result.ResultType;
            string data = Convert.ToString(result.Data);
        }

        //逻辑恢复
        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            EnterpriseInfo[] enterinfo = new EnterpriseInfo[]
           {
                new EnterpriseInfo()
                {
                    Id = Guid.Parse("E95CF06C-4B6D-E811-8696-002324E261D0"),
                    EnterpriseName = "oooo",
                    EnterpriseCode = "04",
                    EnterpriseAddress = "oooo",
                    EnterprisePhone = "4",
                    Description = "oooo",
                    Remark = "oooo",
                    //IsLocked = false,
                    CreatedTime = DateTime.Now,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                },
                new EnterpriseInfo()
                {
                    Id = Guid.Parse("EA5CF06C-4B6D-E811-8696-002324E261D0"),
                    EnterpriseName = "pppp",
                    EnterpriseCode = "05",
                    EnterpriseAddress = "pppp",
                    EnterprisePhone = "5",
                    Description = "pppp",
                    Remark = "pppp",
                    CreatedTime = DateTime.Now,
                    //IsLocked = false,
                    CreatorUserId = "1",
                    LastUpdatedTime = DateTime.Now,
                    LastUpdatorUserId = "1",
                    //IsDeleted = false
                }
           };

            var result = HttpClientHelper.PostResponse<OperationResult>(@"http://localhost:13800/api/service/" + "EnterpriseInfo/Restore", Utility.JsonHelper.ToJson(enterinfo));
            OperationResultType type = result.ResultType;
            string data = Convert.ToString(result.Data);
        }
    }
}
