using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBCA_Desktop_App {
    internal class Repo {
        public static MBCAEntities db = new MBCAEntities();
        public static User logged = null;
        public static Form1 form = null;
        public static Event selEvent = null;
        public static Exhibit selExhibit = null;
    }
}
