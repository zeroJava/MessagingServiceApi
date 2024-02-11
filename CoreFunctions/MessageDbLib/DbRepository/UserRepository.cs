using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Column = MessageDbLib.Constants.TableConstants.UserColumn;
using Prmetr = MessageDbLib.Constants.TableConstants.UserParameter;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		//protected readonly string connectionString;
		//protected readonly IRepoTransaction repoTransaction;
		//protected readonly bool transactionModeEnabled = false;
		public virtual string TableName { get; protected set; } = "dbo.UserTable";

		public UserRepository(string connectionString) :
			base(connectionString)
		{
			this.connectionString = connectionString;
		}

		public UserRepository(string connectionString, IRepoTransaction repoTransaction) :
			base(connectionString, repoTransaction)
		{
			//this.connectionString = connectionString;
			//this.repoTransaction = repoTransaction;
			//this.transactionModeEnabled = true;
		}

		public List<User> GetAllUsers()
		{
			try
			{
				QueryBody queryBody = GetAllUsersQuery();
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, null,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.GetAllUsers", exception);
			}
		}

		private QueryBody GetAllUsersQuery()
		{
			SqlParameter[] sqlParameters = new SqlParameter[0];

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1}", columns, TableName);

			return new QueryBody(query, sqlParameters);
		}

		public User GetUserMatchingId(long id)
		{
			try
			{
				QueryBody queryBody = GetUserMatchingIdQuery(id);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.GetUserMatchingId", exception);
			}
		}

		private QueryBody GetUserMatchingIdQuery(long id)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, id)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns, TableName,
				Prmetr.ID);

			return new QueryBody(query, sqlParameters);
		}

		public User GetUserMatchingUsername(string username)
		{
			try
			{
				QueryBody queryBody = GetUserMatchingUsernameQuery(username);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.GetUserMatchingUsername", exception);
			}
		}

		private QueryBody GetUserMatchingUsernameQuery(string username)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.USERNAME, username)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE USERNAME = {2}", columns, TableName,
				Prmetr.USERNAME);

			return new QueryBody(query, sqlParameters);
		}

		public User GetUserMatchingUsernameAndPassword(string username, string password)
		{
			try
			{
				QueryBody queryBody = GetUsersMatchUsernameAndPasswordQuery(username, password);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<User> users = new List<User>();
					mssqlDbEngine.ExecuteReaderQuery(users, OnPopulateResultListCallBack);
					return users.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.GetUserMatchingUsernameAndPassword", exception);
			}
		}

		private QueryBody GetUsersMatchUsernameAndPasswordQuery(string username, string password)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.USERNAME, username),
				new SqlParameter(Prmetr.PASSWORD, password)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE USERNAME = {2} AND PASSWORD = {3}", columns,
				TableName,
				Prmetr.USERNAME,
				Prmetr.PASSWORD);

			return new QueryBody(query, sqlParameters);
		}

		private User OnPopulateResultListCallBack(DbDataReader dataReader)
		{
			bool isAdvancedUser = false;
			if (dataReader[Column.IS_ADVANCED_USER] != null &&
				bool.TryParse(dataReader[Column.IS_ADVANCED_USER].ToString(), out bool result))
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

		private void PopulateStandardUser(User user, DbDataReader reader)
		{
			user.Id = reader.GetInt64(Column.ID);
			user.UserName = reader.GetString(Column.USERNAME);
			user.Password = reader.GetString(Column.PASSWORD);
			user.EmailAddress = reader.GetString(Column.EMAIL_ADDRESS);
			user.FirstName = reader.GetString(Column.FIRSTNAME);
			user.Surname = reader.GetString(Column.SURNAME);
			user.DOB = reader.GetNlDateTime(Column.DOB);
			user.Gender = reader.GetString(Column.GENDER);
		}

		private void PopulateAdvancedUser(AdvancedUser advancedUser, DbDataReader dataReader)
		{
			advancedUser.AdvanceStartDatetime = dataReader.GetNlDateTime(Column.ADVANCED_USER_START_DATE);
			advancedUser.AdvanceEndDatetime = dataReader.GetNlDateTime(Column.ADVANCED_USER_END_DATE);
		}

		public void InsertUser(User user)
		{
			try
			{
				QueryBody queryBody = user is AdvancedUser ? GetAdvancedUserInsertQuery(user as AdvancedUser) :
					GetUserInsertQuery(user);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQueryInsertCallback(user, OnPopulateIdCallBack);
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.InsertUser", exception);
			}
		}

		private QueryBody GetUserInsertQuery(User user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.USERNAME, GetDBValue(user.UserName)),
				new SqlParameter(Prmetr.PASSWORD, GetDBValue(user.Password)),
				new SqlParameter(Prmetr.EMAIL_ADDRESS, GetDBValue(user.EmailAddress)),
				new SqlParameter(Prmetr.FIRSTNAME, GetDBValue(user.FirstName)),
				new SqlParameter(Prmetr.SECONDNAME, GetDBValue(user.Surname)),
				new SqlParameter(Prmetr.DOB, GetDBValue(user.DOB)),
				new SqlParameter(Prmetr.GENDER, GetDBValue(user.Gender)),
				new SqlParameter(Prmetr.IS_ADVANCED_USER, GetDBValue(false))
			};

			string insertColumns = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})", TableName,
				Column.USERNAME,
				Column.PASSWORD,
				Column.EMAIL_ADDRESS,
				Column.FIRSTNAME,
				Column.SURNAME,
				Column.DOB,
				Column.GENDER,
				Column.IS_ADVANCED_USER);

			string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
				Prmetr.USERNAME,
				Prmetr.PASSWORD,
				Prmetr.EMAIL_ADDRESS,
				Prmetr.FIRSTNAME,
				Prmetr.SECONDNAME,
				Prmetr.DOB,
				Prmetr.GENDER,
				Prmetr.IS_ADVANCED_USER);

			string query = string.Format("{0} {1}", insertColumns, valueSection);

			return new QueryBody(query, sqlParameters);
		}

		private QueryBody GetAdvancedUserInsertQuery(AdvancedUser user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.USERNAME, GetDBValue(user.UserName)),
				new SqlParameter(Prmetr.PASSWORD, GetDBValue(user.Password)),
				new SqlParameter(Prmetr.EMAIL_ADDRESS, GetDBValue(user.EmailAddress)),
				new SqlParameter(Prmetr.FIRSTNAME, GetDBValue(user.FirstName)),
				new SqlParameter(Prmetr.SECONDNAME, GetDBValue(user.Surname)),
				new SqlParameter(Prmetr.DOB, GetDBValue(user.DOB)),
				new SqlParameter(Prmetr.GENDER, GetDBValue(user.Gender)),
				new SqlParameter(Prmetr.IS_ADVANCED_USER, GetDBValue(true)),
				new SqlParameter(Prmetr.ADVANCED_USER_START_DATE, GetDBValue(user.AdvanceStartDatetime)),
				new SqlParameter(Prmetr.ADVANCED_USER_END_DATE, GetDBValue(user.AdvanceEndDatetime))
			};

			string insertColumns = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
				TableName,
				Column.USERNAME,
				Column.PASSWORD,
				Column.EMAIL_ADDRESS,
				Column.FIRSTNAME,
				Column.SURNAME,
				Column.DOB,
				Column.GENDER,
				Column.IS_ADVANCED_USER,
				Column.ADVANCED_USER_START_DATE,
				Column.ADVANCED_USER_END_DATE);

			string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
				Prmetr.USERNAME,
				Prmetr.PASSWORD,
				Prmetr.EMAIL_ADDRESS,
				Prmetr.FIRSTNAME,
				Prmetr.SECONDNAME,
				Prmetr.DOB,
				Prmetr.GENDER,
				Prmetr.IS_ADVANCED_USER,
				Prmetr.ADVANCED_USER_START_DATE,
				Prmetr.ADVANCED_USER_END_DATE);

			string query = string.Format("{0} {1}", insertColumns, valueSection);

			return new QueryBody(query, sqlParameters);
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
				QueryBody queryBody = user is AdvancedUser ? GetUpdateAdvancedUserQuery(user as AdvancedUser) :
					GetUpdateUserQuery(user);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.UpdateUser", exception);
			}
		}

		private QueryBody GetUpdateUserQuery(User user)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, GetDBValue(user.Id)),
				new SqlParameter(Prmetr.PASSWORD, GetDBValue(user.Password)),
				new SqlParameter(Prmetr.FIRSTNAME, GetDBValue(user.FirstName)),
				new SqlParameter(Prmetr.SECONDNAME, GetDBValue(user.Surname)),
				new SqlParameter(Prmetr.DOB, GetDBValue(user.DOB)),
				new SqlParameter(Prmetr.GENDER, GetDBValue(user.Gender)),
				new SqlParameter(Prmetr.EMAIL_ADDRESS, GetDBValue(user.EmailAddress))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setPassword = string.Format("{0} = {1}", Column.PASSWORD, Prmetr.PASSWORD);
			string setFirstName = string.Format("{0} = {1}", Column.FIRSTNAME, Prmetr.FIRSTNAME);
			string setSecondName = string.Format("{0} = {1}", Column.SURNAME, Prmetr.SECONDNAME);
			string setDob = string.Format("{0} = {1}", Column.DOB, Prmetr.DOB);
			string setGender = string.Format("{0} = {1}", Column.GENDER, Prmetr.GENDER);
			string setEmailAddress = string.Format("{0} = {1}", Column.EMAIL_ADDRESS, Prmetr.EMAIL_ADDRESS);

			string whereId = string.Format("WHERE {0} = {1}", Column.ID, Prmetr.ID);

			string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6} {7}", updateTable,
				setPassword,
				setFirstName,
				setSecondName,
				setDob,
				setGender,
				setEmailAddress,
				whereId);

			return new QueryBody(query, sqlParameters);
		}

		private QueryBody GetUpdateAdvancedUserQuery(AdvancedUser advancedUser)
		{
			SqlParameter[] sqlParameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, GetDBValue(advancedUser.Id)),
				new SqlParameter(Prmetr.PASSWORD, GetDBValue(advancedUser.Password)),
				new SqlParameter(Prmetr.FIRSTNAME, GetDBValue(advancedUser.FirstName)),
				new SqlParameter(Prmetr.SECONDNAME, GetDBValue(advancedUser.Surname)),
				new SqlParameter(Prmetr.DOB, GetDBValue(advancedUser.DOB)),
				new SqlParameter(Prmetr.GENDER, GetDBValue(advancedUser.Gender)),
				new SqlParameter(Prmetr.EMAIL_ADDRESS, GetDBValue(advancedUser.EmailAddress)),
				new SqlParameter(Prmetr.ADVANCED_USER_START_DATE, GetDBValue(advancedUser.AdvanceStartDatetime)),
				new SqlParameter(Prmetr.ADVANCED_USER_END_DATE, GetDBValue(advancedUser.AdvanceEndDatetime))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setPassword = string.Format("{0} = {1}", Column.PASSWORD, Prmetr.PASSWORD);
			string setFirstName = string.Format("{0} = {1}", Column.FIRSTNAME, Prmetr.FIRSTNAME);
			string setSecondName = string.Format("{0} = {1}", Column.SURNAME, Prmetr.SECONDNAME);
			string setDob = string.Format("{0} = {1}", Column.DOB, Prmetr.DOB);
			string setGender = string.Format("{0} = {1}", Column.GENDER, Prmetr.GENDER);
			string setEmailAddress = string.Format("{0} = {1}", Column.EMAIL_ADDRESS, Prmetr.EMAIL_ADDRESS);
			string setAdvanStartDate = string.Format("{0} = {1}", Column.ADVANCED_USER_START_DATE, Prmetr.ADVANCED_USER_START_DATE);
			string setAdvanceEndDate = string.Format("{0} = {1}", Column.ADVANCED_USER_END_DATE, Prmetr.ADVANCED_USER_END_DATE);

			string whereId = string.Format("WHERE {0} = {1}", Column.ID, Prmetr.ID);

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

			return new QueryBody(query, sqlParameters);
		}

		public void DeleteUser(User user)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter(Prmetr.ID, GetDBValue(user.Id))
				};

				string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
					Column.ID,
					Prmetr.ID);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing UserRepository.DeleteUser", exception);
			}
		}

		public void Dispose()
		{
			//
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
				Column.ID,
				Column.USERNAME,
				Column.PASSWORD,
				Column.EMAIL_ADDRESS,
				Column.FIRSTNAME,
				Column.SURNAME,
				Column.DOB,
				Column.GENDER,
				Column.IS_ADVANCED_USER,
				Column.ADVANCED_USER_START_DATE,
				Column.ADVANCED_USER_END_DATE);
			return columns;
		}

		/*protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
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
		}*/
	}
}