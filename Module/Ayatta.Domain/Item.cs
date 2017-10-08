using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    ///<summary>
    /// 商品
    ///</summary>
    [ProtoContract]
    public class Item : IEntity<int>
    {
        #region
        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 产品Id
        ///</summary>
        [ProtoMember(2)]
        public int SpuId { get; set; }

        ///<summary>
        /// 最小类目Id
        ///</summary>
        [ProtoMember(3)]
        public int CatgId { get; set; }

        ///<summary>
        /// 根类目Id
        ///</summary>
        [ProtoMember(4)]
        public int CatgRId { get; set; }

        ///<summary>
        /// 品牌Id
        ///</summary>
        [ProtoMember(5)]
        public int BrandId { get; set; }

        ///<summary>
        /// 品牌名
        ///</summary>
        [ProtoMember(6)]
        public string BrandName { get; set; }

        /// <summary>
        /// 品牌首字母
        /// </summary>
        [ProtoMember(7)]
        public string BrandInitial { get; set; }

        ///<summary>
        /// 全类目Id
        ///</summary>
        [ProtoMember(8)]
        public string CatgFId { get; set; }

        ///<summary>
        /// 全类目名称
        ///</summary>
        [ProtoMember(9)]
        public string CatgFName { get; set; }

        ///<summary>
        /// 商家设置的外部Id
        ///</summary>
        [ProtoMember(10)]
        public string Code { get; set; }

        ///<summary>
        /// 商品名称,不能超过60字节
        ///</summary>
        [ProtoMember(11)]
        public string Name { get; set; }

        ///<summary>
        /// 标题 活动 促销信息
        ///</summary>
        [ProtoMember(12)]
        public string Title { get; set; }

        #region Stock
        private int stock;
        ///<summary>
        /// 商品库存数量
        ///</summary>
        [ProtoMember(13)]
        public int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                if (Skus != null && Skus.Any())
                {
                    stock = Skus.Where(x => x.Status == Prod.Status.Online).Sum(x => x.Stock);
                }
                else
                {
                    stock = value;
                }
            }
        }
        #endregion

        ///<summary>
        /// 商品价格
        ///</summary>
        [ProtoMember(14)]
        public decimal Price { get; set; }

        #region AppPrice
        private decimal appPrice;

        ///<summary>
        /// app商品价格
        ///</summary>
        [ProtoMember(15)]
        public decimal AppPrice
        {
            get
            {
                return appPrice;
            }
            set
            {
                if (value <= 0)
                {
                    appPrice = Price;
                }
                else
                {
                    appPrice = value;
                }
            }
        }
        #endregion

        ///<summary>
        /// 商品建议零售价格
        ///</summary>
        [ProtoMember(16)]
        public decimal RetailPrice { get; set; }

        ///<summary>
        /// 条形码
        ///</summary>
        [ProtoMember(17)]
        public string Barcode { get; set; }

        ///<summary>
        /// 关键字
        ///</summary>
        [ProtoMember(18)]
        public string Keyword { get; set; }

        ///<summary>
        /// 商品概要
        ///</summary>
        [ProtoMember(19)]
        public string Summary { get; set; }

        ///<summary>
        /// 商品主图片地址
        ///</summary>
        [ProtoMember(20)]
        public string Picture { get; set; }

        ///<summary>
        /// 商品图片列表(包括主图)
        ///</summary>
        [JsonIgnore]
        [ProtoMember(21)]
        public string ItemImgStr { get; set; }

        ///<summary>
        /// 商品属性图片列表
        ///</summary>
        [JsonIgnore]
        [ProtoMember(22)]
        public string PropImgStr { get; set; }

        ///<summary>
        /// 宽度
        ///</summary>
        [ProtoMember(23)]
        public decimal Width { get; set; }

        ///<summary>
        /// 深度
        ///</summary>
        [ProtoMember(24)]
        public decimal Depth { get; set; }

        ///<summary>
        /// 高度
        ///</summary>
        [ProtoMember(25)]
        public decimal Height { get; set; }

        ///<summary>
        /// 重量
        ///</summary>
        [ProtoMember(26)]
        public decimal Weight { get; set; }

        ///<summary>
        /// 商品所在国家
        ///</summary>
        [ProtoMember(27)]
        public string Country { get; set; } = string.Empty;

        ///<summary>
        /// 商品所在地(城市)Code
        ///</summary>
        [ProtoMember(28)]
        public string Location { get; set; } = string.Empty;

        ///<summary>
        /// 是否为保税仓发货
        ///</summary>
        [ProtoMember(29)]
        public bool IsBonded { get; set; }

        ///<summary>
        /// 是否为海外直邮
        ///</summary>
        [ProtoMember(30)]
        public bool IsOversea { get; set; }

        ///<summary>
        /// 是否定时上架商品
        ///</summary>
        [ProtoMember(31)]
        public bool IsTiming { get; set; }

        ///<summary>
        /// 是否为虚拟物品
        ///</summary>
        [ProtoMember(32)]
        public bool IsVirtual { get; set; }

        ///<summary>
        /// 代充商品类型 可选类型： timecard(点卡软件代充) feecard(话费软件代充)
        ///</summary>
        [ProtoMember(33)]
        public bool IsAutoFill { get; set; }

        ///<summary>
        /// 是否支持货到付款
        ///</summary>
        [ProtoMember(34)]
        public bool SupportCod { get; set; }

        ///<summary>
        /// 是否免运费
        ///</summary>
        [ProtoMember(35)]
        public bool FreightFree { get; set; }

        ///<summary>
        /// 运费模板Id
        ///</summary>
        [ProtoMember(36)]
        public int FreightTplId { get; set; }

        ///<summary>
        /// 0为拍下减库存 1为付款减库存
        ///</summary>
        [ProtoMember(37)]
        public byte SubStock { get; set; }

        ///<summary>
        /// 橱窗推荐
        ///</summary>
        [ProtoMember(38)]
        public int Showcase { get; set; }

        ///<summary>
        /// 上架时间
        ///</summary>
        [ProtoMember(39)]
        public DateTime OnlineOn { get; set; }

        ///<summary>
        /// 下架时间
        ///</summary>
        [ProtoMember(40)]
        public DateTime OfflineOn { get; set; }

        ///<summary>
        /// 积分奖励
        ///</summary>
        [ProtoMember(41)]
        public decimal RewardRate { get; set; }

        ///<summary>
        /// 是否有发票
        ///</summary>
        [ProtoMember(42)]
        public bool HasInvoice { get; set; }

        ///<summary>
        /// 是否有保修
        ///</summary>
        [ProtoMember(43)]
        public bool HasWarranty { get; set; }

        ///<summary>
        /// 是否承诺退换货服务
        ///</summary>
        [ProtoMember(44)]
        public bool HasGuarantee { get; set; }

        ///<summary>
        /// 销售数量
        ///</summary>
        [ProtoMember(50)]
        public int SaleCount { get; set; }

        ///<summary>
        /// 收藏数量
        ///</summary>
        [ProtoMember(51)]
        public int CollectCount { get; set; }

        ///<summary>
        /// 咨询数量
        ///</summary>
        [ProtoMember(52)]
        public int ConsultCount { get; set; }

        ///<summary>
        /// 评论数量
        ///</summary>
        [ProtoMember(53)]
        public int CommentCount { get; set; }

        ///<summary>
        /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
        ///</summary>
        [JsonIgnore]
        [ProtoMember(60)]
        public string PropStr { get; set; } = string.Empty;

        ///<summary>
        /// 属性值别名,比如颜色的自定义名称 1627207:28335:草绿;1627207:3232479:深紫
        ///</summary>
        [JsonIgnore]
        [ProtoMember(61)]
        public string PropAlias { get; set; } = string.Empty;

        ///<summary>
        /// 商品输入属性Id
        ///</summary>
        [JsonIgnore]
        [ProtoMember(62)]
        public string InputId { get; set; } = string.Empty;

        ///<summary>
        /// 商品输入属性值
        ///</summary>
        [JsonIgnore]
        [ProtoMember(63)]
        public string InputStr { get; set; } = string.Empty;


        ///<summary>
        /// 用于搜索的商品属性Id 格式：pid:vid;pid:vid
        ///</summary>
        [JsonIgnore]
        [ProtoMember(64)]
        public string SearchPropId { get; set; } = string.Empty;

        ///<summary>
        /// 用于搜索的商品属性项/值
        ///</summary>
        [JsonIgnore]
        [ProtoMember(65)]
        public string SearchPropStr { get; set; } = string.Empty;

        ///<summary>
        /// Meta
        ///</summary>
        [ProtoMember(70)]
        public string Meta { get; set; } = string.Empty;

        ///<summary>
        /// Label
        ///</summary>
        [ProtoMember(71)]
        public string Label { get; set; } = string.Empty;

        ///<summary>
        /// 相关关联的
        ///</summary>
        [ProtoMember(72)]
        public string Related { get; set; } = string.Empty;

        ///<summary>
        /// 优先级
        ///</summary>
        [ProtoMember(80)]
        public int Prority { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        [ProtoMember(81)]
        public int SellerId { get; set; }

        /// <summary>
        /// 卖家用户名
        /// </summary>
        [ProtoMember(82)]
        public string SellerName { get; set; }

        ///<summary>
        /// 状态 0为可用
        ///</summary>
        [ProtoMember(83)]
        public Prod.Status Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(84)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(85)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(86)]
        public DateTime ModifiedOn { get; set; }

        #endregion

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <returns></returns>
        [ProtoMember(100)]
        public virtual ItemDesc Desc { get; set; }

        ///<summary>
        /// Skus
        ///</summary>
        [ProtoMember(101)]
        public virtual IList<Sku> Skus { get; set; }

        #region Readonly                

        /// <summary>
        /// 属性
        /// </summary>
        [JsonIgnore]
        public virtual IList<Prod.Prop> Props
        {
            get
            {
                var list = new List<Prod.Prop>();
                if (!string.IsNullOrEmpty(PropStr))
                {
                    var array = PropStr.Split(';');
                    foreach (var s in array)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            var temp = s.Split(':');
                            var prop = new Prod.Prop();
                            prop.PId = Convert.ToInt32(temp[0]);
                            prop.VId = Convert.ToInt32(temp[1]);
                            prop.PName = temp[2];
                            prop.VName = temp[3];

                            list.Add(prop);
                        }
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 属性文本
        /// </summary>
        public virtual string[] PropTexts
        {
            get
            {
                var props = Props;
                var list = new List<string>(props.Count);
                foreach (var p in props.GroupBy(x => x.PId))
                {
                    var o = p.FirstOrDefault(x => x.PId == p.Key);
                    list.Add(o.PName + "：" + string.Join("，", p.Select(x => x.VName).ToArray()));
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual IList<string> Imgs
        {
            get
            {
                var list = new List<string>(5);
                if (!string.IsNullOrEmpty(ItemImgStr))
                {
                    list.AddRange(ItemImgStr.Split(';').Where(x => !string.IsNullOrEmpty(x)));
                }
                return list;
            }
        }

        /// <summary>
        /// 颜色图片
        /// </summary>
        public virtual IDictionary<string, string> PropImgs
        {
            get
            {
                var dic = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(PropImgStr))
                {
                    dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(PropImgStr);
                }
                return dic;
            }
        }

        /// <summary>
        /// Sku属性
        /// </summary>

        public virtual IDictionary<string, IDictionary<string, string>> SkuProps
        {
            get
            {
                var dic = new Dictionary<string, IDictionary<string, string>>();
                if (Skus != null && Skus.Any())
                {
                    foreach (var sku in Skus)
                    {
                        var list = sku.Props.OrderBy(x => x.PId);

                        foreach (var o in list.GroupBy(o => o.PId))
                        {
                            var key = o.First(x => x.PId == o.Key).PName;

                            var temp = o.ToDictionary(k => (o.Key + ":" + k.VId), v => v.VName);
                            if (dic.ContainsKey(key))
                            {
                                foreach (var t in from t in temp let x = dic[key] where !x.ContainsKey(t.Key) select t)
                                {
                                    dic[key].Add(t.Key, t.Value);
                                }
                            }
                            else
                            {
                                dic[key] = temp;
                            }
                        }
                    }
                }
                return dic;
            }
        }
        #endregion

        #region        

        /// <summary>
        /// 获取商品零售价格 有多个sku时 返回价格区间
        /// </summary>
        /// <returns></returns>
        public string GetRetailPriceText()
        {
            if (Skus != null && Skus.Any())
            {
                var min = Skus.Min(x => x.RetailPrice);
                var max = Skus.Max(x => x.RetailPrice);
                return min.ToString("F2") + "-" + max.ToString("F2");
            }
            return RetailPrice.ToString("F2");
        }

        /// <summary>
        /// 获取商品价格 有多个sku时 返回价格区间
        /// </summary>
        /// <param name="platform">平台</param>
        /// <returns></returns>
        public string GetPriceText(Platform platform)
        {
            if (platform == Platform.Pc || platform == Platform.Wap)
            {
                if (Skus != null && Skus.Any())
                {
                    var min = Skus.Min(x => x.Price);
                    var max = Skus.Max(x => x.Price);
                    return min.ToString("F2") + "-" + max.ToString("F2");
                }
                return Price.ToString("F2");
            }
            else if (platform == Platform.App)
            {
                if (Skus != null && Skus.Any())
                {
                    var min = Skus.Min(x => x.AppPrice);
                    var max = Skus.Max(x => x.AppPrice);
                    return min.ToString("F2") + "-" + max.ToString("F2");
                }
                return AppPrice.ToString("F2");
            }
            return string.Empty;
        }
        #endregion

        public class Tiny:IEntity
        {
            /// <summary>
            /// Id
            /// </summary>
            public int Id { get; set; }

            ///<summary>
            /// 最小类目Id
            ///</summary>
            public int CatgId { get; set; }

            ///<summary>
            /// 品牌Id
            ///</summary>
            public int BrandId { get; set; }

            ///<summary>
            /// 品牌名
            ///</summary>
            public string BrandName { get; set; }

            ///<summary>
            /// 商家设置的外部Id
            ///</summary>
            public string Code { get; set; }

            ///<summary>
            /// 商品名称,不能超过60字节
            ///</summary>
            public string Name { get; set; }


            #region Stock
            ///<summary>
            /// 商品库存数量
            ///</summary>
            public int Stock { get; set; }
            #endregion

            ///<summary>
            /// 商品价格
            ///</summary>
            [ProtoMember(11)]
            public decimal Price { get; set; }

            #region AppPrice
            private decimal appPrice;

            ///<summary>
            /// app商品价格
            ///</summary>
            [ProtoMember(12)]
            public decimal AppPrice
            {
                get
                {
                    return appPrice;
                }
                set
                {
                    if (value <= 0)
                    {
                        appPrice = Price;
                    }
                    else
                    {
                        appPrice = value;
                    }
                }
            }
            #endregion

            ///<summary>
            /// 商品建议零售价格
            ///</summary>            
            public decimal RetailPrice { get; set; }

            ///<summary>
            /// 条形码
            ///</summary>
            public string Barcode { get; set; }

            ///<summary>
            /// 关键字
            ///</summary>
            public string Keyword { get; set; }

            ///<summary>
            /// 商品概要
            ///</summary>
            public string Summary { get; set; }

            ///<summary>
            /// 商品主图片地址
            ///</summary>
            public string Picture { get; set; }

        }
    }
}