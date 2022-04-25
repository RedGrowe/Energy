using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuzbassEnergo.Model
{
    public class SubDivision
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; }

        public SubDivision(){}
        public SubDivision(string name)
        {
            Name = name;
        }
    }
}
