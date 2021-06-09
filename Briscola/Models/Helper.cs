using System.Collections.Generic;

namespace Briscola.Models
{
    public static class Helper
    {
        public static void RemoveFirst<T>(this List<T> lista) => lista.RemoveAt(0);
    }
}
