using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Solution.CommunicationModule.Contracts;
namespace Solution.Agv.ControlService
{
    /// <summary>
    /// 车辆信息服务
    /// </summary>
    public class AgvControlService : IAgvControlContract
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(AgvControlService));

        /// <summary>
        /// 1、初始化socket客户端。
        /// 2、从业务点表查询本业务需要的点表，通过socket客户端注册相关的通讯点。
        /// 3、根据业务点变化控制/调度车辆、仓储库房出入库业务、设备Equipment资源状态等。
        /// </summary>
        public void Initialize()
        {
            bool socketClientResult = initSocketClient();
            Logger.Info($"AgvControlService.socketClientHelper连接结果：{socketClientResult}");

            bool registerNodeResult = registerNode();
            Logger.Info($"AgvControlService.registerNode：{registerNodeResult}");



        }

        /// <summary>
        /// 初始化socket客户端服务
        /// </summary>
        private bool initSocketClient()
        {
            var socketServerInfo = socketServerContract.SocketServerInfos.FirstOrDefault();

            if (Equals(socketServerInfo, null))
            {
                Logger.Info($"AgvControlService.initSocketClient,未找到SocketServerInfo的信息");

                return false;
            }
            socketClientHelper = new Utility.Socket.SocketClientHelper(socketServerInfo.ServerIp, socketServerInfo.ServerPort);
            var result = socketClientHelper.ConnectAsync().Result;
            socketClientHelper.Connected += SocketClientHelper_Connected;
            socketClientHelper.Closed += SocketClientHelper_Closed;
            socketClientHelper.OnDataReceived += SocketClientHelper_OnDataReceived;
            socketClientHelper.Error += SocketClientHelper_Error;
            return result;
        }

        List<ClientDataEntity> ClientDataEntities = new List<ClientDataEntity>();

        /// <summary>
        /// 注册需要监视的点
        /// </summary>
        /// <returns></returns>
        private bool registerNode()
        {
            //string businessName = Utility.ConfigHelper.GetAppSetting("Business_Agv");
            string businessName = System.Web.Configuration.WebConfigurationManager.AppSettings["Business_Agv"];
            var businessNodes = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps.
                Where(a => a.BusinessNode.BusinessName.StartsWith(businessName));
            bool result = false;
            //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            if (!(Equals(businessNodes, null) && businessNodes.Any()))
            {
                //ClientDataEntities?.Clear();
                foreach (var data in businessNodes)
                {
                    ClientDataEntity clientDataEntity = new ClientDataEntity();
                    clientDataEntity.FunctionCode = FuncCode.SubScription;
                    clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId = data.Id;
                    //clientDataEntity=data.BusinessNode.BusinessName
                    clientDataEntity.StatusCode = 0;
                    clientDataEntity.NodeId = data.DeviceNode.NodeUrl;
                    clientDataEntity.ValueType = Utility.Extensions.TypeExtensions.TypeCodeToTypeDic[(TypeCode)data.DeviceNode.DataType];
                    ClientDataEntities.Add(clientDataEntity);
                }
                if (ClientDataEntities.Count > 0)
                {
                    result = this.socketClientHelper.Send(addMessageSplit(JsonHelper.ToJson(ClientDataEntities)));
                }
                else
                {

                    result = false;
                }
                Logger.Info($"AgvControlService.registerNode ClientDataEntities数量={ClientDataEntities.Count}");

            }
            return result;
        }

        /// <summary>
        /// 增加消息分隔符 开始结束符
        /// </summary>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        private String addMessageSplit(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
        }
        private void SocketClientHelper_Connected(object sender, EventArgs e)
        {
            Logger.Info($"AgvControlService.SocketClientHelper_Connected{socketClientHelper.ServerIp}");

        }
        private void SocketClientHelper_Closed(object sender, EventArgs e)
        {
            Logger.Info($"AgvControlService.SocketClientHelper_Closed{socketClientHelper.ServerIp}");
        }

        private void SocketClientHelper_OnDataReceived(object sender, Utility.Socket.DataEventArgs e)
        {
            //Logger.Info($"SocketClientHelper_OnDataReceived{socketClientHelper.ServerIp}：{e.PackageInfo.Data}");



            var cde = JsonHelper.FromJson<List<ClientDataEntity>>(e.PackageInfo.Data);
            if (!Equals(cde, null) && cde.Any())
            {
                foreach (var v in cde)
                {
                    var tmp = ClientDataEntities.FirstOrDefault(a => a.NodeId == v.NodeId);
                    if (!Equals(v.Value, null))
                    {
                        tmp.Value = v.Value?.ToString();
                    }
                    tmp.StatusCode = v.StatusCode;
                    Logger.Debug(tmp.NodeId + " " + tmp.Value?.ToString());
                }
            }
        }


        private void SocketClientHelper_Error(object sender, Exception e)
        {
            Logger.Error($"AgvControlService.SocketClientHelper_Error：{socketClientHelper.ServerIp}", e);
        }

        /// <summary>
        /// 业务点表契约
        /// </summary>
        public IProductionProcessEquipmentBusinessNodeMapContract ProductionProcessEquipmentBusinessNodeMapContract { get; set; }

        public ISocketServerContract socketServerContract { get; set; }

        private Utility.Socket.SocketClientHelper socketClientHelper;// = new Utility.Socket.SocketClientHelper();


        ///// <summary>
        ///// 车辆信息实体仓储
        ///// </summary>
        //public IRepository<AgvInfo, Guid> AgvInfoRepository { get; set; }

        ///// <summary>
        ///// 查询企业信息数据集
        ///// </summary>
        //public IQueryable<AgvInfo> AgvInfos
        //{
        //    get { return AgvInfoRepository.Entities; }
        //}

        ///// <summary>
        ///// 检查实体是否存在
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool CheckAgvInfoExists(Expression<Func<AgvInfo, bool>> predicate, Guid id)
        //{
        //    return AgvInfoRepository.CheckExists(predicate, id);
        //}

        ///// <summary>
        ///// 增加agv小车信息
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> Add(params AgvInfoInputDto[] inputDtos)
        //{
        //    inputDtos.CheckNotNull("inputDtos");
        //    foreach (var dtoData in inputDtos)
        //    {
        //        if (string.IsNullOrEmpty(dtoData.CarNo))
        //            return new OperationResult(OperationResultType.Error, "请正确填写车辆编号，该组数据不被存储。");
        //        if (string.IsNullOrEmpty(dtoData.CarName))
        //            return new OperationResult(OperationResultType.Error, "请正确填写车辆名称，该组数据不被存储。");
        //        if (AgvInfoRepository.CheckExists(x => x.CarNo == dtoData.CarNo))
        //            return new OperationResult(OperationResultType.Error, $"车辆编号 {dtoData.CarNo} 的数据已存在，该组数据不被存储。");
        //        if (AgvInfoRepository.CheckExists(x => x.CarName == dtoData.CarName))
        //            return new OperationResult(OperationResultType.Error, $"车辆名称 {dtoData.CarName} 的数据已存在，该组数据不被存储。");
        //    }
        //    AgvInfoRepository.UnitOfWork.BeginTransaction();
        //    var result = await AgvInfoRepository.InsertAsync(inputDtos);
        //    AgvInfoRepository.UnitOfWork.Commit();
        //    return result;
        //}

        ///// <summary>
        ///// 更新agv小车信息
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> Update(params AgvInfoInputDto[] inputDtos)
        //{
        //    inputDtos.CheckNotNull("inputDtos");
        //    foreach (AgvInfoInputDto dtoData in inputDtos)
        //    {
        //        if (string.IsNullOrEmpty(dtoData.CarNo))
        //            return new OperationResult(OperationResultType.Error, "请正确填写车辆编号，该组数据不被存储。");
        //        if (string.IsNullOrEmpty(dtoData.CarNo))
        //            return new OperationResult(OperationResultType.Error, "请正确填写车辆名称，该组数据不被存储。");
        //        if (AgvInfoRepository.CheckExists(x => x.CarNo == dtoData.CarNo && x.Id != dtoData.Id))
        //            return new OperationResult(OperationResultType.Error, $"车辆编号 {dtoData.CarNo} 的数据已存在，该组数据不被存储。");
        //        if (AgvInfoRepository.CheckExists(x => x.CarName == dtoData.CarName && x.Id != dtoData.Id))
        //            return new OperationResult(OperationResultType.Error, $"车辆名称 {dtoData.CarName} 的数据已存在，该组数据不被存储。");
        //    }
        //    AgvInfoRepository.UnitOfWork.BeginTransaction();
        //    var result = await AgvInfoRepository.UpdateAsync(inputDtos);
        //    AgvInfoRepository.UnitOfWork.Commit();
        //    return result;
        //}

        ///// <summary>
        ///// 物理删除agv小车信息
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> Delete(params Guid[] ids)
        //{
        //    ids.CheckNotNull("ids");
        //    AgvInfoRepository.UnitOfWork.BeginTransaction();
        //    var result = await AgvInfoRepository.DeleteAsync(ids);
        //    AgvInfoRepository.UnitOfWork.Commit();
        //    return result;
        //}
    }
}
