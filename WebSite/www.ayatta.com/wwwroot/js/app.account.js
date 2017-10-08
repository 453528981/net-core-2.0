$.package('app.account', function () {
    var scope = {};
    this.validateUid = function (obj) {
        var uid = obj.value;
        if (uid.length < 11) {
            return;
        }
        if (!uid.isMobile()) {
            $("#help-block-for-uid").html('请输入正确的手机号码！');
            $("#form-group-for-uid").removeClass('has-success').addClass('has-error');
            return;
        }
        $.postJSON('/account/uid-exist', { uid: uid }, function (e) {
            if (e) {
                document.getElementById('btn-sign-up').disabled = true;
                document.getElementById('btn-get-captcha').disabled = true;
                $("#help-block-for-uid").html('该手机号码已被使用，请换一个！');
                $("#form-group-for-uid").removeClass('has-success').addClass('has-error');
            } else {
                $("#help-block-for-uid").html("");
                document.getElementById('btn-sign-up').disabled = false;
                document.getElementById('btn-get-captcha').disabled = false;
                $("#form-group-for-uid").removeClass('has-error').addClass('has-success');
            }
        });
    }

    this.validatePwd = function (obj, confirm) {
        var pwd = obj.value;
        if (!pwd.isPassword()) {
            $(obj).parent().removeClass('has-success').addClass('has-error');

        } else {
            $(obj).parent().removeClass('has-error').addClass('has-success');
            if (confirm) {
                var p = $("#pwd").val();
                if (p !== pwd) {
                    $(obj).parent().removeClass('has-success').addClass('has-error');
                } else {
                    $(obj).parent().removeClass('has-error').addClass('has-success');
                }
            }
        }

    }

    this.getCaptcha = function (obj) {
        var mobile = document.getElementById('uid').value;
        if (!mobile.isMobile()) {
            $("#help-block-for-uid").html('请输入正确的手机号码！');
            $("#form-group-for-uid").removeClass('has-success').addClass('has-error');

        } else {
            var counter = 8;
            obj.disabled = true;
            var timer = setInterval(function () {
                if (counter < 1) {
                    clearInterval(timer);
                    obj.disabled = false;
                    $(obj).html('获取验证码');

                } else {
                    counter -= 1;
                    $(obj).html('获取验证码（' + counter + '）');
                }
            }, 1000);

            $.postJSON('/misc/sms-captcha-send', { mobile: mobile }, function (r) {
                if (r.status) {
                    scope.guid = r.message;
                    $("#help-block-for-uid").html("");
                    $("#form-group-for-captcha").show();
                } else {
                    $("#help-block-for-uid").html("");
                    $("#form-group-for-captcha").hide();
                }
                console.log(scope);
            });
        }
    }

    var needCaptcha = false;

    this.signIn = function (obj) {
        var form = obj.form;
        var uid = form.uid.value;
        var pwd = form.pwd.value;
        var captcha = form.captcha.value;
        if (uid.isEmpty()) {
            form.uid.focus();
            $(form.uid).parent().addClass('has-error');
            return false;
        }
        if (pwd.isEmpty()) {
            form.pwd.focus();
            $(form.pwd).parent().addClass('has-error');
            return false;
        }
        if (needCaptcha) {
            form.captcha.focus();
            $(form.captcha).parent().addClass('has-error');
            return false;
        }

        $(form.uid).parent().removeClass('has-error');
        $(form.pwd).parent().removeClass('has-error');
        var param = $(form).serialize();

        $.postJSON(form.action, param, function (result) {

            if (result.status) {
                if (result.data != null && result.data !== "") {
                    window.location = result.data;
                } else {
                    window.location = "/";
                    //window.location.reload();
                }
            } else {
                if (result.extra > 2) {
                    needCaptcha = true;
                    $("#form-group-captcha").show();
                }
                $("#notice-msg").html(result.message);
            }
        });

        return false;
    }

    this.signUp = function (obj) {
        var form = obj.form;
        var uid = form.uid.value;
        var pwd = form.pwd.value;
        var repwd = form.repwd.value;
        var captcha = form.captcha.value;
        if (uid.isEmpty()) {
            form.uid.focus();
            $(form.uid).parent().addClass('has-error');
            return false;
        }
        if (captcha.length !== 6) {
            $(form.captcha).parent().addClass('has-error');
            $(form.captcha).parent().show();
            form.captcha.focus();
            return false;
        }
        if (!pwd.isPassword()) {
            form.pwd.focus();
            $(form.pwd).parent().addClass('has-error');
            return false;
        }
        if (repwd !== pwd) {
            form.repwd.focus();
            $(form.repwd).parent().addClass('has-error');
            return false;
        }
        var param = $(form).serialize() + "&guid=" + scope.guid;

        $.postJSON(form.action, param, function (result) {
            $(obj).button('loading');
            if (result.status) {
                if (result.data != null && result.data !== "") {
                    window.location = result.data;
                } else {
                    window.location.reload();
                }
            } else {
                $(obj).button('default');
                $("#notice-msg").html(result.message);
            }

        });

        return false;
    }

    this.confirm = function (obj) {
        var form = obj.form;
        var uid = form.uid.value;

        var captcha = form.captcha.value;
        if (uid.isEmpty()) {
            form.uid.focus();
            $(form.uid).parent().addClass('has-error');
            return false;
        }

        if (captcha.length !== 5) {
            form.captcha.focus();
            $(form.captcha).parent().addClass('has-error');
            return false;
        }

        var param = $(form).serialize();

        $.postJSON(form.action, param, function (result) {

            if (result.code === 0) {

                window.location = '/account/verify';

            } else {
                $("#notice-msg").html(result.message);

            }

        });

        return false;
    }


    this.getVerifyCaptcha = function (obj,email) {
        var counter = 8;
        obj.disabled = true;
        var timer = setInterval(function () {
            if (counter < 1) {
                clearInterval(timer);
                obj.disabled = false;
                $(obj).html('获取验证码');

            } else {
                counter -= 1;
                $(obj).html('获取验证码（' + counter + '）');
            }
        }, 1000);

        $.postJSON('/account/verify-captcha-send', { email: email }, function (r) {
            if (r.status) {
                scope.guid = r.message;               
            } else {
                alert(r.message);
            }            
        });
    }

    this.verify = function (obj,email) {
        var form = obj.form;
    }

});