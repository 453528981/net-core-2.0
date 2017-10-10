String.prototype.lTrim = function() {
    return this.replace(/^\s*/, "");
};
String.prototype.rTrim = function() {
    return this.replace(/\s*$/, "");
};
String.prototype.trim = function() {
    return this.replace(/(^s*)|(s*$)/g, "");
};
String.prototype.endsWith = function(sEnd) {
    return (this.substr(this.length - sEnd.length) == sEnd);
};
String.prototype.startsWith = function(sStart) {
    return (this.substr(0, sStart.length) == sStart);
};
String.prototype.format = function() {
    var s = this;
    for (var i = 0; i < arguments.length; i++) { s = s.replace("{" + (i) + "}", arguments[i]); }
    return (s);
};
String.prototype.escape = function() {
    var returnString;
    returnString = escape(this);
    returnString = returnString.replace(/\+/g, "%2B");
    return returnString;
};
String.prototype.unescape = function() {
    return unescape(this);
};
String.prototype.removeSpaces = function() {
    return this.replace(/ /gi, '');
};
String.prototype.removeExtraSpaces = function() {
    return (this.replace(new RegExp("\\s+", "g"), " "));
};
String.prototype.removeSpaceDelimitedString = function(r) {
    var s = " " + this.trim() + " ";
    return s.replace(" " + r, "").rTrim();
};

/*
去除标点符号
*/
String.prototype.removeSymbol = function() {

    return this.replace(/[^\u4e00-\u9fa5\A-Za-z0-9\-\/\.\'\s]/g, '')
        .replace(/\-+/g, '-').replace(/\/+/g, '/').replace(/\.+/g, '.').replace(/\'+/g, '\'').replace(/\s+/g, ' ')
        .replace(/^[\-\/\.\']/g, '').replace(/[\-\/\.\']$/g, '').replace(/(^s*)|(s*$)/g, "");

}

String.prototype.Length = function(encoding) {
    var step = 1;
    if (encoding == "gbk" || encoding == "gb2312") {
        step = 2;
    } else if (encoding == "utf-8") {
        step = 3;
    }
    var realLength = 0;
    var len = this.length;
    var charCode = -1;
    for (var i = 0; i < len; i++) {
        charCode = this.charCodeAt(i);
        if (charCode >= 0 && charCode <= 128) {
            realLength += 1;
        } else {
            //如果是中文
            realLength += step;
        }
    }
    return realLength;
}

String.prototype.isEmpty = function() {
    return this.length == 0;
};

String.prototype.isMobile = function() {
    var val = this.replace(/\s+/g, "");
    var reg = /(^0{0,1}13[0-9]{9}$)|(13\d{9}$)|(15[0135-9]\d{8}$)|(18[26789]\d{8}$)/;
    return val.length == 11 && reg.exec(val);
};

String.prototype.isPhone = function() {
    var val = this.replace(/\s+/g, "");
    var reg = /^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/;
    return reg.exec(val);
};

String.prototype.isDecimal = function() {
    var re = /^\d*\.?\d{1,2}$/;
    if (this.match(re) == null)
        return false;
    else
        return true;
};

String.prototype.isNumber = function() {
    var re = /^\d*$/;
    if (this.match(re) == null)
        return false;
    else
        return true;
};

String.prototype.isFloat = function() {
    for (i = 0; i < this.length; i++) {
        if ((this.charAt(i) < "0" || this.charAt(i) > "9") && this.charAt(i) != '.') {
            return false;
        }
    }
    return true;
};

String.prototype.isUrl = function() {
    var urlRegX = /[^a-zA-Z0-9-]/g;
    return this.match(urlRegX, "");
};
String.prototype.isEmail = function() {
    var emailReg = /^\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return emailReg.test(this);
};
String.prototype.isAlphaNumeric = function() {
    var alphaReg = /[^a-zA-Z0-9]/g;
    return !alphaReg.test(this);
};
String.prototype.isPostalCode = function isValidPost() {
    var re = /^\d{6}$/;
    if (this.match(re) == null)
        return false;
    else
        return true;
};

String.prototype.isPassword = function() {
    //这个正则要求密码长度最少6位，包含至少1个特殊字符，2个数字，2个大写字母和一些小写字母
    //分解
    //(?=^.{12,25}$) -- 密码长度12-25，自己改变数字可以调节  
    //(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1}) -- 至少一个特殊字母
    //(?=(?:.*?\d){2}) -- 至少2个数字
    //(?=.*[a-z]) -- a-z的小写字母  
    //(?=(?:.*?[A-Z]){2}) -- 至少2个大写字母 
    var reg = /^(?=^.{6,18}$)(?=(?:.*?\d){2})(?=.*[a-z])(?=(?:.*?[A-Z]){2})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$/;
    return reg.test(this);
}