$.package("app.item", function () {
    var stock = 0;
    var scope = {};
    var cartBaseUrl = "/cart/";

    this.go = function (data) {
        scope = data;
        stock = data.stock;

        var keys = getKeys(data.skus)
        scope.skus = getSkus(keys, data.skus);
        scope.skuCount = keys.length;
        scope.propCount = getKeys(data.props).length;
        scope.quantity = 1;
        scope.selecteds = {};
        scope.disableds = {};
        if (scope.skuCount > 0) {
            render();
        }
    }

    this.onPropClick = function (k, v) {
        console.log(k, v);
        for (var key in scope.props) {
            var value = scope.props[key];
            for (var x in value) {
                if (key == k) {
                    if (key == k) {
                        if (x == v) {
                            if (scope.selecteds[x]) {
                                delete scope.selecteds[x];
                            } else {
                                scope.selecteds[x] = k;
                            }
                        } else {
                            delete scope.selecteds[x];
                        }
                    }
                } else {
                    delete scope.disableds[x];
                }
            }
        }

        var selecteds = [];
        for (var key in scope.selecteds) {
            selecteds.push(key);
        }
        selecteds.sort(sort);

        for (var key in scope.selecteds) {
            for (var k in scope.props) {
                if (scope.selecteds[key] != k) {
                    var value = scope.props[k];
                    for (var x in value) {
                        var v = [key, x]
                        v.sort(sort);
                        var i = v.join(";");
                        if (!scope.skus[i] || scope.skus[i].count < 1) {
                            scope.disableds[x] = true;
                        } else {
                            delete scope.disableds[x];
                        }
                    }
                }
            }
        }


        if (selecteds.length > 0) {
            var sku = scope.skus[selecteds.join(";")];
            scope.stock = sku.count;
            var prices = sku.prices;
            var minPrice = Math.min.apply(Math.min, prices);
            var maxPrice = Math.max.apply(Math.max, prices);
            if (minPrice == maxPrice) {
                scope.priceText = minPrice.toFixed(2);
            } else {
                scope.priceText = minPrice.toFixed(2) + '-' + maxPrice.toFixed(2);
            }
            if (selecteds.length == scope.propCount) {
                scope.skuId = sku.id;
            }
        } else {
            scope.stock = stock;//总库存
        }

        render();
    }

    this.addToCart = function (obj) {

        var url = cartBaseUrl + "operate";
        var param = { operate: 0, itemId: scope.id, quantity: scope.quantity };
        if (scope.skuCount > 0) {
            var selecteds = getKeys(scope.selecteds).length;
            if (selecteds != scope.propCount) {
                $("#item-props").addClass('item-sku-attention');
                return;
            } else {
                param.skuId = scope.skuId;
            }

            if (scope.quantity > scope.stock) {
                alert('修改数量');
                return;
            }
        }
        $.getJSON(url, param, function (r) {
            if (r.code != 0) {
                alert(r.message);
            }
        });
    }

    this.onQuantityChange = function (obj) {
        var val = obj.value = obj.value.replace(/\D/g, '');
        if (val < 1) {
            obj.value = 1;
            return;
        }
        if (parseInt(val) > scope.stock) {
            obj.value = scope.stock;
            scope.quantity = scope.stock;
        } else {
            scope.quantity = val;
        }
    }

    this.onQuantityMinus = function () {
        var val = $("#item-quantity").val();
        if (isNaN(val)) {
            return;
        }
        var i = parseInt(val) - 1;
        if (i > 0 && i <= scope.stock) {
            scope.quantity = i;
            $("#item-quantity").val(i);
        }
    }

    this.onQuantityPlus = function () {
        var val = $("#item-quantity").val();
        if (isNaN(val)) {
            return;
        }
        var i = parseInt(val) + 1;
        if (i > 0 && i <= scope.stock) {
            scope.quantity = i;
            $("#item-quantity").val(i);
        }
    }

    this.dump = function () {
        return scope;
    }

    function render() {

        var array = [];
        array.push('<dl class="dl-horizontal">');
        for (var k in scope.props) {
            var v = scope.props[k];

            array.push('<dt>' + k + '</dt>');
            array.push('<dd><ul class="list-inline">');
            for (var x in v) {

                var cls = scope.selecteds[x] ? "item-sku-prop-selected" : "";
                var click = $.format("app.item.onPropClick('{0}','{1}')", k, x);
                if (scope.disableds[x]) {
                    cls = "item-sku-prop-disabled";
                    click = "javascript:void(0);";
                }
                array.push('<li class="item-sku-prop ' + cls + '">');
                if (scope.propImgs[x]) {
                    var style = $.format("background: #FFFFFF url({0}) no-repeat scroll center center", scope.propImgs[x] + "?width=36&height=36");
                    array.push($.format('<a class="item-sku-prop-img zoom" href="javascript:void(0);" onclick="{0}" style="{1}"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span></a>', click, style));
                } else {
                    array.push($.format('<a href="javascript:void(0);" onclick="{0}"><span>{1}</span></a>', click, v[x]));
                }
                array.push('<i></i></li>');
            }
            array.push('</ul></dd>');
        }
        array.push('</dl>');

        var html = array.join('');
        $("#item-props").html(html);
        $("#item-stock").text(scope.stock);
        $("#item-price").text(scope.priceText);

        var quantity = $("#item-quantity").val();
        if (isNaN(quantity)) {
            return;
        }
        var qty = parseInt(quantity);
        if (qty > scope.stock) {
            scope.quantity = scope.stock;
            $("#item-quantity").val(scope.stock);
        }
    }

    function getSkus(keys, skus) {
        var skuResult = {};

        function addToSkuResult(key, sku) {
            if (skuResult[key]) {//SKU信息key属性·
                skuResult[key].count += sku.count;
                skuResult[key].prices.push(sku.price);
            } else {
                skuResult[key] = {
                    id: sku.id,
                    count: sku.count,
                    prices: [sku.price]
                };
            }
        }

        //对一条SKU信息进行拆分组合
        function combineSku(skuKeyAttrs, cnum, sku) {
            var len = skuKeyAttrs.length;
            for (var i = 0; i < len; i++) {
                var key = skuKeyAttrs[i];
                for (var j = i + 1; j < len; j++) {
                    if (j + cnum <= len) {
                        var attr = skuKeyAttrs.slice(j, j + cnum);   //安装组合个数获得属性值·
                        var genKey = key + ";" + attr.join(";");     //得到一个组合key
                        addToSkuResult(genKey, sku);
                    }
                }
            }
        }

        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];  //一条SKU信息key
            var sku = skus[key];    //一条SKU信息value
            var skuKeyAttrs = key.split(";");   //SKU信息key属性值数组
            var len = skuKeyAttrs.length;

            //对每个SKU信息key属性值进行拆分组合
            for (var j = 0; j < len; j++) {
                //单个属性值作为key直接放入SKUResult
                addToSkuResult(skuKeyAttrs[j], sku);
                //对本组SKU信息key属性进行组合，组合个数为j
                (j > 0 && j < len - 1) && combineSku(skuKeyAttrs, j, sku);
            }

            //结果集接放入SKUResult
            skuResult[key] = {
                id: sku.id,
                count: sku.count,
                prices: [sku.price]
            };
        }
        return skuResult;
    }

    function sort(a, b) {
        return parseInt(a) - parseInt(b);
    }

    function getKeys(obj) {
        var keys = [];
        if (typeof (obj) === "object") {
            for (var k in obj) {
                keys.push(k);
            }
        }
        return keys;
    }

    function isSelected(prop) {
        var len = scope.selectedProps || [];
        for (var i = 0; i < len; i++) {
            var item = scope.selectedProps[i];
            if (item == prop || scope.selecteds[x]) {
                return true;
            }
        }
        return false;
    }

    function bindEvent() {
        var ez = $("#zoom").elevateZoom({ gallery: ['gallery-zoom-thumb'], cursor: 'move', zoomWindowWidth: 440, zoomWindowHeight: 440, zoomWindowOffetx: 20 });

        $('.item-sku-prop-img').click(function () {
            if (!$(this).parent().hasClass('item-sku-prop-disabled')) {
                var img = $(this).data("image");
                var temp = ez.data('elevateZoom');

                temp.elem.src = img;

                temp.options.zoomEnabled = false;
                $('.zoom-active').removeClass('zoom-active');
            }
        });
    }
});