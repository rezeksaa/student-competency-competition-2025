using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsemkaSchool {
    internal class Repo {
        public static EsemkaSchoolEntities db = new EsemkaSchoolEntities();
        public static User logged = null;
        public static Form1 loginForm = null;
    }
}
