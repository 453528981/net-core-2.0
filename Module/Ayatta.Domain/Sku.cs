using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{

    ///<summary>
    /// 商品Sku
    ///</summary>
    [ProtoContract]
    public class Sku : IEntity<int>
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
        /// 商品Id
        ///</summary>
        [ProtoMember(3)]
        public int ItemId { get; set; }

        ///<summary>
        /// 最小类目Id
        ///</summary>
        [ProtoMember(5)]
        public int CatgId { get; set; }

        ///<summary>
        /// 根类目Id
        ///</summary>
        [ProtoMember(4)]
        public int CatgRId { get; set; }

        ///<summary>
        /// 品牌Id
        ///</summary>
        [ProtoMember(6)]
        public int BrandId { get; set; }

        ///<summary>
        /// 商家设置的外部id
        ///</summary>
        [ProtoMember(7)]
        public string Code { get; set; }

        ///<summary>
        /// 库存数量
        ///</summary>
        [ProtoMember(8)]
        public int Stock { get; set; }

        ///<summary>
        /// 价格
        ///</summary>
        [ProtoMember(9)]
        public decimal Price { get; set; }

        #region AppPrice
        private decimal appPrice;

        ///<summary>
        /// app商品价格
        ///</summary>
        [ProtoMember(10)]
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
        /// 建议零售价格
        ///</summary>
        [ProtoMember(11)]
        public decimal RetailPrice { get; set; }

        ///<summary>
        /// 条形码
        ///</summary>
        [ProtoMember(12)]
        public string Barcode { get; set; }

        ///<summary>
        /// 商品属性Id 格式：pid:vid;pid:vid
        ///</summary>
        [JsonIgnore]
        [ProtoMember(13)]
        public string PropId { get; set; }

        ///<summary>
        /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
        ///</summary>
        [JsonIgnore]
        [ProtoMember(14)]
        public string PropStr { get; set; }

        ///<summary>
        /// 销售数量
        ///</summary>
        [ProtoMember(15)]
        public int SaleCount { get; set; }

        ///<summary>
        /// 商家Id
        ///</summary>
        [ProtoMember(16)]
        public int SellerId { get; set; }

        ///<summary>
        /// 状态 0为可用
        ///</summary>
        [ProtoMember(17)]
        public Prod.Status Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(18)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(19)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(20)]
        public DateTime ModifiedOn { get; set; }


        #region Readonly
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

        [JsonIgnore]
        public virtual string[] PropTexts
        {
            get
            {
                return !string.IsNullOrEmpty(PropStr) ? PropStr.Split(';').Select(o => o.Split(':')[2].Trim() + "：" + o.Split(':')[3].Trim()).ToArray() : null;
            }
        }
        #endregion
    }

}