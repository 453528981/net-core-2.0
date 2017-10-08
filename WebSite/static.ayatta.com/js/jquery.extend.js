(function ($) {
    $.extend({
        PACKAGES: {},
        namespace: function (name) {
            var i, ni, nis = name.split("."), ns = window;
            for (i = 0; i < nis.length; i = i + 1) {
                ni = nis[i];
                ns[ni] = ns[ni] || {};
                ns = ns[nis[i]];
            }
            return ns;
        },
        package: function () {
            var name = arguments[0],
                func = arguments[arguments.length - 1],
                ns = window,
                returnValue;
            if (typeof func === "function") {
                if (typeof name === "string") {
                    ns = this.namespace(name);
                    if ($.PACKAGES[name]) {
                        throw new Error("Package name [" + name + "] is exist!");
                    } else {
                        $.PACKAGES[name] = {
                            isLoaded: true,
                            returnValue: returnValue
                        };
                    }
                }
                ns.packageName = name;

                returnValue = func.call(ns, this);
            } else {
                throw new Error("Function required");
            }
        },
        postJSON: function (url, data, callback) {
            return this.post(url, data, callback, "json");
        },
        postJSONWS: function (url, data, callback) {
            var param = {};
            if (typeof data == "function" && callback == null) {
                callback = data;
            }
            if (data != null && typeof data != "function") {
                param = data;
            }
            return this.ajax({ contentType: "application/json", type: "POST", url: url, data: param, dataType: "json", success: callback });
        },
        getXML: function (url, data, callback) {
            this.ajax({ type: "GET", url: url, dataType: "xml", data: data, success: callback });
        },
        postXML: function (url, data, callback) {
            this.ajax({ type: "POST", url: url, dataType: "xml", data: data, success: callback });
        },
        format: function (source, params) {
            if (arguments.length == 1)
                return function () {
                    var args = $.makeArray(arguments);
                    args.unshift(source);
                    return $.format.apply(this, args);
                };
            if (arguments.length > 2 && params.constructor != Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor != Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
            });
            return source;
        }
    });
})(jQuery);

(function ($) {
    $.fn.extend({
        check: function () {
            return this.each(function () { this.checked = true; });
        },
        uncheck: function () {
            return this.each(function () { this.checked = false; });
        },
        queryString: function () {
            var s = [];
            function add(key, value) {
                s[s.length] = encodeURIComponent(key) + '=' + encodeURIComponent(value);
            };
            var params = this.serializeArray();
            if ($.isArray(params))
                $.each(params, function (i, o) {
                    if (o.value != "") {
                        add(o.name, o.value);
                    }
                });
            return s.join("&").replace(/%20/g, "+");
        }
    });
})(jQuery);