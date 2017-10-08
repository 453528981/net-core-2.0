using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{

    [ProtoContract]
    public class ItemMini : IEntity<int>
    {
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

        ///<summary>
        /// 商家设置的外部id
        ///</summary>
        [ProtoMember(7)]
        public string Code { get; set; }

        ///<summary>
        /// 商品名称,不能超过60字节
        ///</summary>
        [ProtoMember(8)]
        public string Name { get; set; }

        ///<summary>
        /// 标题 活动 促销信息
        ///</summary>
        [ProtoMember(9)]
        public string Title { get; set; }

        ///<summary>
        /// 商品库存数量
        ///</summary>
        [ProtoMember(10)]
        public int Stock { get; set; }

        ///<summary>
        /// 商品价格
        ///</summary>
        [ProtoMember(11)]
        public decimal Price { get; set; }

        ///<summary>
        /// app商品价格
        ///</summary>
        [ProtoMember(12)]
        public decimal AppPrice { get; set; }

        ///<summary>
        /// 商品建议零售价格
        ///</summary>
        [ProtoMember(13)]
        public decimal RetailPrice { get; set; }

        ///<summary>
        /// 条形码
        ///</summary>
        [ProtoMember(14)]
        public string Barcode { get; set; }

        ///<summary>
        /// 关键字
        ///</summary>
        [ProtoMember(15)]
        public string Keyword { get; set; }

        ///<summary>
        /// 商品概要
        ///</summary>
        [ProtoMember(16)]
        public string Summary { get; set; }

        ///<summary>
        /// 商品主图片地址
        ///</summary>
        [ProtoMember(17)]
        public string Picture { get; set; }

        ///<summary>
        /// 商品图片列表(包括主图)
        ///</summary>
        [JsonIgnore]
        [ProtoMember(18)]
        public string ItemImgStr { get; set; }

        ///<summary>
        /// 商品属性图片列表
        ///</summary>
        [JsonIgnore]
        [ProtoMember(19)]
        public string PropImgStr { get; set; }

        ///<summary>
        /// 是否为保税仓发货
        ///</summary>
        [ProtoMember(26)]
        public bool IsBonded { get; set; }

        ///<summary>
        /// 是否为海外直邮
        ///</summary>
        [ProtoMember(27)]
        public bool IsOversea { get; set; }

        ///<summary>
        /// 是否定时上架商品
        ///</summary>
        [ProtoMember(28)]
        public bool IsTiming { get; set; }

        ///<summary>
        /// 是否为虚拟物品
        ///</summary>
        [ProtoMember(29)]
        public bool IsVirtual { get; set; }


        ///<summary>
        /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
        ///</summary>
        [JsonIgnore]
        [ProtoMember(47)]
        public string PropStr { get; set; }

        ///<summary>
        /// 商家Id
        ///</summary>
        [ProtoMember(58)]
        public int SellerId { get; set; }

        ///<summary>
        /// 状态 0为可用
        ///</summary>
        [ProtoMember(59)]
        public Prod.Status Status { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(62)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Sku列表
        /// </summary>
        [ProtoMember(100)]
        public virtual IList<Sku> Skus { get; set; }

        /// <summary>
        /// 属性
        /// </summary>            
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
                if (Skus != null && Skus.Any())
                {
                    list.AddRange(Skus.SelectMany(x => x.Props));
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

        [ProtoContract]
        public class Sku : IEntity<int>
        {

            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 商家设置的外部id
            ///</summary>
            [ProtoMember(2)]
            public string Code { get; set; }

            ///<summary>
            /// 库存数量
            ///</summary>
            [ProtoMember(3)]
            public int Stock { get; set; }

            ///<summary>
            /// 价格
            ///</summary>
            [ProtoMember(4)]
            public decimal Price { get; set; }

            ///<summary>
            /// app价格
            ///</summary>
            [ProtoMember(5)]
            public decimal AppPrice { get; set; }

            ///<summary>
            /// 建议零售价格
            ///</summary>
            [ProtoMember(6)]
            public decimal RetailPrice { get; set; }

            ///<summary>
            /// 条形码
            ///</summary>
            [ProtoMember(7)]
            public string Barcode { get; set; }

            ///<summary>
            /// 商品属性Id 格式：pid:vid;pid:vid
            ///</summary>
            //[JsonIgnore]
            [ProtoMember(8)]
            public string PropId { get; set; }

            ///<summary>
            /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
            ///</summary>
            [JsonIgnore]
            [ProtoMember(9)]
            public string PropStr { get; set; }

            ///<summary>
            /// 状态 0为可用
            ///</summary>
            [ProtoMember(10)]
            public Prod.Status Status { get; set; }

            [JsonIgnore]
            public virtual IList<Prod.Prop> Props
            {
                get
                {
                    var list = new List<Prod.Prop>(7);
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
                                prop.Extra = "sku";
                                list.Add(prop);
                            }
                        }
                    }
                    return list;
                }
            }

            [JsonIgnore]
            public virtual string[] PropTexts
            {
                get
                {
                    return !string.IsNullOrEmpty(PropStr) ? PropStr.Split(';').Select(o => o.Split(':')[2].Trim() + "：" + o.Split(':')[3].Trim()).ToArray() : null;
                }
            }
        }
    }
}