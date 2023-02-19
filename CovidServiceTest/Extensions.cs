using CovidService.Services.County;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CovidServiceTest
{
    internal class Extensions
    {

        public static Task<T> NewTask<T>(Action<T> initializer = null) where T : new()
        {
            var temp = new T();

            if (initializer != null)
                initializer(temp);

            return Task.FromResult<T>(temp);
        }
    }
}
