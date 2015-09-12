using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSGVoxPuz.PuzGlobal {
    public static class Versioning {

        private const string VERSION_LONG = "1.0.0";

        public static string GetBuildLongVersion() {
            return VERSION_LONG;
        }
    }
}
