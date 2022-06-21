using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intensTask
{
    interface IDatabaseCommunication
    {
        public List<Candidate> GetCandidates(int lastId);
        public void AddCandidate(Candidate candidate);
        public void RemoveCandidate(int candidateId);
        public void UpdateCandidate(Candidate candidate, int candidateId);
        public void UpdateSkill(string oldSkill, string newSkill, int candidateId);
        public void AddSkillToCandidate(List<string> skill, int candidateId);
        public void RemoveSkillFromCandidate(string skill, int candidateId);
        public List<Candidate> SearchCandidates(string name);
        public List<Candidate> SearchCandidates(List<string> skills);
        public List<Candidate> SearchCandidates(string name, List<string> skills);
    }
}
