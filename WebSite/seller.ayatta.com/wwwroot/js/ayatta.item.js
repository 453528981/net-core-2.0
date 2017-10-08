$.package('ayatta.item', function () {
    var props = []; //子属性
    var dynamic = {}; //选中的销售属性值
    var saleProps = []; //销售属性
    var propImgCache = {}; //保存颜色图片
    var skuDataCache = {}; //保存用户输入的sku价格数据信息

    var itemData;
    var propChildren = {};

    this.bind = function (catgId, data) {
        itemData = data;
        propImgCache = data.propImgs || {};
        if (data.skus && data.skus.length > 0) {
            for (var i = 0; i < data.skus.length; i++) {
                var o = data.skus[i];
                //状态不为删除
                if (o.status != 2) {
                    skuDataCache[o.propId] = {
                        code: o.code || '',
                        stock: o.stock || 0,
                        price: o.price || 0,
                        appPrice: o.appPrice || 0,
                        retailPrice: o.retailPrice || 0,
                        barcode: o.barcode || ''
                    };
                }
            }
        }

        $.getJSON('/global/catg/' + catgId, function (r) {
            var propHtml = [];
            var keyPropHtml = [];
            var salePropHtml = [];
            var len = r.props.length;
/*
            r.props = r.props.sort(function (a, b) {
                return a.priority > b.priority ? 1 : -1;
            });
*/
            for (var i = 0; i < len; i++) {
                var p = r.props[i];
                if (p.parentPid == 0 && p.values) {
/*
                    p.values = p.values.sort(function (a, b) {
                        return a.Priority > b.Priority ? 1 : -1;
                    });
*/
                    if (p.isKeyProp == true) {
                        keyPropHtml.push('<div>');
                        keyPropHtml.push(propToHtml(p, data));
                        keyPropHtml.push('</div>');
                    } else if (p.isSaleProp == true) {
                        saleProps.push(p);
                        salePropHtml.push('<div>');
                        salePropHtml.push(propToHtml(p, data));
                        salePropHtml.push('</div>');
                    } else {
                        propHtml.push('<div>');
                        propHtml.push(propToHtml(p, data));
                        propHtml.push('</div>');
                    }
                } else {
                    props.push(p);
                }
            }
            $("#container-prop").html(propHtml.join(''));
            $("#container-keyProp").html(keyPropHtml.join(''));
            $("#container-saleProp").html(salePropHtml.join(''));

            console.log(skuDataCache);
            setSkuHtml();

            for (var key in propChildren) {
                for (var i = 0; i < props.length; i++) {
                    var p = props[i];
                    if (p.parentPid == key && p.parentVid == propChildren[key]) {
                        var html = propToHtml(p, data);
                        $("#container-prop-" + key).parent().append(html);
                    }
                }
            }

            $('select').selectpicker();

            $('.html-editor').summernote({
                height: 400,
                lang: 'zh-CN'
            });
        });
    };


    //hack selectpicker 无法调用 ayatta.item.getChildren
    this.getChildProp = function (obj, id) {

        var container = $("#container-prop-" + id);
        container.nextAll().remove();

        var child = null;
        var val = $(obj).val();

        var len = props.length;

        for (var i = 0; i < len; i++) {
            var o = props[i];
            if (o.parentPid == id && o.parentVid == val) {
                child = o;
                break;
            }
        }

        if (child != null) {
            var html = propToHtml(child, itemData);
            container.parent().append(html);
            $('#prop-' + child.id).selectpicker();
        }
    };

    this.checkBoxOnClick = function (obj) {
        var key = obj.value;
        if (obj.checked) {
            var data = $(obj).data();
            dynamic[key] = data;
        } else {
            delete (dynamic[key]);
        }
        setSkuHtml();
    };

    this.nameOnKeyup = function (obj) {
        var val = obj.value = obj.value.replace(/[^\u4e00-\u9fa5\A-Za-z0-9\-\/\.\'\s]/g, '')
            .replace(/\-+/g, '-').replace(/\/+/g, '/').replace(/\.+/g, '.').replace(/\'+/g, '\'').replace(/\s+/g, ' ')
            .replace(/^[\-\/\.\']/g, '').replace(/(^s*)|(s*$)/g, "");

        var len = val.Length("gbk");
        if (len > 60) {
            $(obj).next('.help-block').addClass('.text-danger');
            return;
        }
        var x = 60 - len;
        $('#item-name-len').html('还可以输入' + x + '字节');
    }

    this.nameOnBlur = function (obj) {
        var val = obj.value = obj.value.replace(/[\-\/\.\']$/g, '');

        var len = val.Length("gbk");
        if (len > 60) {
            $(obj).next('.help-block').addClass('.text-danger');
            return;
        }
        var x = 60 - len;
        $('#item-name-len').html('还可以输入' + x + '字节');
    }

    this.priceOnKeyup = function (obj) {
        obj.value = obj.value.replace(/[^0-9\.]/g, '');
    }

    this.priceOnBlur = function (obj) {
        var val = obj.value.replace(/[^0-9\.]/g, '').replace(/^[\.]/g, '').replace(/[\.*]$/g, '');

        var reg = /^-?\d+(\.\d{1,2})?$/
        if (!reg.test(val)) {
            obj.value = val = val == '' ? 0 : parseFloat(val);
        } else {
            obj.value = val;
        }

        var name = obj.name || '';
        if (name == 'item.price') {
            var price = $(obj).data("price") || {
                min: 0,
                max: 999999999
            };

            if (val < price.min || val > price.max) {
                obj.value = price.min;
            }
        } else {
            var prices = [];

            var array = $('.sku-price');

            $.each(array, function (i, o) {
                var price = $(o).val();
                if (reg.test(price)) {
                    prices.push(price);
                }
            });
            var min = lodash.min(prices);
            var max = lodash.max(prices);
            var data = {
                min: min,
                max: max
            };
            var extraData = $(obj).data();
            var skuKey = extraData.skuKey;

            skuDataCache[skuKey].price = obj.value;
            $("input[name='item.price']").val(min).data("price", data);
        }

    }

    this.appPriceOnBlur = function (obj) {
        var val = obj.value.replace(/[^0-9\.]/g, '').replace(/^[\.]/g, '').replace(/[\.*]$/g, '');

        var reg = /^-?\d+(\.\d{1,2})?$/;
        if (!reg.test(val)) {
            obj.value = val = (val == '' ? 0 : parseFloat(val));
        } else {
            obj.value = val;
        }

        var name = obj.name || '';
        if (name == 'item.appPrice') {
            var price = $(obj).data("appPrice") || {
                min: 0,
                max: 999999999
            };

            if (val < price.min || val > price.max) {
                obj.value = price.min;
            }
        } else {
            var prices = [];

            var array = $('.sku-app-price');

            $.each(array, function (i, o) {
                var price = $(o).val();
                if (reg.test(price)) {
                    prices.push(price);
                }
            });
            var min = lodash.min(prices);
            var max = lodash.max(prices);
            var data = {
                min: min,
                max: max
            };
            var extraData = $(obj).data();
            var skuKey = extraData.skuKey;
            skuDataCache[skuKey].appPrice = obj.value;
            $("input[name='item.appPrice']").val(min).data("appPrice", data);
        }

    }


    this.stockOnKeyup = function (obj) {
        obj.value = obj.value.replace(/\D/g, '');
    }

    this.stockOnBlur = function (obj) {
        var val = obj.value = obj.value.replace(/\D/g, '');
        obj.value = val = val == '' ? 0 : parseInt(val);

        var name = obj.name || '';
        if (name != 'item.stock') {

            var sum = 0;
            var array = $('.sku-stock');

            $.each(array, function (i, o) {
                var stock = $(o).val();
                if (stock != '') {
                    sum += parseInt(stock);
                }
            });
            var extraData = $(obj).data();
            var skuKey = extraData.skuKey;
            skuDataCache[skuKey].stock = obj.value;
            $("input[name='item.stock']").val(sum);
        }

    }


    this.codeOnKeyup = function (obj) {
        obj.value = obj.value.replace(/[^A-Aa-z0-9]/g, '');
    }

    this.imageUploadCallback = function (result) {
        if (result.Status == true) {
            if (isNaN(result.Extra)) {
                propImgCache[result.Extra] = result.Data;
                $("#color-image-" + result.Extra.replace(":", "-")).val(result.Data);
            } else {
                $("#item-img-" + result.Extra).attr("src", result.Data);
                $("#item-img-val-" + result.Extra).val(result.Data);
            }

        } else {
            alert(result.Message);
        }
    };

    this.imageUpload = function (obj, param) {
        var form = document.getElementById('form-upload');

        var html = $.format('<span>选择图片</span><input type="file" name="image" class="input-file" onchange="ayatta.item.imageUpload(this,\'{0}\')" />', param);
        $(obj).parent().html(html);
        $('#frame-upload').html($(obj));
        form.action = '/item/upload/' + param;
        $(form).submit();
    };


    this.submitForm = function (obj) {
        var form = obj.form;
        var action = form.action;
        //var summary = $('#summary').summernote('code');
        //$("#summary").val(summary);
        var param = $(form).serialize();
        $(obj).button('loading');
        $.post(action, param, function (result) {
            //console.log(result);
            if (result.Status) {
                $(obj).button('default');
                //$('#result-message').removeClass('text-danger').addClass('text-success').html('添加商品成功！');
            } else {
                var target = result.Data;
                var targets = ["code", "name", "title", "price", "stock", "summary"];
                $.each(targets, function (i, n) {
                    $("#form-for-" + n).removeClass('has-error');
                });
                var control = "#form-for-" + target;
                if ($.inArray(target, targets) != -1) {
                    $(control).addClass('has-error');
                    $("input[name='item." + target + "']").focus();
                } else {
                    $('#result-message').removeClass('text-success').addClass('text-danger').html(result.Message);
                }
                $(obj).button('default');
            }
        }, 'json');
        return false;
    };
    /*
    this.imageLoad = function (options) {
        var modal = $('<div class="modal fade in"><div class="modal-backdrop fade in"></div><div class="modal-dialog"><div class="modal-content"/></div></div>').appendTo(this.options.appendTo).hide();
        
    }*/

    function propToHtml(p, data) {

        var len = p.values.length;
        if (p.values && p.values.length > 0) {
            var must = '';
            if (p.must == true) {
                must = '<span style="color:red;">*</span>';
            }
            if (p.isEnumProp == true) {

                if (p.multi == true || p.isColorProp == true) {
                    var html = [];
                    var colorNote = '';
                    if (p.isColorProp == true) {
                        colorNote = '<span class="m-l-5 text-danger">请尽量选择已有的颜色！</span>';
                    }

                    html.push('<dl id="container-prop-' + p.id + '"><dt class="m-b-5">' + p.name + ' ' + colorNote + ' ' + must + '</dt><dd>');

                    for (var i = 0; i < len; i++) {
                        var v = p.values[i];
                        if (v.propId == p.id) {
                            var checked = "";
                            for (var j = 0; j < data.props.length; j++) {
                                var k = data.props[j];
                                if (k.pId == p.id && k.vId == v.id) {
                                    checked = 'checked="checked"';
                                    if (p.isSaleProp == true) {
                                        dynamic[v.id] = {
                                            pid: p.id,
                                            vid: v.id,
                                            pname: p.name,
                                            vname: v.name,
                                            color: p.isColorProp
                                        };
                                    }
                                }
                            }

                            if (p.isSaleProp == true) {
                                html.push('<label class="checkbox checkbox-inline m-l-10 m-b-5" ><input type="checkbox" id="prop-' + p.id + '" name="prop.' + p.id + '" value="' + v.id + '" data-pid="' + p.id + '" data-vid="' + v.id + '" data-pname="' + p.name + '" data-vname="' + v.name + '" data-color="' + p.isColorProp + '" onclick="ayatta.item.checkBoxOnClick(this);" ' + checked + ' /><i class="input-helper"></i><span>' + v.name + '</span></label>');
                            } else {
                                html.push('<label class="checkbox checkbox-inline m-l-10 m-b-5" ><input type="checkbox" id="prop-' + p.id + '" name="prop.' + p.id + '" value="' + v.id + '" data-pid="' + p.id + '" data-vid="' + v.id + '" data-pname="' + p.name + '" data-vname="' + v.name + '" data-color="' + p.isColorProp + '" ' + checked + ' /><i class="input-helper"></i><span>' + v.name + '</span></label>');
                            }

                        }
                    }
                    html.push('</dd></dl>');
                    return html.join('');
                } else {

                    var html = [];
                    var search = '';
                    if (p.Id == 20000) {
                        search = 'data-live-search="true"';
                    }
                    html.push('<dl id="container-prop-' + p.id + '"><dt class="m-b-5">' + p.name + ' ' + must + '</dt><dd><select id="prop-' + p.id + '" name="prop.' + p.id + '" ' + search + ' onchange="getChildProp(this,' + p.id + ')">');
                    if (!p.must == true) {
                        html.push('<option value="">请选择</option>');
                    }
                    for (var i = 0; i < len; i++) {
                        var v = p.values[i];
                        if (v.propId == p.id) {
                            var selected = "";

                            for (var j = 0; j < data.props.length; j++) {
                                var k = data.props[j];
                                if (k.pId == p.id && k.vId == v.id) {
                                    selected = 'selected="selected"';
                                    propChildren[p.id] = v.id;
                                }
                            }

                            html.push('<option value="' + v.id + '" ' + selected + ' >' + v.name + '</option>');

                        }
                    }
                    html.push('</select></dd></dl>');
                    return html.join('');
                }

            }

        }
        return '';
    }


    function setSkuHtml() {
        console.log(dynamic);
        var array = [];
        for (var k in dynamic) {
            array.push(dynamic[k]);
        }
        array = array.sort(function (a, b) {
            return a.pid > b.pid ? 1 : -1;
        });

        var keys = [];
        var names = [];
        var colors = [];
        var len = array.length;
        for (var i = 0; i < len; i++) {
            var o = array[i];
            if (o.color == true) {
                colors.push(o);
            }
            if (!lodash.includes(names, o.pname)) {
                names.push(o.pname);
            }
        }

        var group = lodash.groupBy(array, function (n) {
            return n.pid;
        });
        for (var key in group) {
            var array = [];
            var tmp = group[key];
            lodash.forEach(tmp, function (o) {
                array.push(o.pid + ":" + o.vid)
            });
            keys.push(array);
        }

        $("#container-sku").html('');
        var html = getColorTable(colors) + getSkuTable(names, keys);;
        $("#container-sku").html(html);

    }

    function getColorTable(colors) {
        if (colors.length > 0) {
            var html = [];
            html.push('<div class="m-t-15"><p class="m-b-10">颜色图片</p>');
            html.push('<table class="table table-bordered"><thead><tr><th>颜色</th><th>图片（必须上传该颜色对应图片，无图片可不填）</th></thead></tr>');
            for (var key in colors) {
                var color = colors[key];
                var k = color.pid + ':' + color.vid;
                html.push('<tr><td>' + color.vname + '</td><td>');
                html.push('<a class="a-input-file" href="javascript:void(0);"><span>选择图片</span>');

                html.push('<input type="file" name="image" class="input-file" onchange="ayatta.item.imageUpload(this,\'' + k + '\');" />');
                var val = '';
                if (propImgCache[k]) {
                    val = propImgCache[k];
                }
                html.push('</a><input id="color-image-' + color.pid + '-' + color.vid + '" type="hidden" name="color.img.' + k + '" value="' + val + '" />');
                html.push('</td></tr>');
            }
            html.push('</table></div> ');
            return html.join('');
        }
        return '';
    }

    function getSkuTable(names, keys) {

        if (keys.length < saleProps.length) {
            return ('<div class="m-t-15"><p class="m-b-10">商品SKU<span class="m-l-5 text-danger">您需要选择所有的销售属性，才能组合成完整的规格信息！</span></p>');
        }

        var html = [];
        html.push('<div class="m-t-15"><p class="m-b-10">商品SKU</p>');
        var skus = concatAll(keys);
        if (skus && skus.length > 0) {
            var table = [];
            var header = [];
            var body = [];

            table.push('<table class="table table-bordered">');
            header.push('<thead><tr>');
            for (var i in names) {
                header.push('<th width="120">');
                header.push(names[i]);
                header.push('</th>');
            }

            header.push('<th>数 量<span style="color:red;">*</span></th><th >市场价格<span style="color:red;">*</span></th><th width="120">价 格(Pc/Wap)<span style="color:red;">*</span></th><th>价 格(App)<span style="color:red;">*</span></th><th>编 号</th><th>条形码</th>');
            header.push('</tr></thead>');
            body.push('<tbody>');

            for (var i = 0; i < skus.length; i++) {
                body.push('<tr>');
                if (keys.length > 1) {
                    for (var j = 0; j < keys.length; j++) {
                        if (j < keys.length - 1 && i % keys[keys.length - 1].length == 0 && keys[keys.length - 1].length > 0) {
                            var row = 1;
                            var last = 0;
                            for (var r = j + 1; r <= keys.length - 1; r++) {
                                if (r == keys.length - 1) {
                                    last = keys[r].length;
                                } else {
                                    var len = keys[r].length;
                                    if (len > 1) {
                                        row *= len;
                                    } else {
                                        row = 0;
                                        row += len;
                                    }
                                }
                            }
                            if (i % (row * last) == 0) {
                                if ((row * last) > 1) {
                                    body.push('<td rowspan="' + (row * last) + '">');
                                } else {
                                    body.push('<td>');
                                }
                                if (j == 0) {
                                    body.push(getName(keys[j][(i / (row * last))]));
                                } else {
                                    body.push(getName(keys[j][(i / (row * last)) % keys[j].length]));
                                }
                                body.push('</td>');
                            }
                        } else if (j == keys.length - 1) {
                            body.push('<td >');
                            body.push(getName(keys[j][i % keys[j].length]));
                            body.push('</td>');
                        }
                    }
                } else {
                    body.push('<td>');
                    body.push(getName(keys[0][i]));
                    body.push('</td>');
                }

                var stock = 0;
                var retailPrice = 0;
                var price = 0;
                var appPrice = 0;
                var code = '';
                var barcode = '';

                var skuKey = skus[i];
                if (skuDataCache[skuKey]) {
                    code = skuDataCache[skuKey].code;
                    stock = skuDataCache[skuKey].stock;
                    price = skuDataCache[skuKey].price;
                    appPrice = skuDataCache[skuKey].appPrice;
                    retailPrice = skuDataCache[skuKey].retailPrice;
                    barcode = skuDataCache[skuKey].barcode;
                } else {
                    skuDataCache[skuKey] = {};
                    skuDataCache[skuKey].code = code;
                    skuDataCache[skuKey].stock = stock;
                    skuDataCache[skuKey].price = price;
                    skuDataCache[skuKey].appPrice = appPrice;
                    skuDataCache[skuKey].retailPrice = retailPrice;
                    skuDataCache[skuKey].barcode = barcode;
                }

                body.push('<td>');
                var stockInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.stock.' + skuKey + '" value="' + stock + '" maxlength="6" class="form-control input-sm sku-stock" onkeyup="ayatta.item.stockOnKeyup(this);" onblur="ayatta.item.stockOnBlur(this);"/>';
                body.push(stockInput);
                body.push('</td>');

                body.push('<td>');
                var retailPriceInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.retailPrice.' + skuKey + '" value="' + retailPrice + '" maxlength="9" class="form-control input-sm sku-retailPrice" onkeyup="ayatta.item.priceOnKeyup(this);" onblur="ayatta.item.appPriceOnBlur(this);"/>';
                body.push(retailPriceInput);
                body.push('</td>');

                body.push('<td>');
                var priceInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.price.' + skuKey + '" value="' + price + '" maxlength="9" class="form-control input-sm sku-price" onkeyup="ayatta.item.priceOnKeyup(this);" onblur="ayatta.item.priceOnBlur(this);" />';
                body.push(priceInput);
                body.push('</td>');

                body.push('<td>');
                var appPriceInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.appPrice.' + skuKey + '" value="' + appPrice + '" maxlength="9" class="form-control input-sm sku-app-price" onkeyup="ayatta.item.priceOnKeyup(this);" onblur="ayatta.item.appPriceOnBlur(this);" />';
                body.push(appPriceInput);
                body.push('</td>');

                body.push('<td>');
                var codeInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.code.' + skuKey + '" value="' + code + '" maxlength="9" class="form-control input-sm sku-code" onkeyup="ayatta.item.codeOnKeyup(this,0);"/>';
                body.push(codeInput);
                body.push('</td>');

                body.push('<td>');
                var barcodeInput = '<input type="text" data-sku-key="' + skuKey + '" name="sku.barcode.' + skuKey + '" value="' + barcode + '" maxlength="12" class="form-control input-sm sku-barcode" onkeyup="ayatta.item.codeOnKeyup(this,1);" />';
                body.push(barcodeInput);
                body.push('</td>');

                body.push('</tr>');
            }
            body.push('</tbody>');

            table.push(header.join(""));
            table.push(body.join(""));
            html.push(table.join(""));
            html.push('</table>');
        }

        html.push('</div>');
        return html.join("");
    }

    function getName(key) {
        var array = key.split(':');
        var pid = array[0];
        var vid = array[1];
        var val = '';
        for (var i = 0; i < saleProps.length; i++) {
            var p = saleProps[i];
            if (p.id == pid && p.values) {
                for (var i = 0; i < p.values.length; i++) {
                    var v = p.values[i];
                    if (v.id == vid) {
                        val = v.name;
                    }
                }
            }
        }
        return val;
    }

    function concatAll(items) {
        var base = items[0];
        var left = items.slice(1);
        if (left.length) {
            return mulit(base, left);
        } else {
            return base;
        }

        function mulit(base, leftArr) {
            var multiplier = leftArr[0];

            var newBase = [];
            for (var i = 0, len = base.length; i < len; i++) {
                var b = base[i];
                for (var j = 0, len2 = multiplier.length; j < len2; j++) {
                    var m = multiplier[j];
                    newBase.push(b + ';' + m);
                }
            }

            var left = leftArr.slice(1);
            if (left.length) {
                return mulit(newBase, left);
            } else {
                return newBase;
            }
        }
    }
});

