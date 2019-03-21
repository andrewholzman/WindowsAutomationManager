using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AM_History
{
    public class JobHistoryManager
    {
        private readonly string _connStr = "Server=localhost;database=WAM; Integrated Security=SSPI";

        public List<HistoryModel> GetAllJobHistory()
        {
            List<HistoryModel> jobHistory = new List<HistoryModel>();
            SqlCommand cmd = GetDbcommand();
            cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory ORDER BY DateRan DESC";
            //cmd.Parameters.AddWithValue("@Id", newGuid);
            try
            {
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    HistoryModel m = BuildHistoryItem(reader);
                    jobHistory.Add(m);
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve JobHistory. Error: {ex.Message}");
            }
            return jobHistory;
        }

        public List<HistoryModel> GetJobHistoryForResult(string result)
        {
            List<HistoryModel> jobHistory = new List<HistoryModel>();
            SqlCommand cmd = GetDbcommand();
            cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE Status = @result ORDER BY DateRan DESC";
            cmd.Parameters.AddWithValue("@result", result);
            try
            {
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    HistoryModel m = BuildHistoryItem(reader);
                    jobHistory.Add(m);
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve JobHistory. Error: {ex.Message}");
            }
            return jobHistory;
        }

        public List<HistoryModel> GetJobHistoryForJobName(string jobName, string result=null)
        {
            List<HistoryModel> jobHistory = new List<HistoryModel>();
            SqlCommand cmd = GetDbcommand();
            cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobType = @jobName ORDER BY DateRan DESC";
            if (result != null)
            {
                cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobType = @jobName AND Status = @result ORDER BY DateRan DESC";
                cmd.Parameters.AddWithValue("@result", result);
            } 
            cmd.Parameters.AddWithValue("@jobName", jobName);
            try
            {
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    HistoryModel m = BuildHistoryItem(reader);
                    jobHistory.Add(m);
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve JobHistory. Error: {ex.Message}");
            }
            return jobHistory;
        }

        public List<HistoryModel> GetJobHistoryForJobType(string jobType, string result = null)
        {
            List<HistoryModel> jobHistory = new List<HistoryModel>();
            SqlCommand cmd = GetDbcommand();
            
            if (jobType == "Custom")
            {
                if (result != null)
                {
                    cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobName in ('VBS','BAT','PS') AND Status = @result ORDER BY DateRan DESC";
                    cmd.Parameters.AddWithValue("@result", result);
                }
                else
                {
                    cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobName in ('BAT','VBS','PS') ORDER BY DateRan DESC";
                }
            } else
            {
                cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobName in (@jobType) ORDER BY DateRan DESC";
                if (result != null)
                {
                    cmd.CommandText = @"SELECT * FROM WAM.dbo.JobHistory WHERE JobName in (@jobType) AND Status = @result ORDER BY DateRan DESC";
                    cmd.Parameters.AddWithValue("@result", result);
                }
                cmd.Parameters.AddWithValue("@jobType", jobType);
            }
            try
            {
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    HistoryModel m = BuildHistoryItem(reader);
                    jobHistory.Add(m);
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve JobHistory. Error: {ex.Message}");
            }
            return jobHistory;
        }

        private HistoryModel BuildHistoryItem(SqlDataReader reader)
        {
            HistoryModel m = new HistoryModel(
                (Guid)reader["Id"],
                (string)reader["JobType"],
                (string)reader["JobName"],
                (string)reader["DateRan"],
                (string)reader["Status"]
                );
            if (!(reader["Error"] is DBNull))
            {
                m.Error = (string)reader["Error"];
            }
            return m;
        }

        private SqlCommand GetDbcommand()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connStr;
            return conn.CreateCommand();
        }
    }
}
