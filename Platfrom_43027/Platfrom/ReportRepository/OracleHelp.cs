/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 609c6761-855d-496f-886b-1d4b9f353928      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Report.Repository
/////    Project Description:    
/////             Class Name: OracleHelp
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/8/1 14:23:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/8/1 14:23:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
namespace Gsafety.PTMS.Report.Repository
{
    public class OracleHelp
    {
        private string Connectiong = ConfigurationManager.ConnectionStrings["SqlConnection"].ToString();
        private DataTable GetData(string strSql)
        {
            DataSet ds = new DataSet();
            //DbConnection conn = context.Database.Connection;
            DbConnection conn = new SqlConnection(Connectiong);

            try
            {

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlDataAdapter d = new SqlDataAdapter(strSql, conn.ConnectionString);

                d.Fill(ds, "count");

                return ds.Tables["count"];
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                conn.Close();
            }
        }
        private void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] commandParamenters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            if (trans != null)
                command.Transaction = trans;
            if (commandParamenters != null)
            {
                foreach (var item in commandParamenters)
                {
                    command.Parameters.Add(item);
                }
            }
        }
        internal DataTable ExecuteDataTable(string cmdText, params OracleParameter[] commandParameters)
        {
            DbConnection connection = new SqlConnection(Connectiong);

            DbCommand command = new SqlCommand();
            //SqlConnection connection = new SqlConnection(ConnectionString);
            DataTable table = null;
            try
            {
                PrepareCommand(command, connection, null, CommandType.Text, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = (SqlCommand)command;
                table = new DataTable();
                adapter.Fill(table);
                command.Parameters.Clear();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return table;
        }

        internal DataTable ExecuteStoredProcedure(string cmdText, params OracleParameter[] commandParameters)
        {
            DbConnection connection = new SqlConnection(Connectiong);

            DbCommand command = new SqlCommand();
            //SqlConnection connection = new SqlConnection(ConnectionString);
            DataTable table = null;
            try
            {
                PrepareCommand(command, connection, null, CommandType.StoredProcedure, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = (SqlCommand)command;
                table = new DataTable();
                adapter.Fill(table);
                command.Parameters.Clear();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return table;
        }

        public string GetSqlText(string cmdText, params OracleParameter[] commandParameters)
        {
            DbConnection connection = new SqlConnection(Connectiong);

            DbCommand command = new SqlCommand();
            //SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                PrepareCommand(command, connection, null, CommandType.StoredProcedure, cmdText, commandParameters);

                command.ExecuteNonQuery();

                string sqltext = (string)command.Parameters["str_sql"].Value;
                command.Parameters.Clear();
                return sqltext;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            return string.Empty;
        }
    }
}
