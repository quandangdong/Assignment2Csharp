using BusinessObject;
using System.Collections.Generic;


namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public void DeleteMemberById(int memberId) => MemberDAO.Instance.RemoveMemberById(memberId);

        public MemberObject GetMemberByEmailAndPassword(string email, string password) => MemberDAO.Instance.GetMemberByEmailAndPassword(email, password);
        

        public MemberObject GetMemberById(int memberId) => MemberDAO.Instance.GetMemberById(memberId);


        public IEnumerable<MemberObject> GetMemberList() => MemberDAO.Instance.GetMemberList();


        public void InsertMember(MemberObject member) => MemberDAO.Instance.AddNewMember(member);


        public void Update(MemberObject member) => MemberDAO.Instance.UpdateMember(member);
       
    }
}
