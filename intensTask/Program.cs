using System;
using System.Collections.Generic;

namespace intensTask
{
    class Program
    {
        static IDatabaseCommunication database;
        static void Main(string[] args)
        {
            database = new MySQLDatabaseCommunication("sql11.freesqldatabase.com", "sql11501209", "sql11501209", "apHgPq8jHW");
            
        }

        void DatabaseUnitTest()
        {
            database.AddCandidate(new Candidate(0, "Backend3", DateTime.Now, "0000000000", "asdasd@afsf.com"));
            List<string> skills = new List<string>();
            skills.Add("C#");
            database.AddSkillToCandidate(skills, 5);
            skills.Add("Russian");
            database.AddSkillToCandidate(skills, 3);
            database.AddSkillToCandidate(skills, 10);
            database.RemoveSkillFromCandidate("C#", 10);
            database.UpdateSkill("Russian", "Chinese", 10);
            List<Candidate> candidates = database.GetCandidates(0);
            Console.WriteLine(candidates[0].Id);
            database.UpdateCandidate(new Candidate(0, "BackendMenjao", DateTime.Now, "0000000000", "asdasd@afsf.com"), 4);
            database.RemoveCandidate(7);
        }
    }
}
