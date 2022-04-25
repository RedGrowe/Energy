using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuzbassEnergo.Model
{
    public class Process
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [NotMapped]
        public Process OwnerProcess { get; set; } = null;

        public Guid OwnerProcessId { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public Repository Repository { get; set; } = null;

        public Guid RepositoryId { get; set; }

        public Process(){}
        public Process(string name,Repository rep)
        {
            Id = Guid.NewGuid();
            Name = name;
            Repository = rep;
            RepositoryId = rep.Id;
        }
        public void SetOwnerProcess(Process process)
        {
            OwnerProcess = process;
            OwnerProcessId = process.Id;
        }
    }
}
