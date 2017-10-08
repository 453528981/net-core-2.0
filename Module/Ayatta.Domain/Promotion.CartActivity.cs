using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public partial class Promotion
    {
        ///<summary>
        /// 购物车促销活动
        ///</summary>
        [ProtoContract]
        public class CartActivity : IEntity<int>
        {
            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 类型 0为满元减 1为满件折
            ///</summary>
            [ProtoMember(2)]
            public byte Type { get; set; }

            ///<summary>
            /// 活动名称
            ///</summary>
            [ProtoMember(3)]
            public string Name { get; set; }

            ///<summary>
            /// 活动标题
            ///</summary>
            [ProtoMember(4)]
            public string Title { get; set; }

            ///<summary>
            /// 开始时间
            ///</summary>
            [ProtoMember(5)]
            public DateTime StartedOn { get; set; }

            ///<summary>
            /// 结束时间
            ///</summary>
            [ProtoMember(6)]
            public DateTime StoppedOn { get; set; }

            ///<summary>
            /// 促销折扣/减免金额作用于 订单总金额 商品总金额 运费 税费 商品价格
            ///</summary>
            [ProtoMember(7)]
            public DiscountOn DiscountOn { get; set; }

            ///<summary>
            /// 促销值 打x折 减x元
            ///</summary>
            [ProtoMember(8)]
            public decimal DiscountValue { get; set; }

            ///<summary>
            /// 活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app
            ///</summary>
            [ProtoMember(9)]
            public Platform Platform { get; set; }

            ///<summary>
            /// 限定最低用户级别 0为无限定
            ///</summary>
            [ProtoMember(10)]
            public byte UserGrade { get; set; }

            ///<summary>
            /// 限定用户Id 空为无限定 如需限定部分用户 使用","分隔
            ///</summary>
            [ProtoMember(11)]
            public string UserScope { get; set; }

            ///<summary>
            /// 限定商品ItemId 空为无限定 如需限定部分商品 使用","分隔
            ///</summary>
            [ProtoMember(12)]
            public string ItemScope { get; set; }

            ///<summary>
            /// 限定商品CageId 空为无限定 如需限定部分类目 使用","分隔
            ///</summary>
            [ProtoMember(13)]
            public string CatgScope { get; set; }

            ///<summary>
            /// 限定商品BrandId 空为无限定 如需限定部分品牌 使用","分隔
            ///</summary>
            [ProtoMember(14)]
            public string BrandScope { get; set; }

            ///<summary>
            /// 限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔
            ///</summary>
            [ProtoMember(15)]
            public string MediaScope { get; set; }

            ///<summary>
            /// 限定区域Id 空为无限定 如需限定部分区域 使用","分隔
            ///</summary>
            [ProtoMember(16)]
            public string RegionScope { get; set; }

            ///<summary>
            /// 用户参与活动限制类型 0无限制 1限制该活动总的参与次数 2限制该活动每个用户可参与次数
            ///</summary>
            [ProtoMember(17)]
            public LimitType LimitType { get; set; }

            ///<summary>
            /// 用户参与活动限制值 LimitType不为0时有效
            ///</summary>
            [ProtoMember(18)]
            public int LimitValue { get; set; }

            ///<summary>
            /// 卖家Id
            ///</summary>
            [ProtoMember(19)]
            public int SellerId { get; set; }

            ///<summary>
            /// 卖家名称
            ///</summary>
            [ProtoMember(20)]
            public string SellerName { get; set; }

            ///<summary>
            /// 状态 1为可用 0为不可用
            ///</summary>
            [ProtoMember(21)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///</summary>
            [ProtoMember(22)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///</summary>
            [ProtoMember(23)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///</summary>
            [ProtoMember(24)]
            public DateTime ModifiedOn { get; set; }



            /// <summary>
            /// 促销生效必要条件
            /// </summary>
            [ProtoMember(25)]
            public virtual IList<Rule> Rules { get; set; }

            #region Readonly

            /// <summary>
            /// 限定用户
            /// </summary>
            public virtual IList<int> Users
            {
                get
                {
                    if (!string.IsNullOrEmpty(UserScope))
                    {
                        UserScope.Split(',').Select(x => int.Parse(x));                        
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 限定商品
            /// </summary>
            public virtual IList<int> Items
            {
                get
                {
                    if (!string.IsNullOrEmpty(ItemScope))
                    {
                        ItemScope.Split(',').Select(x => int.Parse(x));
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 限定商品类目
            /// </summary>
            public virtual IList<int> Catgs
            {
                get
                {
                    if (!string.IsNullOrEmpty(CatgScope))
                    {
                        CatgScope.Split(',').Select(x => int.Parse(x));
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 限定商品品牌
            /// </summary>
            public virtual IList<int> Brands
            {
                get
                {
                    if (!string.IsNullOrEmpty(BrandScope))
                    {
                        BrandScope.Split(',').Select(x => int.Parse(x));
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 限定媒体
            /// </summary>
            public virtual IList<int> Medias
            {
                get
                {
                    if (!string.IsNullOrEmpty(MediaScope))
                    {
                        MediaScope.Split(',').Select(x => int.Parse(x));

                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 限定区域
            /// </summary>
            public virtual IList<string> Regions
            {
                get
                {
                    if (!string.IsNullOrEmpty(RegionScope))
                    {
                        return RegionScope.Split(',').ToList();
                    }
                    return new List<string>(0);
                }
            }

            #endregion

            /// <summary>
            /// 判断活动在指定平台是否有效            
            /// </summary>
            /// <param name="platform">适用平台</param>
            /// <returns></returns>
            public bool IsValid(Platform platform)
            {
                var now = DateTime.Now;
                var available = ((Platform & platform) == platform);//检查当前促销是否适用于给定平台           
                return Status && StartedOn < now && now < StoppedOn && available && Rules.Any(x => x.Status);
            }

            public bool Match(UserGrade userGrade, int userId, int mediaId = 0, string regionId = "")
            {
                var grade = (byte)userGrade;
                if (UserGrade > 0 && UserGrade < grade)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(UserScope) && !Users.Contains(userId))
                {
                    return false;
                }

                if (mediaId > 0 && !string.IsNullOrEmpty(MediaScope) && !Medias.Contains(mediaId))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(regionId) && !string.IsNullOrEmpty(RegionScope) && !Regions.Contains(regionId))
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 折扣/减免金额作用于 商品价格 时
            /// 验证商品 品牌 类目 是否在促销设置的范围内
            /// <param name="skuId">skuId</param>
            /// <param name="itemId">itemId</param>
            /// <param name="catgId">catgId</param>
            /// <param name="brandId">brandId</param>
            /// <returns></returns>
            public bool Match(int skuId, int itemId, int catgId, int brandId)
            {
                if (DiscountOn == DiscountOn.Price)
                {
                    if (!string.IsNullOrEmpty(ItemScope) && !Items.Contains(itemId))
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(CatgScope) && !Catgs.Contains(catgId))
                    {
                        return false;
                    }
                    if (!string.IsNullOrEmpty(BrandScope) && !Brands.Contains(brandId))
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }

            ///<summary>
            /// 购物车促销活动规则
            ///</summary>
            [ProtoContract]
            public class Rule : IEntity<int>
            {

                ///<summary>
                /// Id
                ///</summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 购物车促销Id
                ///</summary>
                [ProtoMember(2)]
                public int ParentId { get; set; }

                ///<summary>
                /// 开始时间
                ///</summary>
                [ProtoMember(3)]
                public DateTime StartedOn { get; set; }

                ///<summary>
                /// 结束时间
                ///</summary>
                [ProtoMember(4)]
                public DateTime StoppedOn { get; set; }

                ///<summary>
                /// 计算方式
                ///</summary>
                [ProtoMember(5)]
                public CalcType CalcType { get; set; }

                ///<summary>
                /// 计算使用到的参数值
                ///</summary>
                [ProtoMember(6)]
                public string CalcValue { get; set; }

                ///<summary>
                /// 优先顺序 从小到大
                ///</summary>
                [ProtoMember(7)]
                public int Priority { get; set; }

                ///<summary>
                /// 卖家Id
                ///</summary>
                [ProtoMember(8)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用 0为不可用
                ///</summary>
                [ProtoMember(9)]
                public bool Status { get; set; }

                ///<summary>
                /// 创建时间
                ///</summary>
                [ProtoMember(10)]
                public DateTime CreatedOn { get; set; }

                ///<summary>
                /// 最后一次编辑者
                ///</summary>
                [ProtoMember(11)]
                public string ModifiedBy { get; set; }

                ///<summary>
                /// 最后一次编辑时间
                ///</summary>
                [ProtoMember(12)]
                public DateTime ModifiedOn { get; set; }

                /// <summary>
                /// 通过Cale与Value生成的说明
                /// </summary>
                public virtual string Description
                {
                    get
                    {
                        var s = string.Empty;
                        if (CalcType == CalcType.A)
                        {
                            var v = AsValueA();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"{0} -- {1} 期间有效订单总数 {2} {3}，且有效订单总金额 {4} {5}。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"),
                                        v.CountOpt, v.CountParam, v.AmountOpt, v.AmountParam);
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"{0} -- {1} 期间有效订单总数 {2} {3}，或者有效订单总金额 {4} {5}。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"),
                                        v.CountOpt, v.CountParam, v.AmountOpt, v.AmountParam);
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (CalcType == CalcType.B)
                        {
                            var v = AsValueB();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"{0} -- {1} 期间已购买过 {2} 商品";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), string.Join(",", v.Param));
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"{0} -- {1} 期间已购买过 {2} 商品中的任何一个。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), string.Join(",", v.Param));
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (CalcType == CalcType.C)
                        {
                            var v = AsValueB();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"购物车中包含 {0} 商品";
                                    s = string.Format(tpl, string.Join(",", v.Param));
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"购物车中包含 {0} 商品中的任何一个。";
                                    s = string.Format(tpl, string.Join(",", v.Param));
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (CalcType == CalcType.D)
                        {
                            var v = AsValueC();
                            if (v.IsValid)
                            {
                                var tpl = @"{0} -- {1} 期间有效评论数 {2} {3}。";
                                s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), v.Opt, v.Param);
                                return s;
                            }
                            return "参数有误。";
                        }
                        if (CalcType == CalcType.E)
                        {
                            var v = AsValueC();
                            if (v.IsValid)
                            {
                                var tpl = @"{0} -- {1} 期间有效邀请人数 {2} {3}。";
                                s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), v.Opt, v.Param);
                                return s;
                            }
                            return "参数有误。";
                        }
                        return s;
                    }
                }

                /// <summary>
                /// Calc.A
                /// </summary>
                /// <returns></returns>
                public ValueA AsValueA()
                {
                    // value值为 or:count>123,amount<789 或 and:count>123,amount<789
                    if (CalcType != CalcType.A) return new ValueA(false);
                    if (string.IsNullOrEmpty(CalcValue)) return new ValueA(false);
                    var value = CalcValue.ToLower();
                    if (value.StartsWith("and") || value.StartsWith("or"))
                    {
                        try
                        {
                            var o = new ValueA(true);
                            var a = value.Split(':')[0];
                            if (a == "and")
                            {
                                o.And = true;
                            }
                            var tmp = value.Split(':')[1];
                            var vals = tmp.Split(',');
                            var status = false;
                            foreach (var val in vals)
                            {
                                if (val.StartsWith("count"))
                                {
                                    status = true;
                                    o.CountOpt = val.Substring(5, 1)[0];
                                    o.CountParam = Convert.ToInt32(val.Substring(6));
                                }
                                else if (val.StartsWith("amount"))
                                {
                                    status = true;
                                    o.AmountOpt = val.Substring(6, 1)[0];
                                    o.AmountParam = Convert.ToInt32(val.Substring(7));
                                }
                            }
                            return status ? o : new ValueA(false);
                        }
                        catch (Exception)
                        {
                            return new ValueA(false);
                        }
                    }
                    return new ValueA(false);
                }

                /// <summary>
                /// Calc.B Calc.C
                /// </summary>
                /// <returns></returns>
                public ValueB AsValueB()
                {
                    // value值为 or:123,789 或 and:123,789
                    if (CalcType != CalcType.B && CalcType != CalcType.B) return new ValueB(false);
                    if (string.IsNullOrEmpty(CalcValue)) return new ValueB(false);
                    var value = CalcValue.ToLower();
                    var tmp = value.Split(':');
                    var op = tmp[0];
                    if (op == "and" || op == "or")
                    {
                        if (!string.IsNullOrEmpty(tmp[0]))
                        {
                            try
                            {
                                var o = new ValueB(true);
                                o.And = op == "and";
                                var v = tmp[1].Split(',');
                                o.Param = v.Select(x => int.Parse(x)).ToArray();
                                
                                return o;
                            }
                            catch (Exception)
                            {
                                return new ValueB(false);
                            }
                        }
                    }
                    return new ValueB(false);
                }

                /// <summary>
                /// 适用于Calc.D Calc.E
                /// </summary>
                /// <returns></returns>
                public ValueC AsValueC()
                {
                    // value值为 >123 或 =123 或 >123
                    if (CalcType != CalcType.D && CalcType != CalcType.E) return new ValueC(false);
                    if (string.IsNullOrEmpty(CalcValue)) return new ValueC(false);
                    if (CalcValue[0] != '<' && CalcValue[0] != '=' && CalcValue[0] != '>') return new ValueC(false);
                    try
                    {
                        return new ValueC(true) { Opt = CalcValue[0], Param = Convert.ToInt32(CalcValue.Substring(1)) };
                    }
                    catch (Exception)
                    {
                        return new ValueC(false);
                    }
                }

                /// <summary>
                /// 适用于Calc.A
                /// </summary>
                public class ValueA
                {
                    /// <summary>
                    /// 是否订单数/金额都必须满足
                    /// </summary>
                    public bool And { get; set; }

                    /// <summary>
                    /// 订单数比较符
                    /// </summary>
                    public char CountOpt { get; set; }

                    /// <summary>
                    /// 订单数
                    /// </summary>
                    public int CountParam { get; set; }

                    /// <summary>
                    /// 订单金额比较符
                    /// </summary>
                    public char AmountOpt { get; set; }

                    /// <summary>
                    /// 订单金额
                    /// </summary>
                    public int AmountParam { get; set; }

                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="isValid"></param>
                    public ValueA(bool isValid)
                    {
                        CountOpt = '0';
                        AmountOpt = '0';
                        IsValid = isValid;
                    }
                }

                /// <summary>
                /// 适用于Calc.B Calc.C
                /// </summary>
                public class ValueB
                {
                    /// <summary>
                    /// 是否商品都必须满足
                    /// </summary>
                    public bool And { get; set; }

                    /// <summary>
                    /// 商品Id
                    /// </summary>
                    public int[] Param { get; set; }

                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="isValid"></param>
                    public ValueB(bool isValid)
                    {
                        IsValid = isValid;
                    }
                }

                /// <summary>
                /// 适用于Calc.D Calc.E
                /// </summary>
                public class ValueC
                {
                    /// <summary>
                    /// 比较符
                    /// </summary>
                    public char Opt { get; set; }

                    /// <summary>
                    /// 比较值
                    /// </summary>
                    public int Param { get; set; }

                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }


                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="isValid"></param>
                    public ValueC(bool isValid)
                    {
                        Opt = '0';
                        IsValid = isValid;
                    }
                }
            }
        }
    }
}

