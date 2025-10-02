using System;

namespace lab3vN16
{
    public class Magazine : Publication
    {
        public int IssueNumber { get; set; }

        public Magazine(string title, string author, int issueNumber)
            : base(title, author)
        {
            IssueNumber = issueNumber;
        }

        public override void GetInfo()
        {
            Console.WriteLine($"[Журнал] Назва: {Title}, Редактор: {Author}, Номер випуску: {IssueNumber}");
        }
    }
}
