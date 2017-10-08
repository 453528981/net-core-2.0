$.package("app.cart", function () {

    var config = {
        debug: true,
        baseUri: "/cart/"
    };

    //购物车数据
    var cartData = null;

    //待清除商品
    var dataForClean = {
        count: 0,
        skus: [],
        items: [],
        packages: []
    };

    var helpers = {
        isEven: function (i) {
            return i % 2 == 0;
        }
    };

    this.load = function () {
        var path = "data";
        request('load', path, null);
    }

    //增加
    this.increase = function (obj, itemId, skuId) {
        var obj = getProd(itemId, skuId);
        var quantity = obj.quantity + 1;
        if(quantity>obj.max){
            alert('超出库存');
            return;
        }
        if (obj.limited > 0 && quantity >= obj.limited) {
            log("increase", "数量超出限购最大值");
            //数量超出限购最大值
        } else {
            operate(0, itemId, skuId, 1);
        }
    }

    //减少
    this.decrease = function (obj, itemId, skuId) {
        var obj = getProd(itemId, skuId);
        var quantity = obj.quantity - 1;
        if (quantity < 1) {
            this.remove(itemId, skuId, false);//需要提示
        } else {
            operate(1, itemId, skuId, 1);
        }
    }

    //移除
    this.remove = function (obj, itemId, skuId, confirmed) {

        if (confirmed) {
            operate(2, itemId, skuId, 1);
        } else {
            // todo 提示

        }
    }

    //清除商品 disabled true为清除无效的商品
    this.clean = function (disabled, confirmed) {
        setDataForClean(disabled);
        if (dataForClean.count < 1) {
            //请选择
            return;
        }
        if (confirmed) {
            //确认清除
            var path = "clean";
            var skus = dataForClean.skus;//.join(',');
            var items = dataForClean.items;//.join(',');
            var packages = dataForClean.packages;//.join(',');
            request('clean', path, { skus: skus, items: items, packages: packages });
        }
    }

    this.select = function (obj, select, param) {
        obj.disabled = true;

        var path = "select";
        var selected = obj.checked;
        request('select', path, { select: select, param: param, selected: selected });
    }



    this.submit = function (obj) {


        var form = obj.form;
        form.action = config.baseUri + "confirm";
        form.method = "GET";
        form.target = "_blank";
        form.submit();

        //var form = document.createElement('form');
        //form.action = config.baseUri + "confirm";
        //form.method = "POST";
        //form.target = "_blank";
        //var array = document.getElementsByName('skus');
        //var len = array.length;
        //if (len > 0) {
        //    for (var i = 0; i < array.length; i++) {
        //        var o = array[i];
        //        var child = document.createElement('input');
        //        child.name = "skus";
        //        child.type = "hidden";
        //        child.value = o.value;
        //        //child.setAttribute("value", o.value);
        //        form.appendChild(child);
        //    }
        //    form.submit();
        //}
    }

    this.dump = function () {
        return { cartData: cartData, dataForClean: dataForClean };
    }

    function operate(operate, itemId, skuId, quantity) {
        var path = "operate";
        request('operate', path, { operate: operate, itemId: itemId, skuId: skuId, quantity: quantity });
    }

    function request(type, path, param) {
        var url = config.baseUri + path;
        log("请求", url, param);

        $.post(url, param, function (result) {
            log("请求响应", result);
            if (result.code == 0) {

                cartData = result.data;

                dataForClean.count = 0;
                dataForClean.skus = [];
                dataForClean.items = [];
                dataForClean.packages = [];

                render();

            } else {
                onFailed(type, result.code, result.message);
            }
        }, 'json');
    }

    function getProd(itemId, skuId) {
        var bs = cartData.baskets;
        var len = cartData.baskets.length;
        for (var i = 0; i < len; i++) {
            var basket = bs[i];
            if (skuId > 0) {
                var skus = basket.skus;
                for (var j = 0; j < skus.length; j++) {
                    var sku = skus[j];
                    if (sku.id == skuId && sku.itemId == itemId) {
                        return sku;
                    }
                }
            } else {
                var items = basket.items;
                for (var j = 0; j < items.length; j++) {
                    var item = items[j];
                    if (item.id == itemId) {
                        return item;
                    }
                }
            }
        }
    }

    //设置待清除商品
    function setDataForClean(disabled) {
        var count = 0;
        var bs = cartData.baskets;
        var len = cartData.baskets.length;
        for (var i = 0; i < len; i++) {
            var basket = bs[i];

            var skus = basket.skus;
            for (var j = 0; j < skus.length; j++) {
                var sku = skus[j];
                if (disabled && sku.status != 0) {//无效
                    count++;
                    dataForClean.skus.push(sku.id);
                }
                if (!disabled && sku.selected) {
                    count++;
                    dataForClean.skus.push(sku.id);
                }
            }

            var items = basket.items;
            for (var j = 0; j < items.length; j++) {
                var item = items[j];
                if (disabled && item.status != 0) {//无效
                    count++;
                    dataForClean.items.push(item.id);
                }
                if (!disabled && item.selected) {
                    count++;
                    dataForClean.items.push(item.id);
                }
            }
        }
        dataForClean.count = count;
    }

    function onFailed(type, message) {


    }

    function log() {
        if (config.debug) {
            //console.log(arguments)
        }
    }

    function render() {

        $("#cart-container").html(template('cart-tmpl', cartData));
    }
});