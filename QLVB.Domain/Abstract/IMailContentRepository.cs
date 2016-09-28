using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IMailContentRepository
    {
        IQueryable<MailContent> MailContents { get; }

        int Them(MailContent mail);
    }
}