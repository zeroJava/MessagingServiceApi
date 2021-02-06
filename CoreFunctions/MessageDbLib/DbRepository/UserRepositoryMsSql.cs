using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityClasses;
using MessageDbCore.Repositories;
using MessageDbLib.Constants.TableConstants;
using MessageDbLib.DbEngine;
using MessageDbLib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MessageDbLib.DbRepository.ADO
{
	public class UserRepositoryMsSql : IUserRepository
	{
		protected readonly string connectionString;
		protected readonly IRepoTransaction repoTransaction;
		protected readonly bool transactionModeEnabled = false;
		public virtual string TableName { get; protected set; } = "dbo.UserTable";

		public UserRepositoryMsSql(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public UserRepositoryMsSql(string connectionString, IRepoTransaction repoTransaction)
		{
			this.connectionString = connectionString;
			this.repoTransaction = repoTransaction;
			this.transactionModeEnabled = true;
		}

		public List<User> GetAllUsers()
		{
			try
			{
				string columns = GetSelectColumns();
				string query = string.Format("SELECT {0} FROM {1}", columns, TableName);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query, null, connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.GetAllUsers", exception);
			}
		}

		public User GetUserMatchingId(long id)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = GetUserMatchingIdQuery(id);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.GetUserMatchingId", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUserMatchingIdQuery(long id)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.ID, id)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns, TableName,
				UserParameter.ID);
			return Tuple.Create(query, sqlParameters);
		}

		public User GetUserMatchingUsername(string username)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = GetUserMatchingUsernameQuery(username);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.GetUserMatchingUsername", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUserMatchingUsernameQuery(string username)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.USERNAME, username)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE USERNAME = {2}", columns, TableName,
				UserParameter.USERNAME);
			return Tuple.Create(query, sqlParameters);
		}

		public User GetUserMatchingUsernameAndPassword(string username, string password)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = GetUsersMatchUsernameAndPasswordQuery(username, password);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.GetUserMatchingUsernameAndPassword", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUsersMatchUsernameAndPasswordQuery(string username, string password)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.USERNAME, username),
				new SqlParameter(UserParameter.PASSWORD, password)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE USERNAME = {2} AND PASSWORD = {3}", columns,
				TableName,
				UserParameter.USERNAME,
				UserParameter.PASSWORD);
			return Tuple.Create(query, sqlParameters);
		}

		private User OnPopulateResultListCallBack(DbDataReader dataReader)
		{
			bool isAdvancedUser = false;
			if (dataReader[UserColumn.IS_ADVANCED_USER] != null &&
				bool.TryParse(dataReader[UserColumn.IS_ADVANCED_USER].ToString(), out bool result))
			{
				isAdvancedUser = result;
			}
			if (isAdvancedUser)
			{
				AdvancedUser advancedUser = new AdvancedUser();
				PopulateStandardUser(advancedUser, dataReader);
				PopulateAdvancedUser(advancedUser, dataReader);
				return advancedUser;
			}
			else
			{
				User user = new User();
				PopulateStandardUser(user, dataReader);
				return user;
			}
		}

		private void PopulateStandardUser(User user, DbDataReader dataReader)
		{
			if (dataReader[UserColumn.ID] != null &&
				long.TryParse(dataReader[UserColumn.ID].ToString(), out long id))
			{
				user.Id = id;
			}
			if (dataReader[UserColumn.USERNAME] != null)
			{
				user.UserName = dataReader[UserColumn.USERNAME].ToString();
			}
			if (dataReader[UserColumn.PASSWORD] != null)
			{
				user.Password = dataReader[UserColumn.PASSWORD].ToString();
			}
			if (dataReader[UserColumn.EMAIL_ADDRESS] != null)
			{
				user.EmailAddress = dataReader[UserColumn.EMAIL_ADDRESS].ToString();
			}
			if (dataReader[UserColumn.FIRSTNAME] != null)
			{
				user.FirstName = dataReader[UserColumn.FIRSTNAME].ToString();
			}
			if (dataReader[UserColumn.SURNAME] != null)
			{
				user.Surname = dataReader[UserColumn.SURNAME].ToString();
			}
			if (dataReader[UserColumn.DOB] != null &&
				DateTime.TryParse(dataReader[UserColumn.DOB].ToString(), out DateTime dob))
			{
				user.DOB = dob;
			}
			if (dataReader[UserColumn.GENDER] != null)
			{
				user.Gender = dataReader[UserColumn.GENDER].ToString();
			}
		}

		private void PopulateAdvancedUser(AdvancedUser advancedUser, DbDataReader dataReader)
		{
			if (dataReader[UserColumn.ADVANCED_USER_START_DATE] != null &&
				DateTime.TryParse(dataReader[UserColumn.ADVANCED_USER_START_DATE].ToString(), out DateTime startDateTime))
			{
				advancedUser.AdvanceStartDatetime = startDateTime;
			}
			if (dataReader[UserColumn.ADVANCED_USER_END_DATE] != null &&
				DateTime.TryParse(dataReader[UserColumn.ADVANCED_USER_END_DATE].ToString(), out DateTime endDateTime))
			{
				advancedUser.AdvanceEndDatetime = endDateTime;
			}
		}

		public void InsertUser(User user)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = user is AdvancedUser ? GetAdvancedUserInsertQuery(user as AdvancedUser) :
					GetUserInsertQuery(user);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					mssqlDbEngine.ExecuteQueryInsertCallback(user, OnPopulateIdCallBack);
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.InsertUser", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUserInsertQuery(User user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.USERNAME, GetValidValue(user.UserName)),
				new SqlParameter(UserParameter.PASSWORD, GetValidValue(user.Password)),
				new SqlParameter(UserParameter.EMAIL_ADDRESS, GetValidValue(user.EmailAddress)),
				new SqlParameter(UserParameter.FIRSTNAME, GetValidValue(user.FirstName)),
				new SqlParameter(UserParameter.SECONDNAME, GetValidValue(user.Surname)),
				new SqlParameter(UserParameter.DOB, GetValidValue(user.DOB)),
				new SqlParameter(UserParameter.GENDER, GetValidValue(user.Gender)),
				new SqlParameter(UserParameter.IS_ADVANCED_USER, GetValidValue(false))
			};

			string insertColumns = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", TableName,
				UserColumn.USERNAME,
				UserColumn.PASSWORD,
				UserColumn.EMAIL_ADDRESS,
				UserColumn.FIRSTNAME,
				UserColumn.SURNAME,
				UserColumn.DOB,
				UserColumn.GENDER,
				UserColumn.IS_ADVANCED_USER);

			string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
				UserParameter.USERNAME,
				UserParameter.PASSWORD,
				UserParameter.EMAIL_ADDRESS,
				UserParameter.FIRSTNAME,
				UserParameter.SECONDNAME,
				UserParameter.DOB,
				UserParameter.GENDER,
				UserParameter.IS_ADVANCED_USER);

			string query = string.Format("{0} {1}", insertColumns, valueSection);
			return Tuple.Create(query, sqlParameters);
		}

		private Tuple<string, SqlParameter[]> GetAdvancedUserInsertQuery(AdvancedUser user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.USERNAME, GetValidValue(user.UserName)),
				new SqlParameter(UserParameter.PASSWORD, GetValidValue(user.Password)),
				new SqlParameter(UserParameter.EMAIL_ADDRESS, GetValidValue(user.EmailAddress)),
				new SqlParameter(UserParameter.FIRSTNAME, GetValidValue(user.FirstName)),
				new SqlParameter(UserParameter.SECONDNAME, GetValidValue(user.Surname)),
				new SqlParameter(UserParameter.DOB, GetValidValue(user.DOB)),
				new SqlParameter(UserParameter.GENDER, GetValidValue(user.Gender)),
				new SqlParameter(UserParameter.IS_ADVANCED_USER, GetValidValue(true)),
				new SqlParameter(UserParameter.ADVANCED_USER_START_DATE, GetValidValue(user.AdvanceStartDatetime)),
				new SqlParameter(UserParameter.ADVANCED_USER_END_DATE, GetValidValue(user.AdvanceEndDatetime))
			};

			string insertColumns = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
				TableName,
				UserColumn.USERNAME,
				UserColumn.PASSWORD,
				UserColumn.EMAIL_ADDRESS,
				UserColumn.FIRSTNAME,
				UserColumn.SURNAME,
				UserColumn.DOB,
				UserColumn.GENDER,
				UserColumn.IS_ADVANCED_USER,
				UserColumn.ADVANCED_USER_START_DATE,
				UserColumn.ADVANCED_USER_END_DATE);

			string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
				UserParameter.USERNAME,
				UserParameter.PASSWORD,
				UserParameter.EMAIL_ADDRESS,
				UserParameter.FIRSTNAME,
				UserParameter.SECONDNAME,
				UserParameter.DOB,
				UserParameter.GENDER,
				UserParameter.IS_ADVANCED_USER,
				UserParameter.ADVANCED_USER_START_DATE,
				UserParameter.ADVANCED_USER_END_DATE);

			string query = string.Format("{0} {1}", insertColumns, valueSection);
			return Tuple.Create(query, sqlParameters);
		}

		private void OnPopulateIdCallBack(User user, object result)
		{
			long id = Convert.ToInt64(result);
			user.Id = id;
		}

		public void UpdateUser(User user)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = user is AdvancedUser ? GetUpdateAdvancedUserQuery(user as AdvancedUser) :
					GetUpdateUserQuery(user);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.UpdateUser", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUpdateUserQuery(User user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.ID, GetValidValue(user.Id)),
				new SqlParameter(UserParameter.PASSWORD, GetValidValue(user.Password)),
				new SqlParameter(UserParameter.FIRSTNAME, GetValidValue(user.FirstName)),
				new SqlParameter(UserParameter.SECONDNAME, GetValidValue(user.Surname)),
				new SqlParameter(UserParameter.DOB, GetValidValue(user.DOB)),
				new SqlParameter(UserParameter.GENDER, GetValidValue(user.Gender)),
				new SqlParameter(UserParameter.EMAIL_ADDRESS, GetValidValue(user.EmailAddress))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setPassword = string.Format("{0} = {1}", UserColumn.PASSWORD, UserParameter.PASSWORD);
			string setFirstName = string.Format("{0} = {1}", UserColumn.FIRSTNAME, UserParameter.FIRSTNAME);
			string setSecondName = string.Format("{0} = {1}", UserColumn.SURNAME, UserParameter.SECONDNAME);
			string setDob = string.Format("{0} = {1}", UserColumn.DOB, UserParameter.DOB);
			string setGender = string.Format("{0} = {1}", UserColumn.GENDER, UserParameter.GENDER);
			string setEmailAddress = string.Format("{0} = {1}", UserColumn.EMAIL_ADDRESS, UserParameter.EMAIL_ADDRESS);

			string whereId = string.Format("WHERE {0} = {1}", UserColumn.ID, UserParameter.ID);

			string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6} {7}", updateTable,
				setPassword,
				setFirstName,
				setSecondName,
				setDob,
				setGender,
				setEmailAddress,
				whereId);
			return Tuple.Create(query, sqlParameters);
		}

		private Tuple<string, SqlParameter[]> GetUpdateAdvancedUserQuery(AdvancedUser advancedUser)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(UserParameter.ID, GetValidValue(advancedUser.Id)),
				new SqlParameter(UserParameter.PASSWORD, GetValidValue(advancedUser.Password)),
				new SqlParameter(UserParameter.FIRSTNAME, GetValidValue(advancedUser.FirstName)),
				new SqlParameter(UserParameter.SECONDNAME, GetValidValue(advancedUser.Surname)),
				new SqlParameter(UserParameter.DOB, GetValidValue(advancedUser.DOB)),
				new SqlParameter(UserParameter.GENDER, GetValidValue(advancedUser.Gender)),
				new SqlParameter(UserParameter.EMAIL_ADDRESS, GetValidValue(advancedUser.EmailAddress)),
				new SqlParameter(UserParameter.ADVANCED_USER_START_DATE, GetValidValue(advancedUser.AdvanceStartDatetime)),
				new SqlParameter(UserParameter.ADVANCED_USER_END_DATE, GetValidValue(advancedUser.AdvanceEndDatetime))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setPassword = string.Format("{0} = {1}", UserColumn.PASSWORD, UserParameter.PASSWORD);
			string setFirstName = string.Format("{0} = {1}", UserColumn.FIRSTNAME, UserParameter.FIRSTNAME);
			string setSecondName = string.Format("{0} = {1}", UserColumn.SURNAME, UserParameter.SECONDNAME);
			string setDob = string.Format("{0} = {1}", UserColumn.DOB, UserParameter.DOB);
			string setGender = string.Format("{0} = {1}", UserColumn.GENDER, UserParameter.GENDER);
			string setEmailAddress = string.Format("{0} = {1}", UserColumn.EMAIL_ADDRESS, UserParameter.EMAIL_ADDRESS);
			string setAdvanStartDate = string.Format("{0} = {1}", UserColumn.ADVANCED_USER_START_DATE, UserParameter.ADVANCED_USER_START_DATE);
			string setAdvanceEndDate = string.Format("{0} = {1}", UserColumn.ADVANCED_USER_END_DATE, UserParameter.ADVANCED_USER_END_DATE);

			string whereId = string.Format("WHERE {0} = {1}", UserColumn.ID, UserParameter.ID);

			string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} {9}", updateTable,
				setPassword,
				setFirstName,
				setSecondName,
				setDob,
				setGender,
				setEmailAddress,
				setAdvanStartDate,
				setAdvanceEndDate,
				whereId);
			return Tuple.Create(query, sqlParameters);
		}

		public void DeleteUser(User user)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter(UserParameter.ID, GetValidValue(user.Id))
				};

				string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
					UserColumn.ID,
					UserParameter.ID);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepositoryMsSql.DeleteUser", exception);
			}
		}

		public void Dispose()
		{
			//
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
				UserColumn.ID,
				UserColumn.USERNAME,
				UserColumn.PASSWORD,
				UserColumn.EMAIL_ADDRESS,
				UserColumn.FIRSTNAME,
				UserColumn.SURNAME,
				UserColumn.DOB,
				UserColumn.GENDER,
				UserColumn.IS_ADVANCED_USER,
				UserColumn.ADVANCED_USER_START_DATE,
				UserColumn.ADVANCED_USER_END_DATE);
			return columns;
		}

		protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
			string connectionString)
		{
			if (transactionModeEnabled &&
				repoTransaction != null)
			{
				MssqlDbEngine transactionMssqlEngine = new MssqlDbEngine(query, mssqlParameters,
					connectionString,
					repoTransaction);
				return transactionMssqlEngine;
			}
			MssqlDbEngine mssqlDbEngine = new MssqlDbEngine(query, mssqlParameters,
				connectionString);
			return mssqlDbEngine;
		}

		protected object GetValidValue(object value)
		{
			return DbValueUtil.GetValidValue(value);
		}
	}
}