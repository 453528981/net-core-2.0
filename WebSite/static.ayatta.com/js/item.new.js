$.package('item.new', function () {
    var props = [];//子属性
    var dynamic = {};//选中的销售属性值
    var saleProps = [];//销售属性
    this.getProp = function (catgId) {
        $.getJSON('/global/catg/' + catgId, function (r) {
            var propHtml = [];
            var keyPropHtml = [];
            var salePropHtml = [];
            var len = r.Props.length;
            r.Props = r.Props.sort(function (a, b) { return a.Priority > b.Priority ? 1 : -1; });
            for (var i = 0; i < len; i++) {
                var p = r.Props[i];
                if (p.ParentPid == 0 && p.Values) {
                    p.Values = p.Values.sort(function (a, b) { return a.Priority > b.Priority ? 1 : -1; });
                    if (p.IsKeyProp == true) {
                        keyPropHtml.push('<div>');
                        keyPropHtml.push(propToHtml(p));
                        keyPropHtml.push('</div>');
                    } else if (p.IsSaleProp == true) {
                        saleProps.push(p);
                        salePropHtml.push('<div>');
                        salePropHtml.push(propToHtml(p));
                        salePropHtml.push('</div>');
                    } else {
                        propHtml.push('<div>');
                        propHtml.push(propToHtml(p));
                        propHtml.push('</div>');
                    }
                } else {
                    props.push(p);
                }
            }
            $("#container-prop").html(propHtml.join(''));
            $("#container-keyProp").html(keyPropHtml.join(''));
            $("#container-saleProp").html(salePropHtml.join(''));

            $('select').selectpicker();

            $('#desc').summernote({
                height: 400,
                lang: 'zh-CN'
            });
        });
    };


    //hack selectpicker 无法调用 item.new.getChildren
    this.getChildren = function (obj, id) {

        var container = $("#container-prop-" + id);
        container.nextAll().remove();

        var child = null;
        var val = $(obj).val();

        var len = props.length;

        for (var i = 0; i < len; i++) {
            var o = props[i];
            if (id == o.ParentPid && val == o.ParentVid) {
                child = o;
                break;
            }
        }

        if (child != null) {
            var html = propToHtml(child);
            container.parent().append(html);
            $('#prop-' + child.Id).selectpicker();
        }
    };

    this.checkBoxOnClick = function (obj) {
        var key = obj.value;
        if (obj.checked) {
            var data = $(obj).data();
            dynamic[key] = data
        } else {
            delete (dynamic[key])
        }

        var array = [];
        for (var k in dynamic) {
            array.push(dynamic[k]);
        }
        array = array.sort(function (a, b) { return a.pid > b.pid ? 1 : -1; });

        $("#container-sku").html('');
        var html = getSkuHtml(array);
        $("#container-sku").html(html);
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
        }
        else {
            obj.value = val;
        }

        var name = obj.name || '';
        if (name == 'item.price') {
            var price = $(obj).data("price") || { min: 0, max: 999999999 };

            if (val < price.min || val > price.max) {
                obj.value = price.min;
            }
        }
        else {
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
            var data = { min: min, max: max };
            $("input[name='item.price']").val(min).data("price", data);
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
            $("input[name='item.stock']").val(sum);
        }
    }

    this.imageUploadCallback = function (result) {
        if (result.Status == true) {
            console.log(result);
            //var data = result.Data;
            //$('#prop-image-' + result.Extra).val(data);
            //var src = "/temp/" + data;
            //$('#preview-' + result.Extra).attr('src', src);
        } else {
            alert(result.Message);
        }
    };

    this.imageUpload = function (obj, param) {
        var form = document.getElementById('form-upload');

        var html = $.format('<span>选择图片</span><input type="file" name="image" class="input-file" onchange="item.new.imageUpload(this,\'{0}\')" />', param);
        $(obj).parent().html(html);
        $('#frame-upload').html($(obj));
        form.action = '/item/upload/' + param;
        $(form).submit();
    };


    this.submitForm = function (obj) {
        var form = obj.form;
        var action = form.action;
        var desc = $('#desc').summernote('code');
        $("#desc").val(desc);
        var param = $(form).serialize();
        $(obj).button('loading');
        $.post(action, param, function (result) {
            console.log(result);
            if (result.Status) {

                //$('#result-message').removeClass('text-danger').addClass('text-success').html('添加商品成功！');
            } else {
                var target = result.Data;
                var targets = ["code", "name", "title", "price", "stock", "desc"];
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


    function propToHtml(p) {

        var len = p.Values.length;
        if (p.Values && p.Values.length > 0) {
            var must = '';
            if (p.Must == true) {
                must = '<span style="color:red;">*</span>';
            }
            if (p.IsEnumProp == true) {

                if (p.Multi == true || p.IsColorProp == true) {
                    var html = [];
                    var colorNote = '';
                    if (p.IsColorProp == true) {
                        colorNote = '<span class="m-l-5 text-danger">请尽量选择已有的颜色！</span>';
                    }

                    html.push('<dl id="container-prop-' + p.Id + '"><dt class="m-b-5">' + p.Name + ' ' + colorNote + ' ' + must + '</dt><dd>');

                    for (var i = 0; i < len; i++) {
                        var v = p.Values[i];
                        if (v.PropId == p.Id) {
                            if (p.IsSaleProp == true) {
                                html.push('<label class="checkbox checkbox-inline m-l-10 m-b-5" ><input type="checkbox" id="prop-' + p.Id + '" name="prop.' + p.Id + '" value="' + v.Id + '" data-pid="' + p.Id + '" data-vid="' + v.Id + '" data-pname="' + p.Name + '" data-vname="' + v.Name + '" data-color="' + p.IsColorProp + '" onclick="item.new.checkBoxOnClick(this);" /><i class="input-helper"></i><span>' + v.Name + '</span></label>');
                            } else {
                                html.push('<label class="checkbox checkbox-inline m-l-10 m-b-5" ><input type="checkbox" id="prop-' + p.Id + '" name="prop.' + p.Id + '" value="' + v.Id + '" data-pid="' + p.Id + '" data-vid="' + v.Id + '" data-pname="' + p.Name + '" data-vname="' + v.Name + '" data-color="' + p.IsColorProp + '" /><i class="input-helper"></i><span>' + v.Name + '</span></label>');
                            }
                        }
                    }
                    html.push('</dd></dl>');
                    return html.join('');
                }
                else {

                    var html = [];
                    var search = '';
                    if (p.Id == 20000) {
                        search = 'data-live-search="true"'
                    }
                    html.push('<dl id="container-prop-' + p.Id + '"><dt class="m-b-5">' + p.Name + ' ' + must + '</dt><dd><select id="prop-' + p.Id + '" name="prop.' + p.Id + '" ' + search + ' onchange="getChildren(this,' + p.Id + ')">');
                    if (!p.Must == true) {
                        html.push('<option value="">请选择</option>');
                    }
                    for (var i = 0; i < len; i++) {
                        var v = p.Values[i];
                        if (v.PropId == p.Id) {

                            html.push('<option value="' + v.Id + '">' + v.Name + '</option>');
                        }
                    }
                    html.push('</select></dd></dl>');
                    return html.join('');
                }

            }

        }
        return '';
    }


    function getSkuHtml(array) {
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

        var group = lodash.groupBy(array, function (n) { return n.pid; });
        for (var key in group) {
            var array = [];
            var tmp = group[key];
            lodash.forEach(tmp, function (o) { array.push(o.pid + ":" + o.vid) });
            keys.push(array);
        }

        return getColorTable(colors) + getSkuTable(names, keys);
    }

    function getColorTable(colors) {
        if (colors.length > 0) {
            var html = [];
            html.push('<div class="m-t-15"><p class="m-b-10">颜色图片</p>');
            html.push('<table class="table table-bordered"><thead><tr><th>颜色</th><th>图片（必须上传该颜色对应图片，无图片可不填）</th></thead></tr>');
            for (var key in colors) {
                var color = colors[key];
                html.push('<tr><td>' + color.vname + '</td><td>');
                html.push('<a class="a-input-file" href="javascript:void(0);"><span>选择图片</span>');

                html.push('<input type="file" name="image" class="input-file" onchange="item.new.imageUpload(this,\'' + color.pid + ':' + color.vid + '\');" />');

                html.push('</a><input id="color-image-' + color.pid + ':' + color.vid + '" type="hidden" name="color.img.' + color.pid + ':' + color.vid + '" />');
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
                header.push('<th>');
                header.push(names[i]);
                header.push('</th>');
            }

            header.push('<th>价 格<span style="color:red;">*</span></th><th>数 量<span style="color:red;">*</span></th>');
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
                                    body.push(getName(keys[j][(i / (row * last)) % Keys[j].length]));
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
                    body.push((keys[0][i]));
                    body.push('</td>');
                }
                body.push('<td>');
                var id = skus[i];
                var priceInput = '<input type="text" name="sku.price.' + id + '" maxlength="9" class="form-control input-small sku-price" onkeyup="item.new.priceOnKeyup(this);" onblur="item.new.priceOnBlur(this);" />';
                body.push(priceInput);
                body.push('</td>');
                body.push('<td>');
                var stockInput = '<input type="text" name="sku.stock.' + id + '" maxlength="6" class="form-control input-sm input-mini sku-stock" onkeyup="item.new.stockOnKeyup(this);" onblur="item.new.stockOnBlur(this);"/>';
                body.push(stockInput);
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
            if (p.Id == pid && p.Values) {
                for (var i = 0; i < p.Values.length; i++) {
                    var v = p.Values[i];
                    if (v.Id == vid) {
                        val = v.Name;
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