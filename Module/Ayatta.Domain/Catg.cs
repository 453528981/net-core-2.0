using System;
using ProtoBuf;
using System.Collections.Generic;


namespace Ayatta.Domain
{

    [ProtoContract]
    public class Catg
    {
        #region
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int ParentId { get; set; }

        [ProtoMember(4)]
        public bool IsParent { get; set; }

        [ProtoMember(5)]
        public int Priority { get; set; }

        [ProtoMember(6)]
        public byte Status { get; set; }

        [ProtoMember(7)]
        public DateTime CreatedOn { get; set; }

        [ProtoMember(8)]
        public string ModifiedBy { get; set; }

        [ProtoMember(9)]
        public DateTime ModifiedOn { get; set; }
        #endregion

        [ProtoMember(10)]
        public virtual IList<Prop> Props { get; set; }

        public override bool Equals(object o)
        {
            var x = o as Catg;
            return x?.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #region Prop
        [ProtoContract]
        public class Prop
        {
            [ProtoMember(1)]
            public int Id { get; set; }

            [ProtoMember(2)]
            public int CatgId { get; set; }

            [ProtoMember(3)]
            public int ParentPid { get; set; }

            [ProtoMember(4)]
            public int ParentVid { get; set; }

            [ProtoMember(5)]
            public string Name { get; set; }

            [ProtoMember(6)]
            public bool Must { get; set; }
            [ProtoMember(7)]
            public bool Multi { get; set; }

            [ProtoMember(8)]
            public bool IsKeyProp { get; set; }

            [ProtoMember(9)]
            public bool IsSaleProp { get; set; }

            [ProtoMember(10)]
            public bool IsEnumProp { get; set; }

            [ProtoMember(11)]
            public bool IsItemProp { get; set; }

            [ProtoMember(12)]
            public bool IsColorProp { get; set; }

            [ProtoMember(13)]
            public bool IsInputProp { get; set; }

            [ProtoMember(14)]
            public bool IsSearchProp { get; set; }

            [ProtoMember(15)]
            public bool AllowAlias { get; set; }

            [ProtoMember(16)]
            public string ChildName { get; set; }

            [ProtoMember(17)]
            public string FeatureStr { get; set; }

            [ProtoMember(18)]
            public int Priority { get; set; }

            [ProtoMember(19)]
            public byte Status { get; set; }

            [ProtoMember(20)]
            public DateTime CreatedOn { get; set; }

            [ProtoMember(21)]
            public string ModifiedBy { get; set; }

            [ProtoMember(22)]
            public DateTime ModifiedOn { get; set; }

            [ProtoMember(23)]
            public virtual IList<Value> Values { get; set; }


            public override bool Equals(object o)
            {
                var x = o as Prop;
                if (x != null)
                {
                    return (x.Id == Id && x.CatgId == CatgId);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id + CatgId;
            }

            [ProtoContract]
            public class Value
            {
                [ProtoMember(1)]
                public int Id { get; set; }

                [ProtoMember(2)]
                public int CatgId { get; set; }

                [ProtoMember(3)]
                public int PropId { get; set; }

                [ProtoMember(4)]
                public string PropName { get; set; }

                [ProtoMember(5)]
                public string Name { get; set; }

                [ProtoMember(6)]
                public string Alias { get; set; }

                [ProtoMember(7)]
                public string FeatureStr { get; set; }

                [ProtoMember(8)]
                public int Priority { get; set; }

                [ProtoMember(9)]
                public byte Status { get; set; }

                [ProtoMember(10)]
                public DateTime CreatedOn { get; set; }

                [ProtoMember(11)]
                public string ModifiedBy { get; set; }

                [ProtoMember(12)]
                public DateTime ModifiedOn { get; set; }

                public override bool Equals(object o)
                {
                    var x = o as Value;
                    if (x != null)
                    {
                        return (x.Id == Id && x.CatgId == CatgId && x.PropId == PropId);
                    }
                    return false;
                }

                public override int GetHashCode()
                {
                    return Id + CatgId + PropId;
                }
            }
        }

        #endregion

        #region Mini
        [ProtoContract]
        public class Mini
        {
            [ProtoMember(1)]
            public int Id { get; set; }

            [ProtoMember(2)]
            public int ParentId { get; set; }

            [ProtoMember(3)]
            public bool IsParent { get; set; }


            [ProtoMember(4)]
            public string Name { get; set; }


            [ProtoMember(5)]
            public int Priority { get; set; }


            [ProtoMember(6)]
            public IList<Prop> Props { get; set; }


            public override bool Equals(object o)
            {
                var x = o as Catg;
                if (x != null)
                {
                    return (x.Id == Id);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id;
            }


            [ProtoContract]
            public class Prop
            {
                [ProtoMember(1)]
                public int Id { get; set; }

                [ProtoMember(2)]
                public int CatgId { get; set; }

                [ProtoMember(3)]
                public int ParentPid { get; set; }

                [ProtoMember(4)]
                public int ParentVid { get; set; }

                [ProtoMember(5)]
                public string Name { get; set; }

                [ProtoMember(6)]
                public bool Must { get; set; }

                [ProtoMember(7)]
                public bool Multi { get; set; }

                [ProtoMember(8)]
                public bool IsKeyProp { get; set; }

                [ProtoMember(9)]
                public bool IsSaleProp { get; set; }

                [ProtoMember(10)]
                public bool IsEnumProp { get; set; }

                [ProtoMember(11)]
                public bool IsItemProp { get; set; }

                [ProtoMember(12)]
                public bool IsColorProp { get; set; }

                [ProtoMember(13)]
                public bool IsInputProp { get; set; }

                [ProtoMember(14)]
                public bool IsSearchProp { get; set; }

                [ProtoMember(15)]
                public string Priority { get; set; }

                [ProtoMember(16)]
                public IList<Prop.Value> Values { get; set; }

                public override bool Equals(object o)
                {
                    var x = o as Prop;
                    if (x != null)
                    {
                        return (x.Id == Id && x.CatgId == CatgId);
                    }
                    return false;
                }

                public override int GetHashCode()
                {
                    return Id + CatgId;
                }

                [ProtoContract]
                public class Value
                {
                    [ProtoMember(1)]
                    public int Id { get; set; }

                    [ProtoMember(2)]
                    public int CatgId { get; set; }

                    [ProtoMember(3)]
                    public int PropId { get; set; }

                    [ProtoMember(4)]
                    public string Name { get; set; }

                    [ProtoMember(5)]
                    public string Priority { get; set; }

                    public override bool Equals(object o)
                    {
                        var x = o as Value;
                        if (x != null)
                        {
                            return (x.Id == Id && x.CatgId == CatgId && x.PropId == PropId);
                        }
                        return false;
                    }

                    public override int GetHashCode()
                    {
                        return Id + CatgId + PropId;
                    }

                }
            }

        }
        #endregion

        public struct Tiny
        {
            public int Id;
            public string Name;
            public int ParentId;
        }
    }
}