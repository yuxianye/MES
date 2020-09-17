/**
 * 扩展的基本校验规则，
 */
$.extend($.fn.validatebox.defaults.rules, {
    minLength: { // 判断最小长度 
        validator: function (value, param) {
            value = $.trim(value); //去空格 
            return value.length >= param[0];
        },
        message: '最少输入 {0} 个字符。'
    },
    length: {
        validator: function (value, param) {
            var len = $.trim(value).length;
            return len >= param[0] && len <= param[1];
        },
        message: "输入大小不正确"
    },
    zn: {
        validator: function (value) {
            return /^[0-9a-zA-Z]*$/g.test(value);
        },
        message: "只能是数字或字母"
        
    },
    cnhmt_phone: {// 验证港澳台大陆电话号码 
        validator: function (value) {
            return /^[1][3-8]\d{9}$|^([6|9])\d{7}$|^[0][9]\d{8}$|^[6]([8|6])\d{5}$/i.test(value);
        },
        message: '手机号码格式错误'
    },
    phone: {// 验证电话号码 
        validator: function (value) {
            return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '格式不正确,请使用下面格式:020-88888888'
    },
    mobile: {// 验证手机号码 
        validator: function (value) {
            return /^(13|15|18)\d{9}$/i.test(value);
        },
        message: '手机号码格式不正确'
    },
    idcard: {// 验证身份证 
        validator: function (value) {
            return /^\d{15}(\d{2}[Xx0-9])?$/i.test(value);
        },
        message: '身份证号码格式不正确'
    },
    passid: {// 验证通行证 
        validator: function (value) {
            return ((/^[HMhm]{1}([0-9]{10}|[0-9]{8})$/.test(value)) || (/^[0-9]{8}$/.test(value)) || (/^[0-9]{10}$/.test(value)));
        },
        message: '通行证格式不正确'
    },
    intOrFloat: {// 验证整数 
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '请输入数字，并确保格式正确'
    },
    intOrFloat_Tw: {// 验证整数或两位小数 
        validator: function (value) {
            return /^\d+(\.\d{1,2})?$/i.test(value);
        },
        message: '请输入数字(小数为两位)，并确保格式正确'
    },
    intOrFloat_Tw_NotZero: {// 验证整数或两位小数 
        validator: function (value) {
            return /^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d{1,2})?$/i.test(value);
        },
        message: '请输入大于0的数字(小数为两位)，并确保格式正确'
    },
    currency: {// 验证货币 
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '货币格式不正确'
    },
    qq: {// 验证QQ,从10000开始 
        validator: function (value) {
            return /^[1-9]\d{4,9}$/i.test(value);
        },
        message: 'QQ号码格式不正确'
    },
    integer: {// 验证整数 
        validator: function (value) {
            return /^[+]?[1-9]+\d*$/i.test(value);
        },
        message: '请输入整数'
    },
    chinese: {// 验证中文 
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/i.test(value);
        },
        message: '请输入中文'
    },
    english: {// 验证英语 
        validator: function (value) {
            return /^[A-Za-z]+$/i.test(value);
        },
        message: '请输入英文'
    },
    unnormal: {// 验证是否包含空格和非法字符 
        validator: function (value) {
            return /.+/i.test(value);
        },
        message: '输入值不能为空和包含其他非法字符'
    },
    username: {// 验证用户名 
        validator: function (value) {
            return /^[a-zA-Z][a-zA-Z0-9_]{5,15}$/i.test(value);
        },
        message: '用户名不合法（字母开头，允许6-16字节，允许字母数字下划线）'
    },
    faxno: {// 验证传真 
        validator: function (value) {
            //            return /^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/i.test(value); 
            return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '传真号码不正确'
    },
    zip: {// 验证邮政编码 
        validator: function (value) {
            return /^[1-9]\d{5}$/i.test(value);
        },
        message: '邮政编码格式不正确'
    },
    ip: {// 验证IP地址 
        validator: function (value) {
            return /d+.d+.d+.d+/i.test(value);
        },
        message: 'IP地址格式不正确'
    },
    name: {// 验证姓名，可以是中文或英文 
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/i.test(value) | /^\w+[\w\s]+\w+$/i.test(value);
        },
        message: '请输入姓名'
    },
    carNo: {
        validator: function (value) {
            return /^[\u4E00-\u9FA5][\da-zA-Z]{6}$/.test(value);
        },
        message: '车牌号码无效（例：粤J12350）'
    },
    carenergin: {
        validator: function (value) {
            return /^[a-zA-Z0-9]{16}$/.test(value);
        },
        message: '发动机型号无效(例：FG6H012345654584)'
    },
    email: {//验证邮箱
        validator: function (value) {
            return /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
        },
        message: '请输入有效的电子邮件账号(例：abc@126.com)'
    },
    msn: {
        validator: function (value) {
            return /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
        },
        message: '请输入有效的msn账号(例：abc@hotnail(msn/live).com)'
    },
    same: {
        validator: function (value, param) {
            if ($("#" + param[0]).val() != "" && value != "") {
                return $("#" + param[0]).val() == value;
            } else {
                return true;
            }
        },
        message: '两次输入的密码不一致！'
    },
    warnmintime: { // 判断告警的值只能一级一级的增加，最小值
        validator: function (value, param) {
            value = $.trim(value); //去空格 
            if (value != "")
                for (var i = 0; i < param.length; i++) {
                    $(param[i]).val();
                    if ($(param[i]).combobox('getValue')) {
                        var temp = $.trim($(param[i]).combobox('getValue'));
                        if (temp != "" && !isNaN(temp) && parseInt(value) <= parseInt(temp))
                            return false;
                    }
                }
            return true;
        },
        message: '不能小于当前告警的前一级的告警时间'
    },
    warnmaxtime: { // 判断告警的值只能一级一级的增加，最大值
        validator: function (value, param) {
            value = $.trim(value); //去空格 
            if (value != "")
                for (var i = 0; i < param.length; i++) {
                    $(param[i]).val();
                    if ($(param[i]).combobox('getValue')) {
                        var temp = $.trim($(param[i]).combobox('getValue'));
                        if (temp != "" && !isNaN(temp) && parseInt(value) >= parseInt(temp))
                            return false;
                    }
                }
            return true;
        },
        message: '不能大于当前告警的后一级的告警时间'
    },
    compareDate: {
        validator: function (value, param) {
            return dateCompare($(param[0]).datetimebox('getValue'), value); //注意easyui 时间控制获取值的方式
        },
        message: '开始日期不能大于结束日期'
    },
    passWord:{
        validator: function (value) {
            // /^[0-9a-zA-Z]*$/g
            return /(?!^\d+$)(?!^[a-zA-Z]+$)[0-9a-zA-Z]{6,16}/.test(value);
        },
        message: '有效长度6-16位数字和字母'
    },
    HMTName: {
        validator: function (value) {
            // /^[0-9a-zA-Z]*$/g
            return /^[ \ \a-zA-Z\u4e00-\u9fa5]+$/.test(value);
        },
        message: '港澳台姓名:英文,半角空格,汉字'

        
    }

});
