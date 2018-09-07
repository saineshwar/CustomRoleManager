using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public class PasswordConcrete : IPassword
    {
        private readonly DatabaseContext _context;
        public PasswordConcrete(DatabaseContext context)
        {
            _context = context;
        }
        public long? SavePassword(PasswordMaster passwordMaster)
        {
            try
            {
                long? result = -1;
                if (passwordMaster != null)
                {
                    _context.PasswordMaster.Add(passwordMaster);
                    _context.SaveChanges();
                    result = passwordMaster.PasswordId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetPasswordbyUserId(long userId)
        {
            try
            {
                var password = (from passwordmaster in _context.PasswordMaster
                                where passwordmaster.UserId == userId
                                select passwordmaster.Password).FirstOrDefault();

                return password;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
