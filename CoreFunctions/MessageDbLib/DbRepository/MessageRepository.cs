using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using MessageDbLib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Prmetr = MessageDbLib.Constants.TableConstants.MessageParameter;
using Column = MessageDbLib.Constants.TableConstants.MessageColumn;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
   public class MessageRepository : BaseRepository, IMessageRepository
   {
      public virtual string TableName { get; protected set; } = "messagedbo.MessageTable";

      public MessageRepository(string connectionString) : base(connectionString)
      {
      }

      public MessageRepository(string connectionString, IRepoTransaction repoTransaction) :
         base(connectionString, repoTransaction)
      {
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
            new SqlParameter(Prmetr.ID, messageId)
         };

         string columns = GetSelectColumns();
         string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns, TableName,
            Prmetr.ID);

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
            new SqlParameter(Prmetr.MESSAGE_TEXT, text)
         };

         string columns = GetSelectColumns();
         string query = string.Format("SELECT {0} FROM {1} WHERE MESSAGETEXT = {2}", columns,
            TableName,
            Prmetr.MESSAGE_TEXT);

         return Tuple.Create(query, sqlParameters);
      }

      private Message OnPopulateResultListCallBack(DbDataReader dbDataReader)
      {
         string messagetype = dbDataReader.GetString(Column.MULTI_MEDIA_TTPE);
         return ExtractMessage(messagetype, dbDataReader);
      }

      private Message ExtractMessage(string messagetype, DbDataReader dbDataReader)
      {
         if (string.IsNullOrEmpty(messagetype) ||
            messagetype == MessageTypeConstant.Text)
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
            videoMessage.Length = dbDataReader.GetDouble(Column.LENGTH);
            return videoMessage;
         }

         if (messagetype == MessageTypeConstant.Picture)
         {
            PictureMessage pictureMessage = new PictureMessage();
            PopulateTextMessage(pictureMessage, dbDataReader);
            PopulateMediaMessage(pictureMessage, dbDataReader);
            pictureMessage.ImageType = dbDataReader.GetString(Column.IMAGE_TYPE);
            return pictureMessage;
         }

         return null;
      }

      private void PopulateTextMessage(Message message, DbDataReader dbDataReader)
      {
         message.Id = dbDataReader.GetInt64(Column.ID);
         message.MessageText = dbDataReader.GetString(Column.MESSAGE_TEXT);
         message.SenderId = dbDataReader.GetInt64(Column.SENDER_ID);
         message.SenderEmailAddress = dbDataReader.GetString(Column.SENDER_EMAIL_ADDRESS);
         message.MessageCreated = dbDataReader.GetNlDateTime(Column.MESSAGE_CREATED);
      }

      private void PopulateMediaMessage(IMultiMediaMessage mediaMessage, DbDataReader dbDataReader)
      {
         mediaMessage.UniqueGuid = dbDataReader.GetGuid(Column.UNIQUE_QUID);
         mediaMessage.FileName = dbDataReader.GetString(Column.FILE_NAME);
         mediaMessage.MediaFileType = dbDataReader.GetString(Column.MEDIA_FILE_TYPE);
         mediaMessage.FileSize = dbDataReader.GetNlDouble(Column.FILE_SIZE);
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
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(message.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(message.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(message.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(message.MessageCreated)),
            new SqlParameter(Prmetr.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Text)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5})", TableName,
            Column.MESSAGE_TEXT,
            Column.SENDER_ID,
            Column.SENDER_EMAIL_ADDRESS,
            Column.MESSAGE_CREATED,
            Column.MULTI_MEDIA_TTPE);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4})", Prmetr.MESSAGE_TEXT,
            Prmetr.SENDER_ID,
            Prmetr.SENDER_EMAIL_ADDRESS,
            Prmetr.MESSAGE_CREATED,
            Prmetr.MULTI_MEDIA_TYPE);

         string query = string.Format("{0} {1}", insertColumns, valueSection);
         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetPictureInsertQuery(PictureMessage pictureMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(pictureMessage.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(pictureMessage.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(pictureMessage.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(pictureMessage.MessageCreated)),
            new SqlParameter(Prmetr.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Picture)),
            new SqlParameter(Prmetr.UNIQUE_QUID, GetDBValue(pictureMessage.UniqueGuid)),
            new SqlParameter(Prmetr.FILE_NAME, GetDBValue(pictureMessage.FileName)),
            new SqlParameter(Prmetr.MEDIA_FILE_TYPE, GetDBValue(pictureMessage.MediaFileType)),
            new SqlParameter(Prmetr.FILE_SIZE, GetDBValue(pictureMessage.FileSize)),
            new SqlParameter(Prmetr.IMAGE_TYPE, GetDBValue(pictureMessage.ImageType)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
            TableName,
            Column.MESSAGE_TEXT,
            Column.SENDER_ID,
            Column.SENDER_EMAIL_ADDRESS,
            Column.MESSAGE_CREATED,
            Column.MULTI_MEDIA_TTPE,
            Column.UNIQUE_QUID,
            Column.FILE_NAME,
            Column.MEDIA_FILE_TYPE,
            Column.FILE_SIZE,
            Column.IMAGE_TYPE);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
            Prmetr.MESSAGE_TEXT,
            Prmetr.SENDER_ID,
            Prmetr.SENDER_EMAIL_ADDRESS,
            Prmetr.MESSAGE_CREATED,
            Prmetr.MULTI_MEDIA_TYPE,
            Prmetr.UNIQUE_QUID,
            Prmetr.FILE_NAME,
            Prmetr.MEDIA_FILE_TYPE,
            Prmetr.FILE_SIZE,
            Prmetr.IMAGE_TYPE);

         string query = string.Format("{0} {1}", insertColumns, valueSection);
         return Tuple.Create(query, sqlParameters);
      }

      private Tuple<string, SqlParameter[]> GetVideoInsertQuery(VideoMessage videoMessage)
      {
         SqlParameter[] sqlParameters = new SqlParameter[]
         {
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(videoMessage.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(videoMessage.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(videoMessage.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(videoMessage.MessageCreated)),
            new SqlParameter(Prmetr.MULTI_MEDIA_TYPE, GetDBValue(MessageTypeConstant.Video)),
            new SqlParameter(Prmetr.UNIQUE_QUID, GetDBValue(videoMessage.UniqueGuid)),
            new SqlParameter(Prmetr.FILE_NAME, GetDBValue(videoMessage.FileName)),
            new SqlParameter(Prmetr.MEDIA_FILE_TYPE, GetDBValue(videoMessage.MediaFileType)),
            new SqlParameter(Prmetr.FILE_SIZE, GetDBValue(videoMessage.FileSize)),
            new SqlParameter(Prmetr.LENGTH, GetDBValue(videoMessage.Length)),
         };

         string insertColumns = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
            TableName,
            Column.MESSAGE_TEXT,
            Column.SENDER_ID,
            Column.SENDER_EMAIL_ADDRESS,
            Column.MESSAGE_CREATED,
            Column.MULTI_MEDIA_TTPE,
            Column.UNIQUE_QUID,
            Column.FILE_NAME,
            Column.MEDIA_FILE_TYPE,
            Column.FILE_SIZE,
            Column.LENGTH);

         string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})",
            Prmetr.MESSAGE_TEXT,
            Prmetr.SENDER_ID,
            Prmetr.SENDER_EMAIL_ADDRESS,
            Prmetr.MESSAGE_CREATED,
            Prmetr.MULTI_MEDIA_TYPE,
            Prmetr.UNIQUE_QUID,
            Prmetr.FILE_NAME,
            Prmetr.MEDIA_FILE_TYPE,
            Prmetr.FILE_SIZE,
            Prmetr.LENGTH);

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
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(message.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(message.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(message.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(message.MessageCreated)),
            new SqlParameter(Prmetr.ID, GetDBValue(message.Id)),
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", Column.MESSAGE_TEXT,
            Prmetr.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", Column.SENDER_ID,
            Prmetr.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", Column.SENDER_EMAIL_ADDRESS,
            Prmetr.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", Column.MESSAGE_CREATED,
            Prmetr.MESSAGE_CREATED);

         string whereId = string.Format("WHERE {0} = {1}", Column.ID,
            Prmetr.ID);

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
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(pictureMessage.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(pictureMessage.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(pictureMessage.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(pictureMessage.MessageCreated)),
            new SqlParameter(Prmetr.UNIQUE_QUID, GetDBValue(pictureMessage.UniqueGuid)),
            new SqlParameter(Prmetr.FILE_NAME, GetDBValue(pictureMessage.FileName)),
            new SqlParameter(Prmetr.MEDIA_FILE_TYPE, GetDBValue(pictureMessage.MediaFileType)),
            new SqlParameter(Prmetr.FILE_SIZE, GetDBValue(pictureMessage.FileSize)),
            new SqlParameter(Prmetr.IMAGE_TYPE, GetDBValue(pictureMessage.ImageType)),
            new SqlParameter(Prmetr.ID, GetDBValue(pictureMessage.Id))
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", Column.MESSAGE_TEXT,
            Prmetr.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", Column.SENDER_ID,
            Prmetr.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", Column.SENDER_EMAIL_ADDRESS,
            Prmetr.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", Column.MESSAGE_CREATED,
            Prmetr.MESSAGE_CREATED);

         string setUniqueQuId = string.Format("{0} = {1}", Column.UNIQUE_QUID,
            Prmetr.UNIQUE_QUID);

         string setFileName = string.Format("{0} = {1}", Column.FILE_NAME,
            Prmetr.FILE_NAME);

         string setMediaFileType = string.Format("{0} = {1}", Column.MEDIA_FILE_TYPE,
            Prmetr.MEDIA_FILE_TYPE);

         string setFileSize = string.Format("{0} = {1}", Column.FILE_SIZE,
            Prmetr.FILE_SIZE);

         string setImageType = string.Format("{0} = {1}", Column.IMAGE_TYPE,
            Prmetr.IMAGE_TYPE);

         string whereId = string.Format("WHERE {0} = {1}", Column.ID,
            Prmetr.ID);

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
            new SqlParameter(Prmetr.MESSAGE_TEXT, GetDBValue(videoMessage.MessageText)),
            new SqlParameter(Prmetr.SENDER_ID, GetDBValue(videoMessage.SenderId)),
            new SqlParameter(Prmetr.SENDER_EMAIL_ADDRESS, GetDBValue(videoMessage.SenderEmailAddress)),
            new SqlParameter(Prmetr.MESSAGE_CREATED, GetDBValue(videoMessage.MessageCreated)),
            new SqlParameter(Prmetr.UNIQUE_QUID, GetDBValue(videoMessage.UniqueGuid)),
            new SqlParameter(Prmetr.FILE_NAME, GetDBValue(videoMessage.FileName)),
            new SqlParameter(Prmetr.MEDIA_FILE_TYPE, GetDBValue(videoMessage.MediaFileType)),
            new SqlParameter(Prmetr.FILE_SIZE, GetDBValue(videoMessage.FileSize)),
            new SqlParameter(Prmetr.LENGTH, GetDBValue(videoMessage.Length)),
            new SqlParameter(Prmetr.ID, GetDBValue(videoMessage.Id)),
         };

         string updateTable = string.Format("UPDATE {0} SET", TableName);

         string setText = string.Format("{0} = {1}", Column.MESSAGE_TEXT,
            Prmetr.MESSAGE_TEXT);

         string setSenderId = string.Format("{0} = {1}", Column.SENDER_ID,
            Prmetr.SENDER_ID);

         string setSenderEmailAddress = string.Format("{0} = {1}", Column.SENDER_EMAIL_ADDRESS,
            Prmetr.SENDER_EMAIL_ADDRESS);

         string setMessageCreated = string.Format("{0} = {1}", Column.MESSAGE_CREATED,
            Prmetr.MESSAGE_CREATED);

         string setUniqueQuId = string.Format("{0} = {1}", Column.UNIQUE_QUID,
            Prmetr.UNIQUE_QUID);

         string setFileName = string.Format("{0} = {1}", Column.FILE_NAME,
            Prmetr.FILE_NAME);

         string setMediaFileType = string.Format("{0} = {1}", Column.MEDIA_FILE_TYPE,
            Prmetr.MEDIA_FILE_TYPE);

         string setFileSize = string.Format("{0} = {1}", Column.FILE_SIZE,
            Prmetr.FILE_SIZE);

         string setLength = string.Format("{0} = {1}", Column.LENGTH,
            Prmetr.LENGTH);

         string whereId = string.Format("WHERE {0} = {1}", Column.ID,
            Prmetr.ID);

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
               new SqlParameter(Prmetr.ID, DbValueUtil.GetValidValue(message.Id))
            };

            string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName, Column.ID,
               Prmetr.ID);

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
            Column.ID,
            Column.MESSAGE_TEXT,
            Column.SENDER_ID,
            Column.SENDER_EMAIL_ADDRESS,
            Column.MESSAGE_CREATED,
            Column.MULTI_MEDIA_TTPE,
            Column.UNIQUE_QUID,
            Column.FILE_NAME,
            Column.MEDIA_FILE_TYPE,
            Column.FILE_SIZE,
            Column.IMAGE_TYPE,
            Column.LENGTH);
         return columns;
      }
   }
}

// https://dba.stackexchange.com/questions/176582/whats-the-overhead-of-updating-all-columns-even-the-ones-that-havent-changed
// Answers regarding updating the all columns overhead.