using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intensTask
{
    class MySQLDatabaseCommunication : IDatabaseCommunication
    {
        private MySqlConnection connection;

        public MySQLDatabaseCommunication(string server, string database, string uid, string password)
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        private bool Connect()
        {
            if (connection == null)
            {
                return false;
            }

            try {
                connection.Open();
                return true;
            } catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private bool Disconnect()
        {
            if (connection == null)
            {
                return false;
            }
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void AddCandidate(Candidate candidate)
        {
            string query = "INSERT INTO Candidates (fullName, dateOfBirth, contactNumber, email) VALUES (\"" + candidate.FullName + "\"," + candidate.DateToString() + ",\"" + candidate.ContactNumber + "\",\"" + candidate.Email + "\")";
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        public void AddSkillToCandidate(List<string> skill, int candidateId)
        {
            string query = "INSERT INTO Skills(candidateId, skill) VALUES (" + candidateId + ",\"" + skill[0] + "\")";
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                int i = 1;
                while (i < skill.Count)
                {
                    command.CommandText = "INSERT INTO Skills(candidateId, skill) VALUES (" + candidateId + ",\"" + skill[i] + "\")";
                    command.ExecuteNonQuery();
                    i++;
                }
                this.Disconnect();
            }
        }

        public void RemoveCandidate(int candidateId)
        {
            string query = "DELETE FROM Skills WHERE candidateId=" + candidateId;
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM Candidates WHERE id=" + candidateId;
                command.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        public void RemoveSkillFromCandidate(string skill, int candidateId)
        {
            string query = "DELETE FROM Skills WHERE candidateId=" + candidateId + " AND skill=\"" + skill + "\"";
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        public List<Candidate> GetCandidates(int lastId) {
            string query = "SELECT * FROM Candidates LIMIT 10";
            return GetCandidatesByQuery(query);

        }

        private List<Candidate> GetCandidatesByQuery(string query)
        {
            List<Candidate> candidates = new List<Candidate>();
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<string> dates = new List<string>();
            List<string> numbers = new List<string>();
            List<string> emails = new List<string>();
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    ids.Add(int.Parse(dataReader["id"] + ""));
                    names.Add(dataReader["fullName"] + "");
                    dates.Add(dataReader["dateOfBirth"] + "");
                    numbers.Add(dataReader["contactNumber"] + "");
                    emails.Add(dataReader["email"] + "");
                }
                dataReader.Close();
                for (int i = 0; i < ids.Count; i++)
                {
                    candidates.Add(new Candidate(ids[i], names[i], DateTime.Parse(dates[i]), numbers[i], emails[i], GetCandidatesSkills(ids[i])));
                }
                Disconnect();
            }
            return candidates;
        }

        private List<string> GetCandidatesSkills(int candidateId)
        {
            List<string> vestine= new List<string>();
            string query = "SELECT skill  FROM  Skills  WHERE candidateId="+candidateId;
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                vestine.Add(dataReader["skill"] + "");
            }
            dataReader.Close();
            return vestine;
        }

        public List<Candidate> SearchCandidates(string name)
        {
            string query = "SELECT id, fullName, dateOfBirth, contactNumber, email FROM Candidates WHERE fullName=\""+name+"\"";
            return GetCandidatesByQuery(query);
        }

        public List<Candidate> SearchCandidates(List<string> skills)
        {
            if (skills== null || skills.Count==0)
            {
                return null;
            }
            string query = "SELECT DISTINCT p.id, fullName, dateOfBirth, contactNumber, email FROM Candidates p, Skills s WHERE p.id=s.candidateId AND (s.skill=\""+skills[0]+"\"";
            for (int i = 1; i < skills.Count; i++)
            {
                query += "OR s.skill = \""+skills[i]+"\"";
            }
            query += ")";
            return GetCandidatesByQuery(query);
        }

        public List<Candidate> SearchCandidates(string name, List<string> skills)
        {
            if (skills == null || skills.Count == 0)
            {
                return SearchCandidates(name);
            }
            string query = "SELECT DISTINCT p.id, fullName, dateOfBirth, contactNumber, email FROM Candidates p, Skills s WHERE fullName=\""+name+"\" AND p.id=s.candidateId AND (s.skill=\"" + skills[0] + "\"";
            for (int i = 1; i < skills.Count; i++)
            {
                query += "OR s.skill = \"" + skills[i] + "\"";
            }
            query += ")";
            return GetCandidatesByQuery(query);
        }

        public void UpdateSkill(string oldSkill, string newSkill, int candidateId) {
            string query = "UPDATE Skills SET skill=\""+newSkill+"\" WHERE skill=\""+oldSkill+ "\" AND candidateId=" + candidateId;
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        public void UpdateCandidate(Candidate candidate, int candidateId)
        {
            string query = "UPDATE Candidates SET fullName=\""+candidate.FullName+"\",dateOfBirth="+candidate.DateToString()+",contactNumber=\""+candidate.ContactNumber+"\",email=\""+candidate.Email+"\" WHERE id="+ candidateId;
            if (Connect() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                this.Disconnect();
            }
        }
    }
}