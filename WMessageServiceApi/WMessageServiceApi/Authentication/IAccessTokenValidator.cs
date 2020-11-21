﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Authentication
{
	public interface IAccessTokenValidator
	{
		TokenValidationResult IsTokenValid(string encryptedToken);
		TokenValidationResult IsUserCredentialValid(string encryptedUserCred);
	}
}