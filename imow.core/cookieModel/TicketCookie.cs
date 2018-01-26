namespace imow.Core.cookieModel
{
    public class TicketCookie: ICookieModel
    {
        public string GetCookieKey()
        {
            return "ec.random";
        }
    }
}