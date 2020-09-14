using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.Constants.TableConstants
{
    public sealed class AuthorisationParameter
    {
        public const string ID = "@authorisationId";
        public const string AUTHORISATION_CODE = "@authorisationCode";
        public const string START_TIME = "@starttime";
        public const string END_TIME = "@endtime";
        public const string USER_ID = "@userId";
    }
}