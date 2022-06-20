using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.Constants.TableConstants;
using MessageDbLib.DbEngine;
using MessageDbLib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
   public class MessageRepository : IMessageRepository
   {
      protected string connectionString;
      protected readonly IRepoTransaction repoTransaction;
      protected readonly bool transactionModeEnabled = false;

      public virtual string TableName { get; protected set; } = "messagedbo.MessageTable";

      public MessageRepository(string connectionString)
      {
         this.connectionString = connectionString;
      }

      public MessageRepository(string connectionString, IRepoTransaction repoTransaction)
      {
         this.connectionString = connectionString;
         this.repoTransaction = repoTransaction;
         this.transactionModeEnabled = true;
      }

      public Message GetMessageMatchingId(long messageId)
      {
         try
         {
            Tuple<string, SqlParameter[]> query = GetMessageMatchingIdQuery(messageId);

            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2, connectionString))
            {
               List<Message> messages = new List<Message>();
               mssqlDbEngine.ExecuteReaderQuery(messages, OnPopulateResultListCallBack);
               return messages.FirstOrDefault();
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.GetMessageMatchingId", exception);
         }
      }

      private Tuple<string, SqlParameter[]> GetMessageMatchingIdQuery(long messageId)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.ID, messageId)
         };

         string columns = GetSelectColumns();
         string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns, TableName,
            MessageParameter.ID);

         return Tuple.Create(query, sqlParameters);
      }

      public List<Message> GetAllMessages()
      {
         try
         {
            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1}", columns, TableName);

            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query, null, connectionString))
            {
               List<Message> messages = new List<Message>();
               mssqlDbEngine.ExecuteReaderQuery(messages, OnPopulateResultListCallBack);
               return messages;
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.GetAllMessages", exception);
         }
      }

      public List<Message> GetMessagesMatchingText(string text)
      {
         try
         {
            Tuple<string, SqlParameter[]> query = GetMessageMatchingTextQuery(text);
            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
               connectionString))
            {
               List<Message> messages = new List<Message>();
               mssqlDbEngine.ExecuteReaderQuery(messages, OnPopulateResultListCallBack);
               return messages;
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.GetMessageMatchingText", exception);
         }
      }

      private Tuple<string, SqlParameter[]> GetMessageMatchingTextQuery(string text)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, text)
         };

         string columns = GetSelectColumns();
         string query = string.Format("SELECT {0} FROM {1} WHERE MESSAGETEXT = {2}", columns,
            TableName,
            MessageParameter.MESSAGE_TEXT);

         return Tuple.Create(query, sqlParameters);
      }

      private Message OnPopulateResultListCallBack(DbDataReader dbDataReader)
      {
         string messagetype = null;
         if (dbDataReader[MessageColumn.MULTI_MEDIA_TTPE] != null)
         {
            messagetype = dbDataReader[MessageColumn.MULTI_MEDIA_TTPE].ToString();
         }
         return ExtractMessage(messagetype, dbDataReader);
      }

      private Message ExtractMessage(string messagetype, DbDataReader dbDataReader)
      {
         if (string.IsNullOrEmpty(messagetype) || messagetype == MessageTypeConstant.Text)
         {
            Message message = new Message();
            PopulateTextMessage(message, dbDataReader);
            return message;
         }
         if (messagetype == MessageTypeConstant.Video)
         {
            VideoMessage videoMessage = new VideoMessage();
            PopulateTextMessage(videoMessage, dbDataReader);
            PopulateMediaMessage(videoMessage, dbDataReader);

            if (dbDataReader[MessageColumn.LENGTH] != null &&
               double.TryParse(dbDataReader[MessageColumn.LENGTH].ToString(), out double length))
            {
               videoMessage.Length = length;
            }
            return videoMessage;
         }
         if (messagetype == MessageTypeConstant.Picture)
         {
            PictureMessage pictureMessage = new PictureMessage();
            PopulateTextMessage(pictureMessage, dbDataReader);
            PopulateMediaMessage(pictureMessage, dbDataReader);

            if (dbDataReader[MessageColumn.IMAGE_TYPE] != null)
            {
               pictureMessage.ImageType = dbDataReader[MessageColumn.IMAGE_TYPE].ToString();
            }
            return pictureMessage;
         }
         return null;
      }

      private void PopulateTextMessage(Message message, DbDataReader dbDataReader)
      {
         if (dbDataReader[MessageColumn.ID] != null &&
            long.TryParse(dbDataReader[MessageColumn.ID].ToString(), out long id))
         {
            message.Id = id;
         }
         if (dbDataReader[MessageColumn.MESSAGE_TEXT] != null)
         {
            message.MessageText = dbDataReader[MessageColumn.MESSAGE_TEXT].ToString();
         }
         if (dbDataReader[MessageColumn.SENDER_ID] != null &&
            long.TryParse(dbDataReader[MessageColumn.SENDER_ID].ToString(), out long senderId))
         {
            message.SenderId = senderId;
         }
         if (dbDataReader[MessageColumn.SENDER_EMAIL_ADDRESS] != null)
         {
            message.SenderEmailAddress = dbDataReader[MessageColumn.SENDER_EMAIL_ADDRESS].ToString();
         }
         if (dbDataReader[MessageColumn.MESSAGE_CREATED] != null &&
            DateTime.TryParse(dbDataReader[MessageColumn.MESSAGE_CREATED].ToString(), out DateTime createdDate))
         {
            message.MessageCreated = createdDate;
         }
      }

      private void PopulateMediaMessage(IMultiMediaMessage mediaMessage, DbDataReader dbDataReader)
      {
         if (dbDataReader[MessageColumn.UNIQUE_QUID] != null &&
            Guid.TryParse(dbDataReader[MessageColumn.UNIQUE_QUID].ToString(), out Guid guid))
         {
            mediaMessage.UniqueGuid = guid;
         }
         if (dbDataReader[MessageColumn.FILE_NAME] != null)
         {
            mediaMessage.FileName = dbDataReader[MessageColumn.FILE_NAME].ToString();
         }
         if (dbDataReader[MessageColumn.MEDIA_FILE_TYPE] != null)
         {
            mediaMessage.MediaFileType = dbDataReader[MessageColumn.MEDIA_FILE_TYPE].ToString();
         }
         if (dbDataReader[MessageColumn.FILE_SIZE] != null &&
            double.TryParse(dbDataReader[MessageColumn.FILE_SIZE].ToString(), out double fileSize))
         {
            mediaMessage.FileSize = fileSize;
         }
      }

      public void InsertMessage(Message message)
      {
         try
         {
            Tuple<string, SqlParameter[]> query = GetInsertQuery(message);
            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
               connectionString))
            {
               mssqlDbEngine.ExecuteQueryInsertCallback(message, OnPopulateIdCallBack);
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.InsertMessage", exception);
         }
      }

      private Tuple<string, SqlParameter[]> GetInsertQuery(Message message)
      {
         PictureMessage pictureMessage = message as PictureMessage;
         if (pictureMessage != null)
         {
            return GetPictureInsertQuery(pictureMessage);
         }

         VideoMessage videoMessage = message as VideoMessage;
         if (videoMessage != null)
         {
            return GetVideoInsertQuery(videoMessage);
         }
         return GetTextInsertQuery(message);
      }

      private Tuple<string, SqlParameter[]> GetTextInsertQuery(Message message)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(message.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(message.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(message.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(message.MessageCreated)),
            new SqlParameter(MessageParameter.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Text)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5})", TableName,
            MessageColumn.MESSAGE_TEXT,
            MessageColumn.SENDER_ID,
            MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageColumn.MESSAGE_CREATED,
            MessageColumn.MULTI_MEDIA_TTPE);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4})", MessageParameter.MESSAGE_TEXT,
            MessageParameter.SENDER_ID,
            MessageParameter.SENDER_EMAIL_ADDRESS,
            MessageParameter.MESSAGE_CREATED,
            MessageParameter.MULTI_MEDIA_TYPE);

         string query = string.Format("{0} {1}", insertColumns, valueSection);
         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetPictureInsertQuery(PictureMessage pictureMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(pictureMessage.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(pictureMessage.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(pictureMessage.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(pictureMessage.MessageCreated)),
            new SqlParameter(MessageParameter.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Picture)),
            new SqlParameter(MessageParameter.UNIQUE_QUID, GetDBValue(pictureMessage.UniqueGuid)),
            new SqlParameter(MessageParameter.FILE_NAME, GetDBValue(pictureMessage.FileName)),
            new SqlParameter(MessageParameter.MEDIA_FILE_TYPE, GetDBValue(pictureMessage.MediaFileType)),
            new SqlParameter(MessageParameter.FILE_SIZE, GetDBValue(pictureMessage.FileSize)),
            new SqlParameter(MessageParameter.IMAGE_TYPE, GetDBValue(pictureMessage.ImageType)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
            TableName,
            MessageColumn.MESSAGE_TEXT,
            MessageColumn.SENDER_ID,
            MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageColumn.MESSAGE_CREATED,
            MessageColumn.MULTI_MEDIA_TTPE,
            MessageColumn.UNIQUE_QUID,
            MessageColumn.FILE_NAME,
            MessageColumn.MEDIA_FILE_TYPE,
            MessageColumn.FILE_SIZE,
            MessageColumn.IMAGE_TYPE);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
            MessageParameter.MESSAGE_TEXT,
            MessageParameter.SENDER_ID,
            MessageParameter.SENDER_EMAIL_ADDRESS,
            MessageParameter.MESSAGE_CREATED,
            MessageParameter.MULTI_MEDIA_TYPE,
            MessageParameter.UNIQUE_QUID,
            MessageParameter.FILE_NAME,
            MessageParameter.MEDIA_FILE_TYPE,
            MessageParameter.FILE_SIZE,
            MessageParameter.IMAGE_TYPE);

         string query = string.Format("{0} {1}", insertColumns, valueSection);
         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetVideoInsertQuery(VideoMessage videoMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(videoMessage.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(videoMessage.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(videoMessage.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(videoMessage.MessageCreated)),
            new SqlParameter(MessageParameter.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Video)),
            new SqlParameter(MessageParameter.UNIQUE_QUID, GetDBValue(videoMessage.UniqueGuid)),
            new SqlParameter(MessageParameter.FILE_NAME, GetDBValue(videoMessage.FileName)),
            new SqlParameter(MessageParameter.MEDIA_FILE_TYPE, GetDBValue(videoMessage.MediaFileType)),
            new SqlParameter(MessageParameter.FILE_SIZE, GetDBValue(videoMessage.FileSize)),
            new SqlParameter(MessageParameter.LENGTH, GetDBValue(videoMessage.Length)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
            TableName,
            MessageColumn.MESSAGE_TEXT,
            MessageColumn.SENDER_ID,
            MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageColumn.MESSAGE_CREATED,
            MessageColumn.MULTI_MEDIA_TTPE,
            MessageColumn.UNIQUE_QUID,
            MessageColumn.FILE_NAME,
            MessageColumn.MEDIA_FILE_TYPE,
            MessageColumn.FILE_SIZE,
            MessageColumn.LENGTH);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
            MessageParameter.MESSAGE_TEXT,
            MessageParameter.SENDER_ID,
            MessageParameter.SENDER_EMAIL_ADDRESS,
            MessageParameter.MESSAGE_CREATED,
            MessageParameter.MULTI_MEDIA_TYPE,
            MessageParameter.UNIQUE_QUID,
            MessageParameter.FILE_NAME,
            MessageParameter.MEDIA_FILE_TYPE,
            MessageParameter.FILE_SIZE,
            MessageParameter.LENGTH);

         string query = string.Format("{0} {1}", insertColumns, valueSection);
         return Tuple.Create(query, sqlParameters);
      }

      private void OnPopulateIdCallBack(Message message, object result)
      {
         long id = Convert.ToInt64(result);
         message.Id = id;
      }

      public void UpdateMessage(Message message) // Tuple<string, TParameter[]> query where TParameter : IDbDataParameter
      {
         try
         {
            Tuple<string, SqlParameter[]> query = GetUpdateQuery(message);
            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
               connectionString))
            {
               mssqlDbEngine.ExecuteQuery();
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.UpdateMessage", exception);
         }
      }

      private Tuple<string, SqlParameter[]> GetUpdateQuery(Message message)
      {
         PictureMessage pictureMessage = message as PictureMessage;
         if (pictureMessage != null)
         {
            return GetDefaultPictureUpdateQuery(pictureMessage);
         }
         VideoMessage videoMessage = message as VideoMessage;
         if (videoMessage != null)
         {
            return GetDefaultVideoUpdateQuery(videoMessage);
         }
         return GetDefaultTextUpdateQuery(message);
      }

      private Tuple<string, SqlParameter[]> GetDefaultTextUpdateQuery(Message message)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(message.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(message.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(message.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(message.MessageCreated)),
            new SqlParameter(MessageParameter.ID, GetDBValue(message.Id)),
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", MessageColumn.MESSAGE_TEXT,
            MessageParameter.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", MessageColumn.SENDER_ID,
            MessageParameter.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageParameter.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", MessageColumn.MESSAGE_CREATED,
            MessageParameter.MESSAGE_CREATED);

         string whereId = string.Format("WHERE {0} = {1}", MessageColumn.ID,
            MessageParameter.ID);

         string query = string.Format("{0} {1}, {2}, {3}, {4} {5}", updateTable, setText,
            setSenderId,
            setSenderEmailAddress,
            setMessageCreated,
            whereId);

         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetDefaultPictureUpdateQuery(PictureMessage pictureMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(pictureMessage.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(pictureMessage.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(pictureMessage.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(pictureMessage.MessageCreated)),
            new SqlParameter(MessageParameter.UNIQUE_QUID, GetDBValue(pictureMessage.UniqueGuid)),
            new SqlParameter(MessageParameter.FILE_NAME, GetDBValue(pictureMessage.FileName)),
            new SqlParameter(MessageParameter.MEDIA_FILE_TYPE, GetDBValue(pictureMessage.MediaFileType)),
            new SqlParameter(MessageParameter.FILE_SIZE, GetDBValue(pictureMessage.FileSize)),
            new SqlParameter(MessageParameter.IMAGE_TYPE, GetDBValue(pictureMessage.ImageType)),
            new SqlParameter(MessageParameter.ID, GetDBValue(pictureMessage.Id))
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", MessageColumn.MESSAGE_TEXT,
            MessageParameter.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", MessageColumn.SENDER_ID,
            MessageParameter.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageParameter.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", MessageColumn.MESSAGE_CREATED,
            MessageParameter.MESSAGE_CREATED);

         string setUniqueQuId = string.Format("{0} = {1}", MessageColumn.UNIQUE_QUID,
            MessageParameter.UNIQUE_QUID);

         string setFileName = string.Format("{0} = {1}", MessageColumn.FILE_NAME,
            MessageParameter.FILE_NAME);

         string setMediaFileType = string.Format("{0} = {1}", MessageColumn.MEDIA_FILE_TYPE,
            MessageParameter.MEDIA_FILE_TYPE);

         string setFileSize = string.Format("{0} = {1}", MessageColumn.FILE_SIZE,
            MessageParameter.FILE_SIZE);

         string setImageType = string.Format("{0} = {1}", MessageColumn.IMAGE_TYPE,
            MessageParameter.IMAGE_TYPE);

         string whereId = string.Format("WHERE {0} = {1}", MessageColumn.ID,
            MessageParameter.ID);

         string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9} {10}", updateTable,
            setText,
            setSenderId,
            setSenderEmailAddress,
            setMessageCreated,
            setUniqueQuId,
            setFileName,
            setMediaFileType,
            setFileSize,
            setImageType,
            whereId);

         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetDefaultVideoUpdateQuery(VideoMessage videoMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(MessageParameter.MESSAGE_TEXT, GetDBValue(videoMessage.MessageText)),
            new SqlParameter(MessageParameter.SENDER_ID, GetDBValue(videoMessage.SenderId)),
            new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, GetDBValue(videoMessage.SenderEmailAddress)),
            new SqlParameter(MessageParameter.MESSAGE_CREATED, GetDBValue(videoMessage.MessageCreated)),
            new SqlParameter(MessageParameter.UNIQUE_QUID, GetDBValue(videoMessage.UniqueGuid)),
            new SqlParameter(MessageParameter.FILE_NAME, GetDBValue(videoMessage.FileName)),
            new SqlParameter(MessageParameter.MEDIA_FILE_TYPE, GetDBValue(videoMessage.MediaFileType)),
            new SqlParameter(MessageParameter.FILE_SIZE, GetDBValue(videoMessage.FileSize)),
            new SqlParameter(MessageParameter.LENGTH, GetDBValue(videoMessage.Length)),
            new SqlParameter(MessageParameter.ID, GetDBValue(videoMessage.Id)),
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", MessageColumn.MESSAGE_TEXT,
            MessageParameter.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", MessageColumn.SENDER_ID,
            MessageParameter.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageParameter.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", MessageColumn.MESSAGE_CREATED,
            MessageParameter.MESSAGE_CREATED);

         string setUniqueQuId = string.Format("{0} = {1}", MessageColumn.UNIQUE_QUID,
            MessageParameter.UNIQUE_QUID);

         string setFileName = string.Format("{0} = {1}", MessageColumn.FILE_NAME,
            MessageParameter.FILE_NAME);

         string setMediaFileType = string.Format("{0} = {1}", MessageColumn.MEDIA_FILE_TYPE,
            MessageParameter.MEDIA_FILE_TYPE);

         string setFileSize = string.Format("{0} = {1}", MessageColumn.FILE_SIZE,
            MessageParameter.FILE_SIZE);

         string setLength = string.Format("{0} = {1}", MessageColumn.LENGTH,
            MessageParameter.LENGTH);

         string whereId = string.Format("WHERE {0} = {1}", MessageColumn.ID,
            MessageParameter.ID);

         string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9} {10}", updateTable,
            setText,
            setSenderId,
            setSenderEmailAddress,
            setMessageCreated,
            setUniqueQuId,
            setFileName,
            setMediaFileType,
            setFileSize,
            setLength,
            whereId);

         return Tuple.Create(query, sqlParameters);
      }

      public void DeleteMessage(Message message)
      {
         try
         {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
               new SqlParameter(MessageParameter.ID, DbValueUtil.GetValidValue(message.Id))
            };

            string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName, MessageColumn.ID,
               MessageParameter.ID);
            using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
               connectionString))
            {
               mssqlDbEngine.ExecuteQuery();
            }
         }
         catch (Exception exception)
         {
            throw new RepoDbException("Error while executing MessageRepository.DeleteMessage", exception);
         }
      }

      public void Dispose()
      {
         //
      }

      protected static string GetSelectColumns()
      {
         string columns = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
            MessageColumn.ID,
            MessageColumn.MESSAGE_TEXT,
            MessageColumn.SENDER_ID,
            MessageColumn.SENDER_EMAIL_ADDRESS,
            MessageColumn.MESSAGE_CREATED,
            MessageColumn.MULTI_MEDIA_TTPE,
            MessageColumn.UNIQUE_QUID,
            MessageColumn.FILE_NAME,
            MessageColumn.MEDIA_FILE_TYPE,
            MessageColumn.FILE_SIZE,
            MessageColumn.IMAGE_TYPE,
            MessageColumn.LENGTH);
         return columns;
      }

      protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
         string connectionString)
      {
         if (transactionModeEnabled && repoTransaction != null)
         {
            MssqlDbEngine transactionMssqlEngine = new MssqlDbEngine(query, mssqlParameters,
               connectionString,
               repoTransaction);
            return transactionMssqlEngine;
         }

         MssqlDbEngine mssqlDbEngine = new MssqlDbEngine(query, mssqlParameters, connectionString);
         return mssqlDbEngine;
      }

      protected object GetDBValue(object value)
      {
         return DbValueUtil.GetValidValue(value);
      }
   }
}

// https://dba.stackexchange.com/questions/176582/whats-the-overhead-of-updating-all-columns-even-the-ones-that-havent-changed
// Answers regarding updating the all columns overhead.