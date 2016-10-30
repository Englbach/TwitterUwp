using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TwitterUwp.Model
{
    public class NavMenuItem
    {
        public string name { get; set; }
        public Symbol symbol { get; set; }
        public char symbolAsChar => (char)symbol;
        public Type destPage { get; set; }
        public object arguments { get; set; }
    }
}
