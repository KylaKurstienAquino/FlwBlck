using DataLayer;
using RuleLayer;

namespace FlwBlck
{
    public class Program
    {
       static void Main(string[] args)
        {
            SqlData sqlData = new SqlData();

            FBRuleLayer _rulelayer = new FBRuleLayer(sqlData);

            Console.WriteLine("Enter your StudentNo: ");
            string studentNo = Console.ReadLine();

            Console.WriteLine("Enter your Password: ");
            string passWord = Console.ReadLine();

             _rulelayer.LogIn(studentNo, passWord);

        }
    }
}