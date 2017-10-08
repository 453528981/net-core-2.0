using Ayatta;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Web.Models
{
    #region 国家
    public class CountryListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<Country> Countries { get; set; }
    }

    public class CountryDetailModel : Model
    {
        public Country Country { get; set; }
    }

    #endregion

    #region 幻灯片

    public class SlideListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<Slide> Slides { get; set; }
    }
    public class SlideDetailModel : Model
    {
        public Slide Slide { get; set; }
    }

    public class SlideItemListModel : Model
    {
        public string SlideId { get; set; }
        public string Keyword { get; set; }
        public IPagedList<SlideItem> Items { get; set; }
    }

    public class SlideItemDetailModel : Model
    {
        public SlideItem SlideItem { get; set; }
    }

    #endregion

    #region 帮助
    public class HelpListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<Help> Helps { get; set; }
    }

    public class HelpDetailModel : Model
    {
        public Help Help { get; set; }
    }
    #endregion

    #region 银行
    public class BankListModel : Model
    {
        public string Keyword { get; set; }
        public IPagedList<Bank> Banks { get; set; }
    }

    public class BankDetailModel : Model
    {
        public Bank Bank { get; set; }
    }
    #endregion
    #region 支付平台
    public class PaymentPlatformListModel : Model
    {
        public string Keyword { get; set; }
        public IList<PaymentPlatform> PaymentPlatforms { get; set; }
    }

    public class PaymentPlatformDetailModel : Model
    {
        public PaymentPlatform PaymentPlatform { get; set; }
    }
    #endregion
    #region OAuthProvider
    public class OAuthProviderListModel : Model
    {
        public string Keyword { get; set; }
        public IList<OAuthProvider> OAuthProviders { get; set; }
    }

    public class OAuthProviderDetailModel : Model
    {
        public OAuthProvider OAuthProvider { get; set; }
    }
    #endregion
}