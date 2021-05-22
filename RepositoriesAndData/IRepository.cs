using System;
using System.Collections.Generic;


namespace RepositoriesAndData
{
    public interface IRepository
    {
        List<object> GetPage(int page);
    }
}