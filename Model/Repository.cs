using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuzbassEnergo.Model
{
    public class Repository
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [NotMapped]
        public Repository OwnerRepository { get; set; } = null;

        public Guid OwnerRepositoryId { get; set; }
        [NotMapped]
        public Process Process { get; set; } = null;
        public string Name { get; set; }

        [NotMapped]
        public List<SubDivision> ProcessOwner { get; set; } = new();

        [Column(TypeName ="jsonb")]
        public string DivisionGroup { get; set; }

        public Repository(){}
        public Repository(string name,List<SubDivision> pOwner = null)
        {
            Name = name;
            ProcessOwner = pOwner;
        }

        public void ConverProcessOwnerList()
        {
            if (ProcessOwner != null && ProcessOwner.Count >= 0)
                DivisionGroup = JsonConvert.SerializeObject(ProcessOwner);
        }
        public void AddOwnerRepository(Repository repository)
        {
            OwnerRepository = repository;
            OwnerRepositoryId = repository.Id;
        }

        public void SetProcess(Process pro)
        {
            Process = pro;
        }

        public bool HasProcess()
        {
            if (Process == null)
                return false;
            else
                return true;
        }

    }
}
