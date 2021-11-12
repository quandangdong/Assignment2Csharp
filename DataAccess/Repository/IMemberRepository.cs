using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        IEnumerable<MemberObject> GetMemberList();
        MemberObject GetMemberById(int memberId);
        MemberObject GetMemberByEmailAndPassword(string email, string password);
        void InsertMember(MemberObject member);
        void DeleteMemberById(int memberId);
        void Update(MemberObject member);
    }
}
