using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EntEmployeeInfo.EnumType
{
    public enum Sex : int
    {
        /// <summary>
        ///  性别
        /// </summary>
        /// 
        [Description("男")]
        Man = 1,

        [Description("女")]
        Woman = 2,
    }
    #region 工种
    public enum WorkType : int
    {
        /// <summary>
        ///  工种
        /// </summary>
        /// 
        [Description("钳工")]
        QianWorker = 1,

        [Description("车工")]
        CheWorker = 2,

        [Description("铣工")]
        XiWorker = 3,

        [Description("刨工")]
        BaoWorker = 4,

        [Description("磨工")]
        MoWorker = 5,

        [Description("钣金工")]
        BanJinWorker = 6,

        [Description("镗工")]
        TangWorker = 7,

        [Description("冲压工")]
        ChongYaWorker = 8,

        [Description("剪板工")]
        JianBanWorker = 9,

        [Description("折弯工")]
        ZheWanWorker = 10,

        [Description("铸造工")]
        ZhuZaoWorker = 11,

        [Description("锻造工")]
        DuanZaoWorker = 12,

        [Description("热处理工")]
        ReChuLiWorker = 13,

        [Description("机械设备维修工")]
        JiXieSheBeiWeiHuWorker = 14,

        [Description("维修电工")]
        WeiXiuDianWorker = 15,

        [Description("电焊工")]
        DianHan = 16,

        [Description("电加工设备操作工")]
        DianJiaGongSheBeiCaoZuoWorker = 17,
    }

    #endregion

    #region 职务
    public enum Duty : int
    {
        /// <summary>
        ///  职务
        /// </summary>
        /// 
        [Description("操作员工")]
        CaoZuoWorker = 1,

        [Description("统计员")]
        TongJiWorker = 2,

        [Description("生产班长")]
        ShengChanBanZhang = 3,

        [Description("生产主管")]
        ShengChanZhuGuan = 4,

        [Description("生产经理")]
        ShengChanJingLi = 5,

        [Description("生产总监")]
        ShengChanZongJian = 6,

        [Description("维修员")]
        WeiXiuYuan = 7,

        [Description("设施维护员")]
        SheShiWeiHuYuan = 8,

        [Description("电气工程师")]
        DianQiGongChengShi = 9,

        [Description("机械工程师")]
        JiXieGongChengShi = 10,

        [Description("设备主管")]
        SheBeiZhuGuan = 11,

        [Description("设备经理")]
        SheBeiJingLi = 12,

        [Description("检验员")]
        JianChaYuan = 13,

        [Description("实验员")]
        ShiYanYuan = 14,

        [Description("文控专员")]
        WenKongZhuanYuan = 15,

        [Description("计量员")]
        JiLiangYuan = 16,

        [Description("SQE工程师")]
        SQEGongChengShi = 17,

        [Description("质量工程师")]
        ZhiLiangGongChengShi = 18,

        [Description("体系工程师")]
        TiXiGongChengShi = 19,

        [Description("质量主管")]
        ZhiLiangZhuGuan = 20,

        [Description("质量经理")]
        ZhiLiangJingLi = 21,

        [Description("计划员")]
        JiHuaYuan = 22,

        [Description("仓管员")]
        CangGuanYuan = 23,

        [Description("部门经理")]
        BuMenJingLi = 24,

        [Description("采购专员")]
        CaiGouZhuanYuan = 25,

        [Description("采购工程师")]
        CaiGouGongChengShi = 26,

        [Description("采购经理")]
        CaiGouJingLi = 27,

        [Description("业务员")]
        YeWuYuan = 28,

        [Description("跟单员")]
        GenDanYuan = 29,

        [Description("业务经理")]
        YeWuJingLi = 30,

        [Description("行政主管")]
        XingZhengZhuGuan = 31,

        [Description("人事专员")]
        RenShiZhuanYuan = 32,

        [Description("行政人事经理")]
        XingZhengRenShiJingLi = 33,

        [Description("安全专员")]
        AnQuanZhuanYuan = 34,

        [Description("安全主管")]
        AnQuanZhuGuan = 35,

        [Description("安全经理")]
        AnQuanJingLi = 36,

        [Description("技术员")]
        JiShuYuan = 37,

        [Description("技术工程师")]
        JiShuGongChengShi = 38,

        [Description("技术主管")]
        JiShuZhuGuan = 39,

        [Description("技术经理")]
        JiShuJingLi = 40,

        [Description("会计")]
        HuiJi = 41,

        [Description("出纳")]
        ChuNa = 42,

        [Description("财务主管")]
        CaiWuZhuGuan = 43,

        [Description("财务经理")]
        CaiWuJingLi = 44,

        [Description("总经理助理")]
        ZongJingLiZhuLi = 45,

        [Description("总经理秘书")]
        ZongJingLiMiShu = 46,

        [Description("副总经理")]
        FuZongJingLi = 47,

        [Description("总经理")]
        ZongJingLi = 48,
    }

    #endregion

    #region 技能
    public enum Skills : int
    {
        /// <summary>
        /// 技能
        /// </summary>
        [Description("机械维修")]
        JiXieWeiXiu = 1,

        [Description("电焊")]
        DianHan = 2,

        [Description("电加工")]
        DianJiaGong = 3,
    }
    #endregion

    #region 学历
    public enum Education : int
    {
        /// <summary>
        ///  学历
        /// </summary>
        /// 
        [Description("小学")]
        XiaoXue = 1,

        [Description("初中")]
        ChuZhong = 2,

        [Description("高中")]
        GaoZhong = 3,

        [Description("大专")]
        DaZhuan = 4,

        [Description("本科")]
        BenKe = 5,

        [Description("硕士研究生")]
        SuoShiYanJiuSheng = 6,

        [Description("博士研究生")]
        BoShiYanJiuSheng = 7,
    }

    #endregion

    #region 职称
    public enum PositionalTitles : int
    {
        /// <summary>
        ///  职称
        /// </summary>
        /// 
        [Description("技术员")]
        JiShuYuan = 1,

        [Description("助理工程师")]
        ZhuLiGongChengShi = 2,

        [Description("工程师")]
        GongChengShi = 3,

        [Description("高级工程师")]
        GaoJiGongChengShi = 4,

        [Description("正高级工程师")]
        ZhengGaoJiGongChengShi = 5,
    }

    #endregion

}
