using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assignment.Models;

namespace Assignment.Services;





public interface IUserService
{
    IReadOnlyList<AppUser> GetAll();
    AppUser? GetById(string id);
    AppUser? GetByEmail(string email);
    bool ExistsByEmail(string email);

    void Add(AppUser user);
    void Update(AppUser user);
    void Delete(string id);
}



