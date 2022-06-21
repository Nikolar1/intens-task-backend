using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace intensTask
{
    class Candidate
    {
        private int id;
        private string fullName;
        private DateTime dateOfBirth;
        private string contactNumber;
        private string email;
        private List<String> skills;

        public Candidate(int id, string fullName, DateTime dateOfBirth, string contactNumber, string email)
        {
            this.id = id;
            this.fullName = fullName;
            this.dateOfBirth = dateOfBirth;
            this.contactNumber = contactNumber;
            this.email = email;
            skills = new List<string>();
        }

        public Candidate(int id, string fullName, DateTime dateOfBirth, string contactNumber, string email, List<string> skills)
        {
            this.fullName = fullName;
            this.dateOfBirth = dateOfBirth;
            this.contactNumber = contactNumber;
            this.email = email;
            this.skills = skills;
        }

        public string FullName { get => fullName;}
        public DateTime DateOfBirth { get => dateOfBirth;}
        public string ContactNumber { get => contactNumber;}
        public string Email { get => email; }
        public int Id { get => id; }

        public string DateToString()
        {
            return "'"+dateOfBirth.Year+"-"+dateOfBirth.Month+"-"+dateOfBirth.Day+" 00:00:00'";
        }

        public void AddSkill(string skill) {
            skills.Add(skill);
        }

        public void RemoveSkill(string skill) {
            skills.Remove(skill);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
