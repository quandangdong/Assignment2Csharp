using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class MemberDAO : BaseDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        //---------------------------------------------------
        public IEnumerable<MemberObject> GetMemberList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, Password, CompanyName, City, Country FROM Member";
            var members = new List<MemberObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    members.Add(new MemberObject
                    {
                        MemberId = dataReader.GetInt32(0),
                        Email = dataReader.GetString(1),
                        Password = dataReader.GetString(2),
                        CompanyName = dataReader.GetString(3),
                        City = dataReader.GetString(4),
                        Country = dataReader.GetString(5),
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return members;
        }

        //---------------------------------------------------
        public MemberObject GetMemberById(int memberId)
        {
            MemberObject member = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, CompanyName, City, Country " +
                " FROM Member where MemberId = @MemberId";
            try
            {
                var param = dataProvider.CreateParameter("@MemberId", 4, memberId, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    member = new MemberObject
                    {
                        MemberId = dataReader.GetInt32(0),
                        Email = dataReader.GetString(1),
                        CompanyName = dataReader.GetString(2),
                        City = dataReader.GetString(3),
                        Country = dataReader.GetString(4),
                    };
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return member;
        }

        //---------------------------------------------------
        public void AddNewMember(MemberObject member)
        {
            try
            {
                MemberObject mem = GetMemberById(member.MemberId);
                if(mem == null)
                {
                    string SQLInsert = "INSERT Member values(@MemberId, @Email, @CompanyName, " +
                        "@City, @Country, @Password)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 4, member.MemberId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Email", 40, member.Email, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CompanyName", 40, member.CompanyName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@City", 40, member.City, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Country", 40, member.Country,DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Password", 40, member.Password, DbType.String));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The Member ID is already exist.");
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public void UpdateMember(MemberObject member)
        {
            try
            {
                MemberObject mem = GetMemberById(member.MemberId);
                if (mem != null)
                {
                    string SQLUpdate = "UPDATE Member SET " +
                        "MemberId = @MemberId," +
                        "Email = @Email," +
                        "CompanyName = @CompanyName," +
                        "City = @City," +
                        "Country = @Country," +
                        "Password = @Password " +
                        "WHERE MemberId = @MemberId";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 4, member.MemberId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Email", 40, member.Email, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CompanyName", 40, member.CompanyName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Country", 40, member.Country, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@City", 40, member.City, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Password", 40, member.Password, DbType.String));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Cannot update member!");
                }
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public void RemoveMemberById(int memberId)
        {
            try
            {
                MemberObject mem = GetMemberById(memberId);
                if (mem != null)
                {
                    string SQLDelete = "DELETE Member WHERE MemberId = @MemberId";
                    var param = dataProvider.CreateParameter("@MemberId", 4, memberId, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Member does not exist to delete!");
                }
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public MemberObject GetMemberByEmailAndPassword(string email, string password)
        {
            MemberObject member = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, CompanyName, City, Country, Password " +
                " FROM Member WHERE Email = @Email AND Password = @Password";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@Email", 40, email, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Password", 40, password, DbType.String));
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, parameters.ToArray());

                if (dataReader.Read())
                {
                    member = new MemberObject
                    {
                        MemberId = dataReader.GetInt32(0),
                        Email = dataReader.GetString(1),
                        CompanyName = dataReader.GetString(2),
                        City = dataReader.GetString(3),
                        Country = dataReader.GetString(4),
                        Password = dataReader.GetString(5)
                    };
                }
            }
            catch (Exception ex)
            {
                member = null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return member;
        }



    }//end class

} //End namespace
