using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using imow.Core.cookieModel;
using imow.Framework.Cookie;

namespace imow.Services.BussinessService
{
    public class BaseTicketService<T> where T :class,ICookieModel
    {
        private readonly  CookieManager<T> _ticketCookie;

        public BaseTicketService(CookieManager<T> ticketCookie )
        {
            _ticketCookie = ticketCookie;
        }

        /// <summary>
        /// 获得Ticket
        /// </summary>
        /// <returns></returns>
        public FormsAuthenticationTicket GetTicket()
        {

            if (_ticketCookie.Exists())
            {
                try
                {
                    string value = _ticketCookie.Get().Value;
                    var oldTicket = FormsAuthentication.Decrypt(value);

                    if (oldTicket != null)
                    {
                        if (!oldTicket.Expired)
                        {
                            if (FormsAuthentication.SlidingExpiration)
                            {
                                var ticket = FormsAuthentication.RenewTicketIfOld(oldTicket);
                                if (ticket != oldTicket)
                                {
                                    this.AddCookie(ticket);
                                }
                                return ticket;
                            }
                            else
                            {
                                return oldTicket;
                            }
                        }
                        else
                        {
                            _ticketCookie.Remove();
                        }
                    }
                }
                catch (CryptographicException e)
                {
                    _ticketCookie.Remove();
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public void Add(string cookieName,string cookieData, int minutes)
        {
            var ticket = new FormsAuthenticationTicket(2
                                            , cookieName
                                            , DateTime.Now
                                            , DateTime.Now.AddMinutes(minutes)
                                            , false
                                            , cookieData);
            this.AddCookie(ticket);
        }

        private void AddCookie(FormsAuthenticationTicket ticket)
        {
            var encryptTicket = FormsAuthentication.Encrypt(ticket);
            _ticketCookie.SetCookie(true, FormsAuthentication.FormsCookiePath, encryptTicket, FormsAuthentication.RequireSSL, FormsAuthentication.CookieDomain);
        }

        

        public bool Exists()
        {
            return _ticketCookie.Exists();
        }

        public void Clear()
        {
            _ticketCookie.Remove();
        }
    }
}
