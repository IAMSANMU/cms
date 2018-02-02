using System.Web.Routing;

namespace imow.AdminEtl.Models
{
    public class Paging
    {
        public int CurrentPage;

        public int PageSize;

        public int TotalItemCount;

        public RouteData RouteData;
        public RouteValueDictionary RouteValueDictionary;

        public Paging(int CurrentPage, int PageSize, int TotalItemCount, RouteData RouteData, RouteValueDictionary RouteValueDictionary)
        {
            this.CurrentPage = CurrentPage;
            this.PageSize = PageSize;
            this.TotalItemCount = TotalItemCount;
            this.RouteData = RouteData;
            this.RouteValueDictionary = RouteValueDictionary;
        }
        public Paging()
        {

        }
    }
}