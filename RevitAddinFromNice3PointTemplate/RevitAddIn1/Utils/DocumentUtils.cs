using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class DocumentUtils
    {
        public static Document Document;
        public static Element ToElement(this Reference rf) => Document.GetElement(rf);
        public static Element ToElement(this ElementId id) => Document.GetElement(id);
    }
}
